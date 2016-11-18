using System;
using Microsoft.Azure.WebJobs.Host;
using Dale;

public static void Run(string message, TraceWriter log)
{
    log.Info("Processing batch at: {message}");
    Http.doExportWithException(message);
}