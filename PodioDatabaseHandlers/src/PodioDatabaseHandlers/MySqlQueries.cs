using System;
namespace BrickBridge.Lambda.MySql
{
    public class MySqlQueries
    {        
        /// <summary>
        /// ?appName,?spaceId
        /// </summary>
        public const string INSERT_PODIO_APP = @"INSERT INTO PodioApp(PodioAppName,PodioSpaceId) VALUES(?appName,?spaceId);";
        /// <summary>
        /// ?bbcAppId,?version,?spaceName,?appName
        /// </summary>
        public const string SELECT_APP_ID = @"SELECT app.Id FROM PodioApp app 
                                            INNER JOIN PodioSpace space ON app.PodioSpaceId = space.Id
                                            INNER JOIN BbcApp b ON b.Id = space.BbcAppId
                                            WHERE b.BbcAppId = ?bbcAppId
                                            AND b.Version = ?version
                                            AND space.PodioSpaceName = ?spaceName
                                            AND app.PodioAppName = ?appName;";
        /// <summary>
        /// ?podioAppId
        /// </summary>
        public const string SELECT_APP_FIELDS = @"SELECT v.PodioFieldId, v.ExternalId, v.`Type` FROM podioTest.PodioAppView v WHERE PodioAppId = ?podioAppId;";
        /// <summary>
        /// ?podioAppId,?itemId,?revision,?clientId,?envId
        /// </summary>
        public const string INSERT_ITEM = @"INSERT INTO PodioItem(PodioAppId,ItemId,Revision,ClientId,EnvId) VALUES(?podioAppId,?itemId,?revision,?clientId,?envId);";
        /// <summary>
        /// ?spaceName,?bbcAppId
        /// </summary>
        public const string SELECT_SPACE_ID = @"SELECT Id FROM PodioSpace WHERE PodioSpaceName = ?spaceName AND BbcAppId = ?bbcAppId;";
        /// <summary>
        /// ?bbcAppId,?version
        /// </summary>
        public const string SELECT_BBCAPP_ID = @"SELECT Id FROM BbcApp WHERE BbcAppId = ?bbcAppId AND Version = ?version;";

        /// <summary>
        /// returns new id
        /// </summary>
        public const string GET_ID = @"SELECT LAST_INSERT_ID();";
        /// <summary>
        /// ?fieldId,?optionId,?optionText,?itemId
        /// </summary>
        public const string INSERT_CATEGORY_DATA = @"INSERT INTO podioTest.CategoryFieldData(PodioFieldId,CategoryOptionId,CategoryOptionText,PodioItemId) VALUES(?fieldId,?optionId,?optionText,?itemId);";
        /// <summary>
        /// ?fieldId,?contactId,?itemId
        /// </summary>
        public const string INSERT_CONTACT_DATA = @"INSERT INTO podioTest.ContactFieldData(PodioFieldId,PodioContactId,PodioItemId) VALUES(?fieldId,?contactId,?itemId);";
        /// <summary>
        /// ?fieldId,?start,?end,?itemId
        /// </summary>
        public const string INSERT_DATE_DATA = @"INSERT INTO podioTest.DateFieldData(PodioFieldId,StartDate,EndDate,PodioItemId) VALUES(?fieldId,?start,?end,?itemId);";
        /// <summary>
        /// ?fieldId,?duration,?itemId
        /// </summary>
        public const string INSERT_DURATION_DATA = @"INSERT INTO podioTest.DurationFieldData(PodioFieldId,SecondsDuration,PodioItemId) VALUES(?fieldId,?duration,?itemId);";
        /// <summary>
        /// ?fieldId,?location,?itemId
        /// </summary>
        public const string INSERT_LOCATION_DATA = @"INSERT INTO podioTest.LocationFieldData(PodioFieldId,Location,PodioItemId) VALUES(?fieldId,?location,?itemId);";
        /// <summary>
        /// ?fieldId,?memberId,?itemId
        /// </summary>
        public const string INSERT_MEMBER_DATA = @"INSERT INTO podioTest.MemberFieldData(PodioFieldId,MemberId,PodioItemId) VALUES(?fieldId,?memberId,?itemId);";
        ///// <summary>
        ///// ?fieldId,?amount,?currency,?itemId
        ///// </summary>
        public const string INSERT_MONEY_DATA = @"INSERT INTO podioTest.MoneyFieldData(PodioFieldId,Amount,Currency,PodioItemId) VALUES(?fieldId,?amount,?currency,?itemId);";
        /// <summary>
        /// ?fieldId,?value,?itemId
        /// </summary>
        public const string INSERT_NUMBER_DATA = @"INSERT INTO podioTest.NumberFieldData(PodioFieldId,`Value`,PodioItemId) VALUES(?fieldId,?value,?itemId);";
        /// <summary>
        /// ?fieldId,?phoneOrEmail,?type,?value,?itemId
        /// </summary>
        public const string INSERT_PHONEEMAIL_DATA = @"INSERT INTO podioTest.PhoneEmailFieldData(PodioFieldId,PhoneOrEmail,`Type`,`Value`,PodioItemId) VALUES(?fieldId,?phoneOrEmail,?type,?value,?itemId);";
        /// <summary>
        /// ?fieldId,?progress,?itemId
        /// </summary>
        public const string INSERT_PROGRESS_DATA = @"INSERT INTO podioTest.ProgressFieldData(PodioFieldId,Progress,PodioItemId) VALUES(?fieldId,?progress,?itemId);";
        /// <summary>
        /// ?fieldId,?ref,?itemId
        /// </summary>
        public const string INSERT_RELATION_DATA = @"INSERT INTO podioTest.RelationFieldData(PodioFieldId,RefPodioItemId,PodioItemId) VALUES(?fieldId,?ref,?itemId);";
        /// <summary>
        /// ?fieldId,?text,?itemId
        /// </summary>
        public const string INSERT_TEXT_DATA = @"INSERT INTO podioTest.TextFieldData(PodioFieldId,Text,PodioItemId) VALUES(?fieldId,?text,?itemId);";
    }
}
