using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Saasafras.Model;
using Amazon.Lambda.Core;
using BrickBridge.Lambda.MySql;
// Assembly attribute to enable the Lambda function's JSON input to be converted into a .NET class.
[assembly: LambdaSerializer(typeof(Amazon.Lambda.Serialization.Json.JsonSerializer))]

namespace BrickBridge.Lambda
{
    public class SaasafrasAppTableRebuildRequest
    {
        public string solutionId { get; set; }
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
        public async Task FunctionHandler(SaasafrasAppTableRebuildRequest input, ILambdaContext context)
        {
            context.Logger.LogLine($"{Newtonsoft.Json.JsonConvert.SerializeObject(input)}");
            using (var _mysql = new MySqlQueryHandler(context))
            {				
				context.Logger.LogLine($"Rebuilding app tables for {input.solutionId}, {input.version}");
                try
                {
                    var task = _mysql.RebuildCoreTables(input.solutionId, input.version);
                    await task;
                }
                catch (Exception e)
                {
                    context.Logger.LogLine(e.Message);
                }
                task.ContinueWith((t) => _mysql.RebuildAppTable(input.solutionId, input.version, 'Y'));
                await task;
            }
        }
    }
}