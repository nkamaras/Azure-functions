using Microsoft.ApplicationInsights.Metrics;
using Microsoft.ApplicationInsights;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static AzureLoggingFunction.Helpers.EnumHelper;
using AzureLoggingFunction.Data;

namespace AzureLoggingFunction.Services.TelemetryRepository
{
    public class TelemetryRepository : ITelemetryRepository
    {
        private readonly IConfiguration _configuration;
        private ILogger<TelemetryRepository> _logger;

        TelemetryClient telemetry = new TelemetryClient();

        List<LogEntity> logEntities = new List<LogEntity>();


        public TelemetryRepository(IConfiguration configuration, ILogger<TelemetryRepository> logger)
        {
            _configuration = configuration;
            _logger = logger;
            telemetry.InstrumentationKey = _configuration["APPINSIGHTS_INSTRUMENTATIONKEY"];
        }

        public void AddLogEntityToList(string module, string customLog, Severity severity)
        {
            logEntities.Add(new LogEntity
            {
                module = module,
                LogMessage = customLog,
                severity = (int)severity
            });

            switch ((int)severity)
            {
                case 0:
                case 1:
                case 2:
                    _logger.LogWarning(customLog);
                    break;
                case 3:
                    _logger.LogError(customLog);
                    break;
                case 4:
                    _logger.LogCritical(customLog);
                    break;
            }



        }

        public void UploadLogEntities()
        {

            if (this.logEntities.Count > 0)
            {
                telemetry.TrackTrace(JsonConvert.SerializeObject(this.logEntities));
            }

        }


        public void UploadMetric<T, Y>(string metricName, string dimensionName, T metricValue, Y dimensionValue)
        {

            Metric metric = telemetry.GetMetric(metricName, dimensionName);
            metric.TrackValue(metricValue.ToString(), dimensionValue.ToString());
        }


        public void FlushTelemetry()
        {
            this.logEntities.Clear();
            telemetry.Flush();
            System.Threading.Thread.Sleep(3000);
        }

        public void UploadMultiDimensionalMetric<T>(string metricNamespace, string metricName, Dictionary<string, string> metricDimensions, T metricValue)
        {

            List<string> dimensions = metricDimensions.Keys.ToList();
            List<string> dimesnionValues = metricDimensions.Values.ToList();

            MetricIdentifier identifierId = new MetricIdentifier(metricNamespace, metricName, dimensions);
            Metric metric = telemetry.GetMetric(identifierId);

            metric.TrackValue(metricValue.ToString(), dimesnionValues[0], dimesnionValues[1], dimesnionValues[2], dimesnionValues[3], dimesnionValues[4]);
        }
    }
}
