
namespace Kborod.SharedDto.AsyncServerMessaging.Messages
{
    public enum ResponseType
    {
        TestResponse = 0,
        ErrorResponse = 1,

        MessageReceivedResponse = 101,
        ResendLastRequestsResponse = 102,
        LastResponsesListResponse = 103,
        AreYouAliveResponse = 104,
        SessionErrorResponse = 105,

        AddedToQueueResponse = 201,
        MatchStartedResponse = 202,
    }
}
