
namespace Kborod.SharedDto.AsyncServerMessaging.Messages
{
    public interface IResponse
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
