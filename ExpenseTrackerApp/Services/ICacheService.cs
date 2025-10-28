namespace ExpenseTrackerApp.Services
{
    public interface ICacheService
    {
        /// <summary>
        /// Store a value with an optional expiration. If expiration is null the item does not expire.
        /// </summary>
        Task SetAsync<T>(string key, T value, TimeSpan? absoluteExpirationRelativeToNow = null);

        /// <summary>
        /// Get a value. Returns default(T) if not found or expired.
        /// </summary>
        Task<T?> GetAsync<T>(string key);

        /// <summary>
        /// Remove an entry (memory + persistent).
        /// </summary>
        Task RemoveAsync(string key);

    }
}
