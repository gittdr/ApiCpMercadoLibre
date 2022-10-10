using Newtonsoft.Json.Linq;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApiCpMercadoLibre
{
    internal class Program
    {
        static void Main(string[] args)
        {
            //string id = "4017611389022000";
            //string secret = "ZK5Iuxv7CSovbXwWKzoKQ3rjtZtbFq0o";

            var client = new RestClient("https://api.mercadolibre.com/oauth/token");
            var request = new RestRequest(Method.POST);
            request.AddHeader("cache-control", "no-cache");
            
            request.AddHeader("Content-Type", "application/json");
            var body = @"{
            " + "\n" +
                        @"       ""client_id"": 4017611389022000,
            " + "\n" +
                        @"       ""client_secret"": ""ZK5Iuxv7CSovbXwWKzoKQ3rjtZtbFq0o"",
            " + "\n" +
                        @"       ""grant_type"": ""client_credentials""
            " + "\n" +
                        @"}";
            request.AddParameter("application/json", body, ParameterType.RequestBody);
            //request.AddParameter("application/json", body, ParameterType.RequestBody);
            //request.AddParameter("application/json", "grant_type=client_credentials&scope=all&client_id=" + id + "&client_secret=" + secret, ParameterType.RequestBody);
            RestResponse response = (RestResponse)client.Execute(request);

            dynamic resp = JObject.Parse(response.Content);
            string token = resp.access_token;
            string token_type = resp.token_type;
            string expires_in = resp.expires_in;
            string scope = resp.scope;
            string user_id = resp.user_id;

            client = new RestClient("https://xxx.xxx.com/services/api/x/users/v1/employees");
            request = new RestRequest(Method.GET);
            request.AddHeader("authorization", "Bearer " + token);
            request.AddHeader("cache-control", "no-cache");
            response = (RestResponse)client.Execute(request);
        }
    }
}
