using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BrickBridge.Models;
using Amazon.Lambda.Core;
using BrickBridge.Lambda.MySql;
using PodioCore.Models;
using PodioCore.Utils.ItemFields;

[assembly: LambdaSerializer(typeof(Amazon.Lambda.Serialization.Json.JsonSerializer))]
namespace BrickBridge.Lambda
{
    public class DeleteItemFunction
    {
        public async System.Threading.Tasks.Task FunctionHandler(RoutedPodioEvent input, ILambdaContext context)
        {
            context.Logger.LogLine($"Entered function...");
            context.Logger.LogLine($"AppId: {input.appId}");
            context.Logger.LogLine($"ClientId: {input.clientId}");

            //var spaceId = input.currentItem.App.SpaceId.Value;
            //context.Logger.LogLine($"SpaceId: {spaceId}");

            //var deployment = input.currentEnvironment.apps.First(a => a.appId == input.appId);
            //context.Logger.LogLine($"Deployment: {deployment.date}");

            //var spaceName = deployment.deployedSpaces.First(kv => kv.Value == spaceId.ToString()).Key;
            //context.Logger.LogLine($"Space Name: {spaceName}");

            //var appName = input.currentItem.App.Name;

            using (var _mysql = new MySqlQueryHandler(context))
            {
                context.Logger.LogLine($"Deleting item {input.podioEvent.item_id}.");
                try
                {
                    await _mysql.DeletePodioItem(int.Parse(input.podioEvent.item_id));
                }
                catch (InvalidOperationException e)
                {
                    Console.WriteLine($"ItemId: {input.podioEvent.item_id}");
                    throw e;
                }
            }
        }
    }
}