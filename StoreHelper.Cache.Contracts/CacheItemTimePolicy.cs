using System;

namespace StoreHelper.Cache.Contracts
{
    public sealed class CacheItemTimePolicy
    {
        public CacheItemTimePolicy()
        {
            RenewOnAccess = true;
            ValidTill = DateTime.UtcNow.AddHours(4);
        }

        /// <summary>
        /// Initialize new time policy
        /// </summary>
        /// <param name="validTill">Time in UTC format </param>
        public CacheItemTimePolicy(DateTime validTill)
        {
            ValidTill = validTill;
        }

        public bool RenewOnAccess { get; set; }

        /// <summary>
        /// Indicated time frame where cache item will be invalidated
        /// </summary>
        public DateTime ValidTill { get; set; }

        public static CacheItemTimePolicy Default => new CacheItemTimePolicy(DateTime.UtcNow.AddHours(4));
    }
}
