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
    public class SaasafrasAppTableRebuildRequest
    {
        public string appId { get; set; }
        public string version { get; set; }
    }

    public class SaasafrasAppTableRebuildFunction
    {
        
        /// <summary>
        /// A simple function that takes a string and does a ToUpper
        /// </summary>
        /// <param name="input"></param>
        /// <param name="context"></param>
        /// <returns></returns>
        public async Task FunctionHandler(WebApiRequest<SaasafrasAppTableRebuildRequest> input, ILambdaContext context)
        {
			context.Logger.LogLine($"Entered function...");
            using (var _mysql = new MySqlQueryHandler(context))
            {				
				context.Logger.LogLine($"Rebuilding app tables for {input.bodyJson.appId}, {input.bodyJson.version}");

                await _mysql.RebuildAppTable(input.bodyJson.appId, input.bodyJson.version, 'Y');
            }
        }
    }
}