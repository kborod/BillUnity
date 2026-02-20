
namespace Kborod.SharedDto.AsyncServerMessaging.Messages
{
    public interface IRequest
    {
        /// <summary>
        /// Обязательно должен быть доставлен
        /// </summary>
        bool IsRequired { get; }
        /// <summary>
        /// Тип сообщения
        /// </summary>
        RequestType RequestType { get; }
    }
}
