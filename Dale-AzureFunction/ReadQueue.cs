using DG.AuditLogExporter.Functions.Constants;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;
using System;

namespace DG.AuditLogExporter.Functions
{
    public static class ReadQueue
    {
        [FunctionName("ReadQueue")]
        public static void Run([QueueTrigger("dale-auditeventqueue", Connection = EnvKeys.AzureConnectionString)]string message, TraceWriter log)
        {
            log.Info($"Processing batch : {message}");
            var conf = Dale.Config.Create(
                Environment.GetEnvironmentVariable(EnvKeys.Tenant),
                Environment.GetEnvironmentVariable(EnvKeys.ClientId),
                Environment.GetEnvironmentVariable(EnvKeys.ClientSecret),
                Environment.GetEnvironmentVariable(EnvKeys.AzureConnectionString),
                Environment.GetEnvironmentVariable(EnvKeys.AzureQueueName),
                Environment.GetEnvironmentVariable(EnvKeys.RedactedFields),
                Environment.GetEnvironmentVariable(EnvKeys.PartiallyRedactedFields));
            var res = new Dale.Exporter(conf).ExportWithException(message);
            foreach (string s in res)
            {
                log.Info(s);
            }
        }
    }
}
