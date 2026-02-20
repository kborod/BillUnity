
namespace Kborod.SharedDto.AsyncServerMessaging.Messages
{
    public enum RequestType
    {
        Test = 0,

        ResponseReceived = 101,
        ResendLastResponses = 102,
        LastRequestsList = 103,
        IamAlive = 104,
        StartSession = 105,

        WantMatch = 201,
        WantMatchCancel = 202,
    }
}
