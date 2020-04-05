using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HRM.Helpers
{
    public class MongoDbSetting: IMongoDbSetting
    {
        public string ConnectionString { get; set; }
        public string Database { get; set; }
        public string StorageDatabase { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public string AuthMechanism { get; set; }
        public string Host { get; set; }
        public int Port { get; set; }
    }
    public interface IMongoDbSetting
    {
        public string ConnectionString { get; set; }
        public string Database { get; set; }
        public string StorageDatabase { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public string AuthMechanism { get; set; }
        public string Host { get; set; }
        public int Port { get; set; }
    }
}
