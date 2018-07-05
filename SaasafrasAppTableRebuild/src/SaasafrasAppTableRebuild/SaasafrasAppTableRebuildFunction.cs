using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BrickBridge.Models;
using Amazon.Lambda.Core;
using BrickBridge.Lambda.MySql;
// Assembly attribute to enable the Lambda function's JSON input to be converted into a .NET class.
[assembly: LambdaSerializer(typeof(Amazon.Lambda.Serialization.Json.JsonSerializer))]

namespace BrickBridge.Lambda
{
    public class SaasafrasAppTableRebuildFunction
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
            context.Logger.LogLine($"AppId: {input.appId}");
            context.Logger.LogLine($"ClientId: {input.clientId}");
            ILambdaSerializer serializer = new Amazon.Lambda.Serialization.Json.JsonSerializer();

            var deployment = input.currentEnvironment.apps.First(a => a.appId == input.appId);
            context.Logger.LogLine($"Deployment: {deployment.date}");
            
            using (var _mysql = new MySqlQueryHandler(context))
            {
				context.Logger.LogLine($"Calling rebuild app tables procedure on {input.appId}, {input.version}");

				await _mysql.RebuildAppTable(input.appId, input.version, 'Y');

				context.Logger.LogLine($"Submitted request to rebuild tables for {input.appId}, {input.version}");
            }
        }
    }
}
