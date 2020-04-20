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
    public class SaasafrasRebuildRequest
    {
        public string command { get; set; }
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
        public async Task FunctionHandler(SaasafrasRebuildRequest input, ILambdaContext context)
        {
            context.Logger.LogLine($"{Newtonsoft.Json.JsonConvert.SerializeObject(input)}");
            using (var _mysql = new MySqlQueryHandler(context))
            {
                switch(input.command)
                {
                    case "rebuild-core-tables":
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
                        break;
                    case "rebuild-app-tables":
                        context.Logger.LogLine($"Rebuilding app tables for {input.solutionId}, {input.version}");
                        try
                        {
                            var task = _mysql.RebuildAppTable(input.solutionId, input.version, 'Y');
                            await task;
                        }
                        catch (Exception e)
                        {
                            context.Logger.LogLine(e.Message);
                        }
                        break;
                    default:
                        throw new Exception($"command {input.command} not recognized.");
                }
            }
        }
    }
}