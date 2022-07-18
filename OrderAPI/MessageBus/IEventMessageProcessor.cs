namespace OrderAPI.MessageBus
{
    public interface IEventMessageProcessor
    {
        Task ProcessNotification(string Message);
    }
}