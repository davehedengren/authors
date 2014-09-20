using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Configuration;
using JWinston.ExternalAPIs.JWinston.ExternalAPIs;

namespace JWinston.ExternalAPIs
{
    public class CacheManager : ICacheData
    {
        private static object _lockObject = new object();
        private int _secondsToCacheData;
        public CacheManager(int secondsToCacheData)
        {
            _secondsToCacheData = secondsToCacheData;
        }

        private static CacheManager _internalManager = null;

        public static CacheManager GetCacheManager(int secondsToCacheData)
        {
            if (null == _internalManager)
            {
                lock(_lockObject)
                {
                    if (null == _internalManager)
                    {
                        _internalManager = new CacheManager(secondsToCacheData);
                    }
                }
            }
            return _internalManager;
        }

        public string GetCacheValue(string key)
        {
            string returnData = null;
            if (null != HttpContext.Current.Cache[key])
            {
                returnData = HttpContext.Current.Cache[key].ToString();
            }
            return returnData;
        }

        public void InsertToCache(string key, string value)
        {
            lock (_lockObject)
            {
                if (null == HttpContext.Current.Cache.Get(key))
                {
                    HttpContext.Current.Cache.Add(
                        key
                        , value
                        , null
                        , DateTime.Now.Add(TimeSpan.FromSeconds(_secondsToCacheData))
                        , System.Web.Caching.Cache.NoSlidingExpiration
                        , System.Web.Caching.CacheItemPriority.Low
                        , null
                        );
                }
            }
        }
    }
}