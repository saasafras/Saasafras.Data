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
        public string appId { get; set; }
        public string version { get; set; }
    }

    public class SaasafrasAppTableCreateFunction
    {
        /// <summary>
        /// A simple function that takes a string and does a ToUpper
        /// </summary>
        /// <param name="input"></param>
        /// <param name="context"></param>
        /// <returns></returns>
        public async Task FunctionHandler(RoutedPodioEvent input, ILambdaContext context)
        {
            context.Logger.LogLine($"Entered function...");
            ILambdaSerializer serializer = new Amazon.Lambda.Serialization.Json.JsonSerializer();

            using (var _mysql = new MySqlQueryHandler(context))
            {
                context.Logger.LogLine($"Creating view and table, populating view for {input.appId}, {input.version}, {space.Name.Split(" - ")[1]}, {app.Config.Name}");
                await _mysql.CreatePodioAppView(input.appId, input.version, space.Name.Split(" - ")[1], app.Config.Name)
                            .ContinueWith(async t => await _mysql.CreatePodioAppTable(input.appId, input.version, space.Name.Split(" - ")[1], app.Config.Name))
                            .ContinueWith(async t => await _mysql.RebuildAppTable(input.appId, input.version, 'Y'));
            }
        }
    }
}
