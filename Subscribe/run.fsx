open System
open System.Configuration
open FSharp.Data
open FSharp.Data.HttpRequestHeaders
open JWT

let fetchAuthToken clientId clientSecret tenant =
  let url = "https://login.microsoftonline.com/" + tenant + "/oauth2/token"
  let json =
    Http.RequestString(url, 
                       headers=[Accept "application/json"],
                       body=FormValues ["grant_type", "client_credentials";
                                         "client_id", clientId;
                                         "client_secret", clientSecret;
                                         "resource", "https://manage.office.com"])

  let res = JsonValue.Parse json
  let token = res.GetProperty("access_token").AsString()
  token


let subscribe (log :TraceWriter) webhook authId tenantId token contentType =
  let url = "https://manage.office.com/api/v1.0/" + tenantId +
            "/activity/feed/subscriptions/start"
  let reqbody =
   "{'webhook': {"  +
      "'address': '" + webhook + "', " +
      "'authId': '" + authId + "', " +
      "'expiration': ''" +
      "}}"

  let resp =
    Http.Request(url,
                 headers = [Accept "application/json"; 
                            Authorization token;
                            ContentType HttpContentTypes.Json],
                 query = ["contentType", contentType],
                 body = TextRequest reqbody)
  log.Info(sprintf "Subscribing to %s: HTTP %i" contentType resp.StatusCode)


let Run(input: string, log: TraceWriter) =
  let tenant = Environment.GetEnvironmentVariable("APPSETTING_Tenant")
  let clientId = Environment.GetEnvironmentVariable("APPSETTING_ClientId")
  let clientSecret =  Environment.GetEnvironmentVariable("APPSETTING_ClientSecret")
  let webHookUrl = "https://" + Environment.GetEnvironmentVariable("WEBSITE_HOSTNAME") + "/api/Notify"
  
  let authId = clientId
  let jwt = fetchAuthToken clientId clientSecret tenant
  let token = "Bearer " + jwt
  let tenantId = JsonValue.Parse(JsonWebToken.Decode(jwt, "", false)).["tid"].AsString()
  
  let subscribe' = subscribe log webHookUrl authId tenantId token
  subscribe' "Audit.SharePoint"
  subscribe' "Audit.Exchange"
  subscribe' "Audit.AzureActiveDirectory"
  subscribe' "Audit.General"