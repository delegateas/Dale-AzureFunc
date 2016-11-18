using System;
using Microsoft.Azure.WebJobs.Host;
using FSharp.Azure.Storage;
using Dale;

public static void Run(string message, TraceWriter log)
{
    log.Info("Processing batch : " + message);
    var res = Interop.doExportWithException(message);
    foreach (string s in res)
    {
        log.Info(s);
    }
}