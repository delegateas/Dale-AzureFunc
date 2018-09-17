using System;
using System.Net.Http;
using DG.AuditLogExporter.Functions.Constants;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Azure.WebJobs.Host;

namespace DG.AuditLogExporter.Functions
{
    public static class Notify
    {
        [FunctionName("Notify")]
        public static HttpResponseMessage Run([HttpTrigger(AuthorizationLevel.Function, "get", "post", Route = null)]HttpRequestMessage req, TraceWriter log)
        {
            log.Info("Received message:");
            log.Info(req.Content.ReadAsStringAsync().Result);
            log.Info("EOF");

            var conf = Dale.Config.Create(
                Environment.GetEnvironmentVariable(EnvKeys.Tenant),
                Environment.GetEnvironmentVariable(EnvKeys.ClientId),
                Environment.GetEnvironmentVariable(EnvKeys.ClientSecret),
                Environment.GetEnvironmentVariable(EnvKeys.AzureConnectionString),
                Environment.GetEnvironmentVariable(EnvKeys.AzureQueueName),
                Environment.GetEnvironmentVariable(EnvKeys.RedactedFields),
                Environment.GetEnvironmentVariable(EnvKeys.PartiallyRedactedFields));
            return new Dale.Exporter(conf).Handle(req);
        }
    }
}
