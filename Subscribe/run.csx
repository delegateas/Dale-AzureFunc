using System;
using System.IO;
using System.Text;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Formatting;
using System.Configuration;
using JWT;

public static async Task<string> fetchAuthToken(string clientId, string clientSecret, string tenant)
{
    var url = "https://login.microsoftonline.com/" + tenant + "/oauth2/token";
    using (var http = new HttpClient())
    {
        var formContent = new FormUrlEncodedContent(new[]
        {
            new KeyValuePair<string, string>("grant_type", "client_credentials"),
            new KeyValuePair<string, string>("client_id", clientId),
            new KeyValuePair<string, string>("client_secret", clientSecret),
            new KeyValuePair<string, string>("resouce", "https://manage.office.com"),
        });

        var resp = await http.PostAsync(url, formContent);
        var body = await resp.Content.ReadAsStringAsync();
        var JsonSerializer = new DefaultJsonSerializer();
        var json = JsonSerializer.Deserialize<Dictionary<string, string>>(body);
        return json["access_token"];
    }
}

public static async Task subscribe(TraceWriter log, string webhook, string authId, string tenantId, string token, string contentType)
{
    var url = "https://manage.office.com/api/v1.0/" + tenantId + "/activity/feed/subscriptions/start";
    using (var http = new HttpClient())
    {
        http.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
        var reqbody = "{'webhook': {" +
                                      "'address': '" + webhook + "', " +
                                      "'authId': '" + authId + "', " +
                                      "'expiration': ''" +
                                   "}}";
        url += "?contentType=" + contentType;
        var req = new HttpRequestMessage(HttpMethod.Post, url);
        req.Content = new StringContent(reqbody, Encoding.UTF8, "application/json");
        var resp = await http.SendAsync(req);
        var body = await resp.Content.ReadAsStringAsync();

        log.Info("Subscribing to " + contentType + ": HTTP " + resp.StatusCode.ToString());
    }
}


public static async Task Run(string input, TraceWriter log)
{
    var tenant = Environment.GetEnvironmentVariable("APPSETTING_Tenant");
    var clientId = Environment.GetEnvironmentVariable("APPSETTING_ClientId");
    var clientSecret = Environment.GetEnvironmentVariable("APPSETTING_ClientSecret");
    var webHookUrl = "https://" + Environment.GetEnvironmentVariable("WEBSITE_HOSTNAME") + "/api/Notify";
    var authId = clientId;
    var jwt = await fetchAuthToken(clientId, clientSecret, tenant);
    var json = JsonWebToken.Decode(jwt, "", false);
    var JsonSerializer = new DefaultJsonSerializer();
    var token = JsonSerializer.Deserialize<Dictionary<string, string>>(json);
    var tenantId = token["tid"];


    subscribe(log, webHookUrl, authId, tenantId, jwt, "Audit.SharePoint");
    subscribe(log, webHookUrl, authId, tenantId, jwt, "Audit.Exchange");
    subscribe(log, webHookUrl, authId, tenantId, jwt, "Audit.AzureActiveDirectory");
    subscribe(log, webHookUrl, authId, tenantId, jwt, "Audit.General");
}