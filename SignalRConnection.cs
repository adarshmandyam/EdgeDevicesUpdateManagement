using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Microsoft.Azure.WebJobs.Extensions.SignalRService;

namespace EdgeHubTriggerFunctions
{
    public static class SignalRConnection
    {
        [FunctionName("SignalRConnection")]
        public static SignalRConnectionInfo Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get")] HttpRequest req,
            [SignalRConnectionInfo(HubName = "broadcast")] SignalRConnectionInfo connectionInfo,
            ILogger log)
        {
            log.LogInformation("C# HTTP trigger function processed a request.");

            return connectionInfo;
        }
    }
}
