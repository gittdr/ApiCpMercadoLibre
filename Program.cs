using ApiCpMercadoLibre.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
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
            GetDetails(token);
        }

        public void GetDetails(string token)
        {
            //2036231 / MXXQR1
            //2025473/middle-mile/facilities/MXXEM1
            //2036355/MXXEM1
            string Ai_orden = "123434";
            string uwe = "KGM";
            var client = new RestClient("https://api.mercadolibre.com/shipping/fiscal/MLM/routes/2036355/middle-mile/facilities/MXXEM1/details");
            //var client = new RestClient("https://api.mercadolibre.com/routes/2259547528693863/carta-porte-details");
            //var client = new RestClient("https://api.mercadolibre.com/classified_locations/countries/" + "UY");
            var request = new RestRequest(Method.GET);
            request.AddHeader("authorization", "Bearer " + token);
            request.AddHeader("cache-control", "no-cache");
            RestResponse response = (RestResponse)client.Execute(request);
            var data = Newtonsoft.Json.JsonConvert.DeserializeObject<MLMCartaPorte>(response.Content);
            string id = data.id;
            string cost = data.cost;
            string fullname = data.recipient.full_name;
            string rfc = data.recipient.rfc;
            
            string fiscal_fullname = data.origin.fiscal_information.full_name;
            string fiscal_rfc = data.origin.fiscal_information.rfc;
            string fiscal_residences = data.origin.fiscal_information.fiscal_residence;

            string addressline = data.origin.address.address_line;
            string street_name = data.origin.address.street_name;
            string street_number = data.origin.address.street_number;
            string intersection = data.origin.address.intersection;
            string zip_code = data.origin.address.zip_code;
            string city_id = data.origin.address.city.id;
            string city_name = data.origin.address.city.name;
            string state_id = data.origin.address.state.id;
            string state_name = data.origin.address.state.name;
            string country_id = data.origin.address.country.id;
            string country_name = data.origin.address.country.name;
            string neig_id = data.origin.address.neighborhood.id;
            string neig_name = data.origin.address.neighborhood.name;
            string muni_id = data.origin.address.municipality.id;
            string muni_name = data.origin.address.municipality.name;

            //DESTINATION
            string dfiscal_fullname = data.destination.fiscal_information.full_name;
            string dfiscal_rfc = data.destination.fiscal_information.rfc;
            string dfiscal_residences = data.destination.fiscal_information.fiscal_residence;
            string daddressline = data.destination.address.address_line;
            string dstreet_name = data.destination.address.street_name;
            string dstreet_number = data.destination.address.street_number;
            string dintersection = data.destination.address.intersection;
            string dzip_code = data.destination.address.zip_code;
            string dcity_id = data.destination.address.city.id;
            string dcity_name = data.destination.address.city.name;
            string dstate_id = data.destination.address.state.id;
            string dstate_name = data.destination.address.state.name;
            string dcountry_id = data.destination.address.country.id;
            string dcountry_name = data.destination.address.country.name;
            string dneig_id = data.destination.address.neighborhood.id;
            string dneig_name = data.destination.address.neighborhood.name;
            string dmuni_id = data.destination.address.municipality.id;
            string dmuni_name = data.destination.address.municipality.name;
            //END DESTINATION


            //SHIPMENTS
            dynamic info = data.shipments;
            foreach (var item in info)
            {

                string ship_id = item.id;
                string ship_url = item.url;
                string moreurl = "/items/details";
                string urls = "https://api.mercadolibre.com/shipping/fiscal/MLM/shipments/" + ship_id + moreurl;
                var client2 = new RestClient(urls);
                
                var request2 = new RestRequest(Method.GET);
                request2.AddHeader("authorization", "Bearer " + token);
                request2.AddHeader("cache-control", "no-cache");
                RestResponse response2 = (RestResponse)client2.Execute(request2);
                string respuesta = response2.Content;
                var dataz = Newtonsoft.Json.JsonConvert.DeserializeObject<MLMCartaPorte>(respuesta);
                if (dataz.status == 0)
                {
                    dynamic elementos = dataz.package.items;
                    foreach (var ccitem in elementos)
                    {
                        string cate = ccitem.category;
                        if (cate == "1010101")
                        {
                            cate = "01010101";
                        }
                        string descript = ccitem.description;
                        string unitcode = ccitem.unit_code;
                        string quanti = ccitem.quantity;
                        int heig = ccitem.dimensions.height;
                        int widht = ccitem.dimensions.width;
                        int length = ccitem.dimensions.length;
                        int weight = ccitem.dimensions.weight;

                        //Aqui va el sp para insertar las mercancias
                        InsertMerc(Ai_orden, id, cate, descript, weight, uwe, quanti, unitcode);

                    }
                    int total_items = dataz.package.total_items;
                }
                else
                {
                    InsertMercErrores(ship_id);
                }
                

            }
            //END SHIPMENTS
        }
        public void InsertMerc(string Ai_orden, string id, string cate, string descript, int weight, string uwe, string quanti, string unitcode)
        {
            string cadena = @"Data source=172.24.16.112; Initial Catalog=TMWSuite; User ID=sa; Password=tdr9312;Trusted_Connection=false;MultipleActiveResultSets=true";
            //DataTable dataTable = new DataTable();

            using (SqlConnection connection = new SqlConnection(cadena))
            {

                using (SqlCommand selectCommand = new SqlCommand("sp_Insert_Api_MercadoL_JC", connection))
                {

                    selectCommand.CommandType = CommandType.StoredProcedure;
                    selectCommand.CommandTimeout = 1000;
                    selectCommand.Parameters.AddWithValue("@Ai_orden", Ai_orden);
                    selectCommand.Parameters.AddWithValue("@id", id);
                    selectCommand.Parameters.AddWithValue("@cate", cate);
                    selectCommand.Parameters.AddWithValue("@descript", descript);
                    selectCommand.Parameters.AddWithValue("@weight", weight);
                    selectCommand.Parameters.AddWithValue("@uwe", uwe);
                    selectCommand.Parameters.AddWithValue("@quanti", quanti);
                    selectCommand.Parameters.AddWithValue("@unitcode", unitcode);

                    try
                    {
                        connection.Open();
                        selectCommand.ExecuteNonQuery();
                    }
                    catch (Exception ex)
                    {
                        string message = ex.Message;
                    }
                    finally
                    {
                        connection.Close();
                    }
                }
            }

        }
        public void InsertMercErrores(string ship_id)
        {
            string cadena = @"Data source=172.24.16.112; Initial Catalog=TMWSuite; User ID=sa; Password=tdr9312;Trusted_Connection=false;MultipleActiveResultSets=true";
            //DataTable dataTable = new DataTable();

            using (SqlConnection connection = new SqlConnection(cadena))
            {

                using (SqlCommand selectCommand = new SqlCommand("sp_Insert_Api_MercadoL_Errores_JC", connection))
                {

                    selectCommand.CommandType = CommandType.StoredProcedure;
                    selectCommand.CommandTimeout = 1000;
                    selectCommand.Parameters.AddWithValue("@ship_id", ship_id);
                    try
                    {
                        connection.Open();
                        selectCommand.ExecuteNonQuery();
                    }
                    catch (Exception ex)
                    {
                        string message = ex.Message;
                    }
                    finally
                    {
                        connection.Close();
                    }
                }
            }

        }
    }
}
