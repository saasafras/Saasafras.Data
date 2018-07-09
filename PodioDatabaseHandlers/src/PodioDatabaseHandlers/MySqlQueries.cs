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
        public const string SELECT_APP_FIELDS = @"SELECT v.PodioFieldId, v.ExternalId, v.`Type`, v.`Name` FROM podioTest.PodioAppView v WHERE PodioAppId = ?podioAppId;";
        /// <summary>
        /// ?podioAppId,?itemId,?revision,?clientId,?envId
        /// </summary>
        public const string INSERT_ITEM = @"INSERT INTO PodioItem(PodioAppId,ItemId,Revision,ClientId,EnvId) VALUES(?podioAppId,?itemId,?revision,?clientId,?envId);";
		/// <summary>
		/// ?itemId,?revision
		/// </summary>
		public const string SELECT_ITEM_REVISION = @"SELECT COALESCE(SUM(Id),0) AS Id FROM PodioItem WHERE `ItemId` = ?itemId AND `Revision` = ?revision;";

		public const string UPDATE_ITEM = @"UPDATE PodioItem SET Revision = ?revision WHERE `PodioAppId`=?podioAppId AND `itemId`=?itemId AND `ClientId`=?clientId AND `envId`=?envId;";
        /// <summary>
        /// ?spaceName,?bbcAppId
        /// </summary>
        public const string SELECT_SPACE_ID = @"SELECT Id FROM PodioSpace WHERE PodioSpaceName = ?spaceName AND BbcAppId = ?bbcAppId;";
        /// <summary>
        /// ?bbcAppId,?version
        /// </summary>
        public const string SELECT_BBCAPP_ID = @"SELECT Id FROM BbcApp WHERE BbcAppId = ?bbcAppId AND Version = ?version;";
        
        /// <summary>
        /// ?itemId
        /// </summary>
		public const string DELETE_ITEM = 
			@"DELETE FROM CategoryFieldData WHERE PodioItemId = ?itemId;
            DELETE FROM ContactFieldData WHERE PodioItemId = ?itemId;
            DELETE FROM DateFieldData WHERE PodioItemId = ?itemId;
            DELETE FROM DurationFieldData WHERE PodioItemId = ?itemId;
            DELETE FROM LocationFieldData WHERE PodioItemId = ?itemId;
            DELETE FROM MemberFieldData WHERE PodioItemId = ?itemId;
            DELETE FROM MoneyFieldData WHERE PodioItemId = ?itemId;
            DELETE FROM NumberFieldData WHERE PodioItemId = ?itemId;
            DELETE FROM PhoneEmailFieldData WHERE PodioItemId = ?itemId;
            DELETE FROM ProgressFieldData WHERE PodioItemId = ?itemId;
            DELETE FROM RelationFieldData WHERE PodioItemId = ?itemId;
            DELETE FROM TextFieldData WHERE PodioItemId = ?itemId;
            DELETE FROM PodioItem WHERE Id = ?itemId;";
		
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
        /// <summary>
        /// The sp rebuild app tables.
        /// </summary>
		public const string SP_REBUILD_APP_TABLES = @"admin_rebuild_app_tables";

		public const string MAIN_PODIO_APP_TABLE_CREATE = @"CREATE TABLE `{0}Table` (
                `PodioItemId` int(11) unsigned NOT NULL,
                `ClientId` varchar(45) DEFAULT NULL,
                `ItemId` int(11) DEFAULT NULL,
                `Revision` smallint(3) DEFAULT NULL,
                `EnvId` varchar(45) DEFAULT NULL,{1}
                ) ENGINE=InnoDB DEFAULT CHARSET=latin1;";

		public const string ADD_TEXT_FIELD_TO_PODIO_APP_TABLE = @"`{0}` TEXT";

		public const string MAIN_PODIO_APP_VIEW_CREATE = @"CREATE VIEW `{0}View` AS SELECT 
                `PodioItemFieldViewTable`.`PodioItemId` AS `PodioItemId`,
                `PodioItemFieldViewTable`.`ClientId` AS `ClientId`,
                `PodioItemFieldViewTable`.`ItemId` AS `ItemId`,
                `PodioItemFieldViewTable`.`Revision` AS `Revision`,
                `PodioItemFieldViewTable`.`EnvId` AS `EnvId`,{1}
                FROM `PodioItemFieldViewTable`
                WHERE
                (`PodioItemFieldViewTable`.`PodioAppId` = {2})
                GROUP BY `PodioItemFieldViewTable`.`PodioItemId`";

        public const string ADD_MAX_FIELD_STATEMENT_TO_PODIO_APP_VIEW = @"MAX((CASE
            WHEN
                ((`PodioItemFieldViewTable`.`Type` = '{0}')
                    AND (`PodioItemFieldViewTable`.`Name` = '{1}'))
            THEN
                `PodioItemFieldViewTable`.`ReferencedData`
            ELSE NULL
        END)) AS `{1}`";

    }
}
