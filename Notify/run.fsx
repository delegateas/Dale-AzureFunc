open System.Net
open System.Net.Http
open FSharp.Interop.Dynamic
open Newtonsoft.Json
open Dale.Http

let Run(req: HttpRequestMessage, log: TraceWriter) :Task<HttpResponseMessage> =
  log.Info(sprintf "WebHook triggered.")
  return wrappedAuditHandler req |> Async.StartAsTask
