using System.Net;
using System.Net.Http;
using Newtonsoft.Json;
using Dale;

public static async Task<HttpResponseMessage> Run(HttpRequestMessage req, TraceWriter log) {
   log.Info("Received message:");
   log.Info(req.Content.ReadAsStringAsync().Result);
   return await Middleware.interopHandler(req);
}
