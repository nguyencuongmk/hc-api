using Microsoft.Extensions.Caching.Distributed;

namespace HC.Foundation.Common.Extensions
{
    public static class DistributedCacheExtension
    {
        public async static Task SetAsync(this IDistributedCache distributedCache, string key, object value, TimeSpan expriationTimeSpan, CancellationToken token = default)
        {
            await distributedCache.SetAsync(key, value.ToByteArray(), new DistributedCacheEntryOptions() { AbsoluteExpirationRelativeToNow = expriationTimeSpan }, token);
        }
        public async static Task<T> GetAsync<T>(this IDistributedCache distributedCache, string key, CancellationToken token = default) where T : class
        {
            var result = await distributedCache.GetAsync(key, token);
            return result.FromByteArray<T>();
        }
    }
}