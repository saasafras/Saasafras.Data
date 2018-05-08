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
    public class AddItemFunction
    {
        public async System.Threading.Tasks.Task FunctionHandler(RoutedPodioEvent input, ILambdaContext context)
        {
            context.Logger.LogLine($"Entered function...");
            context.Logger.LogLine($"AppId: {input.appId}");
            context.Logger.LogLine($"ClientId: {input.clientId}");
            ILambdaSerializer serializer = new Amazon.Lambda.Serialization.Json.JsonSerializer();
            BrickBridge.Models.Podio.Item _item;
            using(var stream = new System.IO.MemoryStream())
            {
                serializer.Serialize<Item>(input.currentItem, stream);
                var inputText = System.Text.Encoding.UTF8.GetString(stream.ToArray());
                context.Logger.LogLine($"Input: {inputText}");
                stream.Position = 0;
                _item = serializer.Deserialize<Models.Podio.Item>(stream);
            }

            context.Logger.LogLine($"SpaceId: {_item.app.space_id}");
            var deployment = input.currentEnvironment.apps.First(a => a.appId == input.appId);
            var spaceName = deployment.deployedSpaces.First(kv => kv.Value == _item.app.space_id.ToString()).Key;
             

            var appName = input.currentItem.App.Name;

            using (var _mysql = new MySqlQueryHandler(context))
            {
                context.Logger.LogLine($"Inserting item {input.currentItem.ItemId} from app {input.appId}");
                var podioAppId = await _mysql.GetPodioAppId(input.appId, input.version, spaceName, appName);
                var podioItemId = await _mysql.InsertPodioItem(podioAppId, input.currentItem.ItemId, int.Parse(input.podioEvent.item_revision_id), input.clientId, input.currentEnvironment.environmentId);
                var appFields = await _mysql.SelectAppFields(podioAppId);
                context.Logger.LogLine($"Item has {input.currentItem.Fields.Count} fields to insert.");
                foreach (var field in input.currentItem.Fields)
                {
                    //if the field matches a field defined for this app, insert the field data
                    var appField = appFields.First(a => a.ExternalId == field.ExternalId && a.Type == field.Type);
                    context.Logger.LogLine($"Inserting field {field.ExternalId} for item {input.currentItem.ItemId}");
                    switch (field.Type)
                    {
                        case "category":
                            var c = input.currentItem.Field<CategoryItemField>(field.ExternalId);
                            foreach (var option in c.Options)
                            {
                                var i = await _mysql.InsertCategoryField(podioItemId, appField.PodioFieldId, option.Text, option.Id.Value);
                            }
                            break;
                        case "contact":
                            var co = input.currentItem.Field<ContactItemField>(field.ExternalId);
                            foreach (var contact in co.Contacts)
                            {
                                var i = await _mysql.InsertContactField(podioItemId, appField.PodioFieldId, contact.ProfileId);
                            }
                            break;
                        case "date":
                            var d = input.currentItem.Field<DateItemField>(field.ExternalId);
                            if (d.Start.HasValue)
                                await _mysql.InsertDateField(podioItemId, appField.PodioFieldId, d.Start, d.End);
                            break;
                        case "duration":
                            var du = input.currentItem.Field<DurationItemField>(field.ExternalId);
                            if (du.Value.HasValue)
                                await _mysql.InsertDurationField(podioItemId, appField.PodioFieldId, du.Value.Value.Seconds);
                            break;
                        case "location":
                            var l = input.currentItem.Field<LocationItemField>(field.ExternalId);
                            foreach (var location in l.Locations)
                            {
                                var i = await _mysql.InsertLocationField(podioItemId, appField.PodioFieldId, location);
                            }
                            break;
                        case "member":
                            var me = input.currentItem.Field<ContactItemField>(field.ExternalId);
                            foreach (var member in me.Contacts)
                            {
                                var i = await _mysql.InsertMemberField(podioItemId, appField.PodioFieldId, member.ProfileId);
                            }
                            break;
                        case "money":
                            var m = input.currentItem.Field<MoneyItemField>(field.ExternalId);
                            if (m.Value.HasValue)
                                await _mysql.InsertMoneyField(podioItemId, appField.PodioFieldId, m.Value.Value, m.Currency);
                            break;
                        case "number":
                            var n = input.currentItem.Field<NumericItemField>(field.ExternalId);
                            if (n.Value.HasValue)
                                await _mysql.InsertNumberField(podioItemId, appField.PodioFieldId, n.Value.Value);
                            break;
                        case "phone":
                            var p = input.currentItem.Field<PhoneItemField>(field.ExternalId);
                            foreach (var phone in p.Value)
                            {
                                var i = await _mysql.InsertPhoneField(podioItemId, appField.PodioFieldId, phone.Type, phone.Value);
                            }
                            break;
                        case "email":
                            var e = input.currentItem.Field<EmailItemField>(field.ExternalId);
                            foreach (var email in e.Value)
                            {
                                var i = await _mysql.InsertEmailField(podioItemId, appField.PodioFieldId, email.Type, email.Value);
                            }
                            break;
                        case "progress":
                            var pr = input.currentItem.Field<ProgressItemField>(field.ExternalId);
                            if (pr.Value.HasValue)
                                await _mysql.InsertProgressField(podioItemId, appField.PodioFieldId, pr.Value.Value);
                            break;
                        case "app":
                            var a = input.currentItem.Field<AppItemField>(field.ExternalId);
                            foreach (var item in a.Items)
                            {
                                var i = await _mysql.InsertRelationField(podioItemId, appField.PodioFieldId, item.ItemId);
                            }
                            break;
                        case "text":
                            var t = input.currentItem.Field<TextItemField>(field.ExternalId);
                            await _mysql.InsertTextField(podioItemId, appField.PodioFieldId, t.Value);
                            break;
                        case "calculation":
                            //var ca = input.currentItem.Field<CalculationItemField>(field.ExternalId);
                            //if (ca.HasValue() && ca.Value.HasValue)
                            //    await _mysql.InsertNumberField(podioItemId, appField.PodioFieldId, (double)ca.Value.Value);
                            //else
                                //await _mysql.InsertTextField(podioItemId, appField.PodioFieldId, ca.ValueAsString);
                            break;
                        default: throw new Exception($"Cannot handle field type: {field.Type}");
                    }
                }
            }
        }
    }
}
