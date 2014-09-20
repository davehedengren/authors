using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Configuration;

namespace JWinston.ExternalAPIs.JWinston.ExternalAPIs
{
    interface IConfigurationManager
    {
        string GetConfigurationByKey(string key);
    }
}
