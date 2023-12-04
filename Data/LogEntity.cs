using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AzureLoggingFunction.Data
{
    public class LogEntity
    {
        public int severity { get; set; }
        public string LogMessage { get; set; }
        public string module { get; set; }
    }
}
