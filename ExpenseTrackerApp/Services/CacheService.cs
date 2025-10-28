using System.Security.Cryptography;
using System.Text;
using System.Text.Json;

namespace ExpenseTrackerApp.Services
{
    internal class PrefCacheItemDto
    {
        public DateTimeOffset? ExpirationUtc { get; set; }
        public string SerializedValue { get; set; } = string.Empty;
        public string TypeName { get; set; } = string.Empty;
    }


    public class PreferenceCacheService : ICacheService
    {
        private readonly JsonSerializerOptions _jsonOptions = new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            WriteIndented = false
        };

          public Task SetAsync<T>(string key, T value, TimeSpan? absoluteExpirationRelativeToNow = null)
        {
            if (key is null) throw new ArgumentNullException(nameof(key));

            var json = JsonSerializer.Serialize(value, _jsonOptions);
            Preferences.Set(GetPrefKeyFor(key), json);
            return Task.CompletedTask;
        }

        public Task<T?> GetAsync<T>(string key)
        {
            if (key is null) throw new ArgumentNullException(nameof(key));

            var prefKey = GetPrefKeyFor(key);
            if (!Preferences.ContainsKey(prefKey))
                return Task.FromResult<T?>(default);

            var json = Preferences.Get(prefKey, string.Empty);
            if (string.IsNullOrEmpty(json))
                return Task.FromResult<T?>(default);

            try
            {
                var val = JsonSerializer.Deserialize<T>(json, _jsonOptions);
                return Task.FromResult(val);
            }
            catch
            {
                 return Task.FromResult<T?>(default);
            }
        }

        /// <summary>
        /// Remove an entry from Preferences.
        /// </summary>
        public Task RemoveAsync(string key)
        {
            if (key is null) throw new ArgumentNullException(nameof(key));
            Preferences.Remove(GetPrefKeyFor(key));
            return Task.CompletedTask;
        }

        private static string GetPrefKeyFor(string key)
        {
            // SHA256-based safe preference key
            using var sha = SHA256.Create();
            var hash = sha.ComputeHash(Encoding.UTF8.GetBytes(key));
            return "prefcache::" + Base64UrlEncode(hash);
        }

        private static string Base64UrlEncode(byte[] input)
        {
            return Convert.ToBase64String(input).Replace('+', '-').Replace('/', '_').TrimEnd('=');
        }

    }
}