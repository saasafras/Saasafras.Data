using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BrickBridge.Models;
using Amazon.Lambda.Core;
using BrickBridge.Lambda.MySql;
using PodioCore.Applications;
using PodioCore.Spaces;

// Assembly attribute to enable the Lambda function's JSON input to be converted into a .NET class.
[assembly: LambdaSerializer(typeof(Amazon.Lambda.Serialization.Json.JsonSerializer))]

namespace BrickBridge.Lambda
{
    public class SaasafrasAppTableCreateRequest
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
            System.Console.WriteLine($"Entered function...");
            ILambdaSerializer serializer = new Amazon.Lambda.Serialization.Json.JsonSerializer();
            
            using (var _mysql = new MySqlQueryHandler(context))
            {
                context.Logger.LogLine($"Creating view and table, populating view for {input.solutionId}, {input.version}, {input.spaceName}, {input.appName}");
                await _mysql.CreatePodioAppView(input.solutionId, input.version, input.spaceName, input.appName)
                            .ContinueWith(async t => await _mysql.CreatePodioAppTable(input.solutionId, input.version, input.spaceName, input.appName))
                            .ContinueWith(async t => await _mysql.RebuildAppTable(input.solutionId, input.version, 'Y'));
            }
        }

        static void Main()
        {
            var request = new SaasafrasAppTableCreateRequest
            {
                solutionId = "mpactprobeta",
                version = "3.0",
                spaceName = "0. Agency Administration",
                appName = "Agency Profile"
            };
            var function = new SaasafrasAppTableCreateFunction();
            function.FunctionHandler(request, null).Wait();
        }
    }
}
