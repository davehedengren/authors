using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Configuration;

namespace JWinston.ExternalAPIs.JWinston.ExternalAPIs
{
    public class CommonWebConfiguration: IConfigurationManager
    {
        public string GetConfigurationByKey(string key)
        {
            return WebConfigurationManager.AppSettings[key];   
        }
    }
}