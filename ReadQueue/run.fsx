open System
open FSharp.Azure.Storage
open Dale

let Run (message: string, log: TraceWriter) =  
    log.Info("Processing batch : " + message)
    let conf =
     { Tenant = Environment.GetEnvironmentVariable("Tenant");
       ClientId = Environment.GetEnvironmentVariable("ClientId");
       ClientSecret = Environment.GetEnvironmentVariable("ClientSecret");
       AzureConnectionString = Environment.GetEnvironmentVariable("AzureConnectionString");
       AzureQueueName = Environment.GetEnvironmentVariable("AzureQueueName");
       RedactedFields = (Set.ofArray (Environment.GetEnvironmentVariable("RedactedFields").Split(',')));
       PartiallyRedactedFields = (Set.ofArray (Environment.GetEnvironmentVariable("PartiallyRedactedFields").Split(','))); }

    let dale = Dale.Exporter(conf)

    let res = dale.ExportWithException(message)
    res |> Seq.map(fun x -> log.Info(x))