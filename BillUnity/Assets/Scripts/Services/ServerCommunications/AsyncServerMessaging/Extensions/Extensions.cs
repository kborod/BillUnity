using Kborod.SharedDto.AsyncServerMessaging.Messages;
using System;
using UnityEngine;

namespace Kborod.Services.ServerCommunication.AsyncServerMessaging
{
    public static class Extensions
    {
        public static T GetPayload<T>(this ResponseEnvelope response)
        {
            return response.Payload.ToObject<T>();
        }

        //public static IResponse GetPayload(this ResponseEnvelope response)
        //{
        //    IResponse result = response.ResponseType switch
        //    {
        //        ResponseType.ErrorResponse => response.GetPayload<ErrorResponseDto>(),
        //        ResponseType.TestResponse => response.GetPayload<TestResponseDto>(),

        //        ResponseType.MessageReceivedResponse => response.GetPayload<MessageReceivedResponseDto>(),
        //        ResponseType.ResendLastRequestsResponse => response.GetPayload<ResendLastMessagesResponseDto>(),
        //        ResponseType.AreYouAliveResponse => response.GetPayload<AreYouAliveResponseDto>(),
        //        ResponseType.LastResponsesListResponse => response.GetPayload<LastMessagesResponseDto>(),
        //        ResponseType.SessionErrorResponse => response.GetPayload<SessionErrorResponseDto>(),

        //        ResponseType.AddedToQueueResponse => response.GetPayload<AddedToQueueResponseDto>(),
        //        ResponseType.SearchCancelledResponse => response.GetPayload<SearchCancelledResponseDto>(),
        //        ResponseType.MatchStartedResponse => response.GetPayload<MatchStartedResponseDto>(),
        //        ResponseType.AimInfoResponse => response.GetPayload<AimInfoResponseDto>(),
        //        ResponseType.MakeShotResponse => response.GetPayload<MakeShotResponseDto>(),
        //        ResponseType.StartTurnResponse => response.GetPayload<StartTurnResponseDto>(),
        //        ResponseType.MatchOverResponse => response.GetPayload<MatchOverResponseDto>(),

        //        _ => null,
        //    };

        //    if (result == null)
        //    {
        //        Debug.LogError($"Unknown response type: {response.ResponseType}");
        //        throw new NotImplementedException($"Unknown response type: {response.ResponseType}");
        //    }

        //    return result;
        //}
    }
}