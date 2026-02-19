using Kborod.Services.ServerCommunication.AsyncServerMessaging.Messages;
using System;

namespace Kborod.Services.ServerCommunication.AsyncServerMessaging
{
    public static class Extensions
    {
        public static T GetPayload<T>(this ResponseEnvelope response)
        {
            return response.Payload.ToObject<T>();
        }
        public static IResponse GetPayload(this ResponseEnvelope response)
        {
            return response.ResponseType switch
            {
                ResponseType.MessageReceivedResponse => response.GetPayload<MessageReceivedResponseDto>(),
                ResponseType.ResendLastRequestsResponse => response.GetPayload<ResendLastMessagesResponseDto>(),
                ResponseType.AreYouAliveResponse => response.GetPayload<AreYouAliveResponseDto>(),
                ResponseType.LastResponsesListResponse => response.GetPayload<LastMessagesResponseDto>(),
                ResponseType.SessionErrorResponse => response.GetPayload<SessionErrorResponseDto>(),
                ResponseType.ErrorResponse => response.GetPayload<ErrorResponseDto>(),
                ResponseType.TestResponse => response.GetPayload<TestResponseDto>(),
                _ => throw new NotImplementedException()
            };
        }
    }
}