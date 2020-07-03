using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Amazon.Lambda.Core;
using BrickBridge.Lambda.MySql;
using PodioCore.Applications;
using PodioCore.Spaces;

// Assembly attribute to enable the Lambda function's JSON input to be converted into a .NET class.
[assembly: LambdaSerializer(typeof(Amazon.Lambda.Serialization.Json.JsonSerializer))]

namespace BrickBridge.Lambda
{
    public class SaasafrasAppTableCreateRequest //: SaasafrasSolutionCommand
    {
        public string solutionId { get; set; }
        public string version { get; set; }
        public string spaceName { get; set; }
        public string appName { get; set; }
    }

    public class SaasafrasAppTableCreateFunction
    {
        /// <summary>
        /// A simple function that takes a string and does a ToUpper
        /// </summary>
        /// <param name="input"></param>
        /// <param name="context"></param>
        /// <returns></returns>
        public async Task FunctionHandler(SaasafrasAppTableCreateRequest input, ILambdaContext context)
        {
            var apiKey = System.Environment.GetEnvironmentVariable("API_KEY");
            var baseUrl = System.Environment.GetEnvironmentVariable("BASE_URL");
            var connectionString = System.Environment.GetEnvironmentVariable("PODIO_DB_CONNECTION_STRING");

            var client = new BbcServiceClient(baseUrl, apiKey);
            SaasafrasTokenProvider token = new SaasafrasTokenProvider(input.solutionId, input.version, client);
            //var podio = new PodioCore.Podio(token);
            
            System.Console.WriteLine($"Entered function...");
            ILambdaSerializer serializer = new Amazon.Lambda.Serialization.Json.JsonSerializer();
            
            using (var _mysql = new MySqlQueryHandler(connectionString ?? "server=mpactprodata.czsyc7qltifw.us-east-2.rds.amazonaws.com;uid=admin;pwd=perilousDeity;database=podioTest"))
            {
                if (input.spaceName != "Contacts")
                {
                    await _mysql.CreatePodioAppView(input.solutionId, input.version, input.spaceName, input.appName);
                    await _mysql.CreatePodioAppTable(input.solutionId, input.version, input.spaceName, input.appName);
                }
                else
                {
                    await _mysql.CreatePodioContactsTable();
                    var http = new System.Net.Http.HttpClient();
                    http.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("OAuth2", token.TokenData.AccessToken);
                    http.BaseAddress = new Uri("https://api.podio.com/");
                    int limit = 50;
                    int offset = 0;
                    int count = 0;
                    do
                    {
                        var content = await http.GetStringAsync($"contact?offset={offset}&limit={limit}");
                        var contacts = Newtonsoft.Json.JsonConvert.DeserializeObject<List<PodioContact>>(content);
                        foreach(var contact in contacts)
                        {
                            await _mysql.AddContact(
                                contact.ProfileId,
                                contact.UserId,
                                contact.Name,
                                (contact.Email?.Length ?? 0) > 0 ? contact.Email[0] : "",
                                (contact.Address?.Length ?? 0) > 0 ? contact.Address[0] : "",
                                contact.City,
                                contact.State,
                                contact.Zip,
                                contact.Type,
                                (contact.Phone?.Length ?? 0) > 0 ? contact.Phone[0] : ""
                                );
                        }
                        count = contacts.Count;
                        offset += limit;
                    }
                    while (count >= limit);
                }
            }

        }

        static void Main()
        {
            var request = new SaasafrasAppTableCreateRequest
            {
                solutionId = "mpactprobeta",
                version = "3.0",
                spaceName = "Contacts"
            };
            var function = new SaasafrasAppTableCreateFunction();
            function.FunctionHandler(request, null).Wait();
        }
    }
    public class PodioContact
    {
        [Newtonsoft.Json.JsonProperty(PropertyName = "profile_id")]
        public int ProfileId { get; set; }
        [Newtonsoft.Json.JsonProperty(PropertyName = "user_id")]
        public int UserId { get; set; }
        [Newtonsoft.Json.JsonProperty(PropertyName = "phone")]
        public string[] Phone { get; set; }
        [Newtonsoft.Json.JsonProperty(PropertyName = "address")]
        public string[] Address { get; set; }
        [Newtonsoft.Json.JsonProperty(PropertyName = "city")]
        public string City { get; set; }
        [Newtonsoft.Json.JsonProperty(PropertyName = "name")]
        public string Name { get; set; }
        [Newtonsoft.Json.JsonProperty(PropertyName = "zip")]
        public string Zip { get; set; }
        [Newtonsoft.Json.JsonProperty(PropertyName = "state")]
        public string State { get; set; }
        [Newtonsoft.Json.JsonProperty(PropertyName = "mail")]
        public string[] Email { get; set; }
        [Newtonsoft.Json.JsonProperty(PropertyName = "type")]
        public string Type { get; set; }
    }
}
