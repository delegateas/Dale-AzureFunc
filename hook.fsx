open System.Net
open System.Net.Http
open FSharp.Interop.Dynamic
open Newtonsoft.Json
open Dale.Http

let Notify(req: HttpRequestMessage, log: TraceWriter) =
  log.Info(sprintf "WebHook triggered.")
  return wrappedAuditHandler req
