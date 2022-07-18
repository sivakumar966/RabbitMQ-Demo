namespace OrderAPI.MessageBus
{
    public interface IPublisher
    {
        void SendMessage(string message);
    }
}