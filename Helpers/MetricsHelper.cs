using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AzureLoggingFunction.Helpers
{
    public static class MetricsHelper {

        public static double GetExecutionTime(DateTime startTime, DateTime endTime)
        {
            TimeSpan elapsed = endTime - startTime;
            return Math.Round(TimeSpan.FromMilliseconds(elapsed.TotalMilliseconds).TotalMinutes, 2);
        }

        public static int Totalexceptions(int totalException)
        {
            return totalException += 1;
        }

    }
}
