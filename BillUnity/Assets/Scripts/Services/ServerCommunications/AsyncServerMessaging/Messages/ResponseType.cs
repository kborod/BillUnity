
namespace Kborod.Services.ServerCommunication.AsyncServerMessaging.Messages
{
    public enum ResponseType
    {
        MessageReceivedResponse,
        ResendLastRequestsResponse,
        AreYouAliveResponse,
        LastResponsesListResponse,
        SessionErrorResponse,
        ErrorResponse,
        TestResponse,
    }
}
