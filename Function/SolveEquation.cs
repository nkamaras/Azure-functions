using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using AzureLoggingFunction.Services.TelemetryRepository;
using AzureLoggingFunction.Helpers;
using Microsoft.ApplicationInsights.Channel;
using Microsoft.ApplicationInsights.DataContracts;
using System.Collections.Generic;
//using static AzureLoggingFunction.Helpers.EnumHelper;


namespace AzureLoggingFunction.Function
{
    public class SolveEquation
    {

        private readonly ITelemetryRepository _telemetryService;

        public SolveEquation(ITelemetryRepository telemetryService) { 
            _telemetryService = telemetryService;
        }

        [FunctionName("SolveEquation")]
        public  async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = null)] HttpRequest req,
        ILogger log)
        {

            DateTime startTime = DateTime.Now;

            Exception Error = null;
            string responseMessage = String.Empty;
            double x1 = 0;
            double x2 = 0;
            int equationGrade = 2;

            try {

                log.LogWarning("HTTP trigger function processed a request.");
                _telemetryService.AddLogEntityToList("SolveEquation", "Start solving a quadratic equation", EnumHelper.Severity.Information);

                int a = 2;
                int b = -8;
                int c = 6;

                double d = Math.Pow(b,2)-4*a*c;

                if (d > 0)
                { 
                    x1 = (-1*b + Math.Pow(d,0.5))/(2*a);
                    x2 = (-1*b - Math.Pow(d,0.5))/(2*a);
                    responseMessage = String.Format("The two solutions of the quadratic equation are {0} and {1}", x1, x2);
                }
                else if (d == 0)
                {
                    x1 = -b / (2 * a);
                    responseMessage = String.Format("Solution of the quadratic equation is {0}", x1);
                }
                else {
                    responseMessage = "Equation is not feasible in R";
                }
            }
            catch (Exception ex)
            {
                Error = ex;
                responseMessage = String.Format("Exception occurred: {0} - {1}", ex.Message, ex.StackTrace);

                _telemetryService.AddLogEntityToList("LoggingFunction", responseMessage, EnumHelper.Severity.Error);
            }
            finally {
                _telemetryService.UploadLogEntities();
                _telemetryService.FlushTelemetry();
            }

            _telemetryService.UploadMetric("Equation execution Time", "Total Execution Time", MetricsHelper.GetExecutionTime(startTime, DateTime.Now), equationGrade);

            Dictionary<string, string> dimensions = new Dictionary<string, string>()
            {
                { "Equation grade" , "2nd" },
                { "D" , ">0" }
            }; 

            //_telemetryService.UploadMultiDimensionalMetric("Execution","Execution Time", dimensions, MetricsHelper.GetExecutionTime(startTime, DateTime.Now));

            return new OkObjectResult(responseMessage);
        }
    }
}
