using System.Net;
using System.Net.Http;
using Newtonsoft.Json;
using Dale;

public static HttpResponseMessage Run(HttpRequestMessage req, TraceWriter log) {
   log.Info("Received message:");
   log.Info(req.Content.ReadAsStringAsync().Result);
   return Middleware.interopHandler(req);
}
