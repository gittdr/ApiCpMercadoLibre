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
            Program muobject = new Program();
            muobject.GetToken();

        }

        public void GetToken()
        {
            //POST
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
            RestResponse response = (RestResponse)client.Execute(request);

            dynamic resp = JObject.Parse(response.Content);
            string token = resp.access_token;
            string token_type = resp.token_type;
            string expires_in = resp.expires_in;
            string scope = resp.scope;
            string user_id = resp.user_id;
            GetCountries(token);
        }

        public void GetCountries(string token)
        {
            //var client = new RestClient("https://api.mercadolibre.com/routes/2259547528693863/carta-porte-details");
            var client = new RestClient("https://api.mercadolibre.com/classified_locations/countries/" + "UY");
            var request = new RestRequest(Method.GET);
            request.AddHeader("authorization", "Bearer " + token);
            request.AddHeader("cache-control", "no-cache");
            RestResponse response = (RestResponse)client.Execute(request);
            dynamic resp = JObject.Parse(response.Content);
            string id = resp.id;
            string name = resp.name;
            dynamic geo_array = resp.geo_information;
            dynamic locations = geo_array.location;
            string latitud = locations.latitude;
            //string 

        }
    }
}
