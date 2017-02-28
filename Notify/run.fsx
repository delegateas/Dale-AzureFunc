open System
open Microsoft.Azure.WebJobs.Host
open FSharp.Azure.Storage
open Dale

let Run (req: HttpRequestMessage, log: TraceWriter) =  
   log.Info("Received message:")
   log.Info(req.Content.ReadAsStringAsync().Result)
   log.Info("EOF")
   let conf =
     { Tenant = Environment.GetEnvironmentVariable("Tenant");
       ClientId = Environment.GetEnvironmentVariable("ClientId");
       ClientSecret = Environment.GetEnvironmentVariable("ClientSecret");
       AzureConnectionString = Environment.GetEnvironmentVariable("AzureConnectionString");
       AzureQueueName = Environment.GetEnvironmentVariable("AzureQueueName");
       RedactedFields = (Set.ofArray (Environment.GetEnvironmentVariable("RedactedFields").Split(',')));
       PartiallyRedactedFields = (Set.ofArray (Environment.GetEnvironmentVariable("PartiallyRedactedFields").Split(','))); }

   let dale = Dale.Exporter(conf)
   dale.Handle(req)