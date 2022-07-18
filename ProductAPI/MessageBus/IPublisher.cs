namespace ProductAPI.MessageBus
{
    public interface IPublisher
    {
        void SendMessage(string message);
    }
}