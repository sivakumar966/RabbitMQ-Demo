namespace ReportAPI.MessageBus
{
    public interface IEventMessageProcessor
    {
        Task ProcessNotification(string Message);
    }
}