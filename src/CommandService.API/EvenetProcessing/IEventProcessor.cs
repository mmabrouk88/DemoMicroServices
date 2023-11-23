namespace CommandService.API.EvenetProcessing
{
    public interface IEventProcessor
    {
        void ProcessEvent(string message);
    }
}
