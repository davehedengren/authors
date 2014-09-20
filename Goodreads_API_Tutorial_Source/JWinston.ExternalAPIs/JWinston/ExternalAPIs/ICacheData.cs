using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace JWinston.ExternalAPIs
{
    interface ICacheData
    {
        string GetCacheValue(string key);
        void InsertToCache(string key, string value);
    }
}
