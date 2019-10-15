using System;
using System.Threading.Tasks;
using Amazon.Lambda.Core;
using Amazon.RDSDataService;
using Amazon.RDSDataService.Model;
[assembly: LambdaSerializer(typeof(Amazon.Lambda.Serialization.Json.JsonSerializer))]
namespace Saasafras.Data.CreateSolutionDb
{
    class Program
    {
        public async Task<String> Main(string[] args, ILambdaContext lambdaContext)
        {
            var client = new AmazonRDSDataServiceClient();
            var executeStatementRequest = new ExecuteStatementRequest();
            executeStatementRequest.Database = "saasafrasPodio2";
            executeStatementRequest.ResourceArn = "arn:aws:rds:us-east-2:586320648585:cluster:saasafras-dev";
            executeStatementRequest.SecretArn = "arn:aws:secretsmanager:us-east-2:586320648585:secret:rds-db-credentials/cluster-UZENGP2SBVI6CVPZ62D3HGJSDE/saasafras-rb0sPO";
            executeStatementRequest.Sql = "select * from information_schema.tables;";
            executeStatementRequest.IncludeResultMetadata = true;

            var response = await client.ExecuteStatementAsync(executeStatementRequest);
            Console.WriteLine("response: " + Newtonsoft.Json.JsonConvert.SerializeObject(response));
            foreach(var record in response.Records)
            {
                Console.WriteLine("reading next record.");
                foreach(var field in record)
                {
                    Console.WriteLine("reading next field.");

                    Console.WriteLine($"field: {field}");
                    Console.WriteLine($"type: {field.GetType().FullName}");
                }
            }
            return $"response: {response.Records.Count}";
        }
    }
}
    