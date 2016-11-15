using Microsoft.FSharp;
using Microsoft.FSharp.Core;
using System.Net;
using System.Net.Http;
using Newtonsoft.Json;
using Dale;

public static async Task<HttpResponseMessage> Run(HttpRequestMessage req, TraceWriter log) {
   var handler = FSharpFunc<HttpRequestMessage, Control.FSharpAsync<HttpResponseMessage>>.ToConverter(Middleware.wrappedAuditHandler);
   log.Info("Received message:");
   log.Info(req.ToString());
   return await handler(req);
}
