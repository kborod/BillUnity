
namespace Kborod.Services.ServerCommunication.Sockets.Messages
{
    public interface IResponseMeta
    {
        /// <summary>
        /// Обязательно должен быть доставлен
        /// </summary>
        bool IsRequired { get; }
        /// <summary>
        /// Тип сообщения
        /// </summary>
        ResponseType ResponseType { get; }
    }
}
