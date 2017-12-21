using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HiveAPI
{

    public class AppSettings
    {
        public Logging Logging { get; set; }
        public ConnectionStrings ConnectionStrings { get; set; }
        public string SecurityKey { get; set; }
    }

    public class LogLevel
    {
        public string Default { get; set; }
    }

    public class Logging
    {
        public bool IncludeScopes { get; set; }
        public LogLevel LogLevel { get; set; }
    }

    public class ConnectionStrings
    {
        public string DefaultConnection { get; set; }
    }   
}
