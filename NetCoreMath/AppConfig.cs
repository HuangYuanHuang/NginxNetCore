using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NetCoreMath
{
    public class AppConfig
    {
        public string WebHost { get; set; }

        public string HostName { get; set; }

        public MongoConfig MongoConfig { get; set; }

        public SignalrConfig SignalrConfig { get; set; }
    }

    public class MongoConfig
    {
        public string ConnectString { get; set; }

        public string DBName { get; set; }
    }

    public class SignalrConfig
    {
        public string Url { get; set; }

        public string MethodName { get; set; }
    }

}
