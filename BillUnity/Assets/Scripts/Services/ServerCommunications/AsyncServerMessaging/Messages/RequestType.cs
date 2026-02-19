
namespace Kborod.Services.ServerCommunication.AsyncServerMessaging.Messages
{
    public enum RequestType
    {
        ResponseReceived,
        ResendLastResponses,
        LastRequestsList,
        IamAlive,
        StartSession,
        Test
    }
}
