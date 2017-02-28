using System.Net;
using System.Configuration;
using System.Net.Http;
using Newtonsoft.Json;
using Dale;

public static HttpResponseMessage Run(HttpRequestMessage req, TraceWriter log) {
   log.Info("Received message:");
   log.Info(req.Content.ReadAsStringAsync().Result);
   log.Info("EOF");
   
   var conf = Dale.Config.Create(
       ConfigurationManager.AppSettings["Tenant"],
       ConfigurationManager.AppSettings["ClientId"],
       ConfigurationManager.AppSettings["ClientSecret"],
       ConfigurationManager.AppSettings["AzureConnectionString"],
       ConfigurationManager.AppSettings["AzureQueueName"],
       ConfigurationManager.AppSettings["RedactedFields"],
       ConfigurationManager.AppSettings["PartiallyRedactedFields"]);

   return new Dale.Exporter(conf).Handle(req);
}
