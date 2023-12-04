using Microsoft.ApplicationInsights.Channel;
using Microsoft.ApplicationInsights.Metrics;
using Microsoft.ApplicationInsights;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static AzureLoggingFunction.Helpers.EnumHelper;

namespace AzureLoggingFunction.Services.TelemetryRepository
{
    public interface ITelemetryRepository
    {

        public void UploadLogEntities();
        public void AddLogEntityToList(string module, string customLog, Severity severity);
        public void UploadMetric<T, Y>(string metricName, string dimensionName, T trackValue, Y dimensionValue);
        public void UploadMultiDimensionalMetric<T>(string metricNamespace, string metricName, Dictionary<string, string> metricDimensions, T metricValue);
        public void FlushTelemetry();

    }
}
