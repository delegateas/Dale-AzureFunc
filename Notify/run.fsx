open System.Net
open System.Net.Http
open Newtonsoft.Json
open Dale.Middleware

let Run(req: HttpRequestMessage, log: TraceWriter) :Task<HttpResponseMessage> =
  log.Info(sprintf "WebHook triggered.")
  wrappedAuditHandler req |> Async.StartAsTask
