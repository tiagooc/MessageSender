namespace MessageSender.Repository;

public interface IRepository
{
    public Task<bool> Set<T>(string key, T value);

    public Task<T?> Get<T>(string key);
    
    public Task Delete(string key);
}