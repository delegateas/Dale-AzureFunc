using System;
using System.Configuration;
using Microsoft.Azure.WebJobs.Host;
using FSharp.Azure.Storage;
using Dale;

public static void Run(string message, TraceWriter log)
{
    log.Info("Processing batch : " + message);
    var conf = Dale.Config.Create(
       ConfigurationManager.AppSettings["Tenant"],
       ConfigurationManager.AppSettings["ClientId"],
       ConfigurationManager.AppSettings["ClientSecret"],
       ConfigurationManager.AppSettings["AzureConnectionString"],
       ConfigurationManager.AppSettings["AzureQueueName"],
       ConfigurationManager.AppSettings["RedactedFields"],
       ConfigurationManager.AppSettings["PartiallyRedactedFields"]);

    var res = new Dale.Exporter(conf).ExportWithException(message);
    foreach(string s in res) {
        log.Info(s);
    }
}