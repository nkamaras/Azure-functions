using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AzureLoggingFunction.Helpers
{
    public static class EnumHelper
    {

        public enum Severity
        {

            Debug = 0,
            Information = 1,
            Warning = 2,
            Error = 3,
            Critical = 4
        }

    }
}
