namespace QueueDEMO.QueueInterfaces
{
    public interface IAzureStorageQueue
    {
        Task CreateMessage(string message);
        Task<string> PeekMessage();
        Task DeleteMessage();
    }
}
