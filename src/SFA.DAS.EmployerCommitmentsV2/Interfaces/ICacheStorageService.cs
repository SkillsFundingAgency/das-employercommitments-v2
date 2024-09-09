namespace SFA.DAS.EmployerCommitmentsV2.Interfaces
{
    public interface ICacheStorageService
    {
        Task<T> RetrieveFromCache<T>(string key);
        Task SaveToCache<T>(string key, T item, TimeSpan timeSpan);
        Task SaveToCache<T>(string key, T item, int expirationInHours);
        Task SaveToCache<T>(Guid key, T item, int expirationInHours);
        Task SaveToCache<T>(T item, int expirationInHours) where T : ICacheModel;
        Task DeleteFromCache(string key);
    }
}
