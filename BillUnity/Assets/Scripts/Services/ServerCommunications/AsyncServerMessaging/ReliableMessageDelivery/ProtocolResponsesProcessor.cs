using Kborod.Services.ServerCommunication.AsyncServerMessaging.Messages;
using Kborod.Utils;
using System.Collections.Generic;
using System.Linq;

namespace Kborod.Services.ServerCommunication.AsyncServerMessaging.ReliableMessageDelivery
{
    public class ProtocolResponsesProcessor
    {
        public Result<List<ResponseEnvelope>> GetValidatedResponses(ResponseEnvelope response, SessionInfo sessionMessages, IMessagingTransport messagingTransport)
        {
            List<ResponseEnvelope> responses = new List<ResponseEnvelope>() { response };
            if (response.ResponseType == ResponseType.SessionErrorResponse)
            {
                return Result<List<ResponseEnvelope>>.Fail("Session not created on server");
            }
            if (response.ResponseType == ResponseType.ResendLastRequestsResponse)
            {
                ResendLastRequests(response, sessionMessages, messagingTransport);
                return Result<List<ResponseEnvelope>>.Ok(null);
            }
            
            if (response.ResponseType == ResponseType.MessageReceivedResponse)
            {
                ServerReceivedRequestHandler(response, sessionMessages);
                return Result<List<ResponseEnvelope>>.Ok(null);
            }
            
            if (response.ResponseType == ResponseType.AreYouAliveResponse)
            {
                messagingTransport.SendRequest(RequestEnvelope.Create(new IamAliveDto()));
                return Result<List<ResponseEnvelope>>.Ok(null);
            }
            
            if(response.ResponseType == ResponseType.LastResponsesListResponse)
            {
                responses = response.GetPayload<LastMessagesResponseDto>()?.LastResponses;

                if (responses.Min(r => r.SequenceNumber) > sessionMessages.LastReceivedResponseNumber + 1)
                    return Result<List<ResponseEnvelope>>.Fail("Lost responses not found on Server");
            }
            else if (!response.IsRequired)
            {
                return Result<List<ResponseEnvelope>>.Ok(responses);
            }

            if (IsOrderCorrect(responses, sessionMessages) == false)
            {
                messagingTransport.SendRequest(RequestEnvelope.Create(new ResendLastMessagesRequestDto(sessionMessages.LastReceivedResponseNumber + 1)));
                return Result<List<ResponseEnvelope>>.Ok(null);
            }

            responses = GetUnprocessedRequests(responses, sessionMessages);

            messagingTransport.SendRequest(RequestEnvelope.Create(new MessageReceivedRequestDto(sessionMessages.LastReceivedResponseNumber)));

            return Result<List<ResponseEnvelope>>.Ok(responses);
        }

        private void ResendLastRequests(ResponseEnvelope response, SessionInfo sessionMessages, IMessagingTransport messagingTransport)
        {
            var fromNumber = response.GetPayload<ResendLastMessagesResponseDto>()!.FromNumberInclusive;
            var requests = sessionMessages.GetRequestsFromNumber(fromNumber);
            messagingTransport.SendRequest(RequestEnvelope.Create(new LastMessagesRequestDto(requests)));
        }

        private void ServerReceivedRequestHandler(ResponseEnvelope request, SessionInfo sessionMessages)
        {
            var payload = request.GetPayload<MessageReceivedResponseDto>();
            sessionMessages.RemoveRequestsBeforeNumber(payload.LastReceivedRequest);
        }

        private bool IsOrderCorrect(List<ResponseEnvelope> responses, SessionInfo sessionMessages)
        {
            if (responses == null || responses.Count == 0)
                return true;

            for (int i = 0; i < responses.Count - 1; i++)
            {
                if (responses[i].SequenceNumber + 1 != responses[i + 1].SequenceNumber)
                    return false;
            }

            var minNumberInList = responses.Min(r => r.SequenceNumber);

            return sessionMessages.LastReceivedResponseNumber + 1 >= minNumberInList;
        }

        private List<ResponseEnvelope> GetUnprocessedRequests(List<ResponseEnvelope> responsesList, SessionInfo sessionMessages)
        {
            if (responsesList == null || responsesList.Count == 0)
                return null;

            var maxNumberInList = responsesList.Max(r => r.SequenceNumber);
            var currentReceivedNumber = sessionMessages.LastReceivedResponseNumber;

            if (maxNumberInList <= currentReceivedNumber)
                return null;

            sessionMessages.SetLastReceivedResponseNumber(maxNumberInList);

            return responsesList
                .Where(r => r.SequenceNumber > currentReceivedNumber)
                .OrderBy(r => r.SequenceNumber)
                .ToList();
        }
    }
}