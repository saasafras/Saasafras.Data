using System;
namespace BrickBridge.Lambda.MySql
{
    public class MySqlQueries
    {        
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
        /// ?id
        /// </summary>
		public const string DELETE_ITEM_REVISION = 
			@"DELETE FROM CategoryFieldData WHERE PodioItemId = ?id;
            DELETE FROM ContactFieldData WHERE PodioItemId = ?id;
            DELETE FROM DateFieldData WHERE PodioItemId = ?id;
            DELETE FROM DurationFieldData WHERE PodioItemId = ?id;
            DELETE FROM LocationFieldData WHERE PodioItemId = ?id;
            DELETE FROM MemberFieldData WHERE PodioItemId = ?id;
            DELETE FROM MoneyFieldData WHERE PodioItemId = ?id;
            DELETE FROM NumberFieldData WHERE PodioItemId = ?id;
            DELETE FROM PhoneEmailFieldData WHERE PodioItemId = ?id;
            DELETE FROM ProgressFieldData WHERE PodioItemId = ?id;
            DELETE FROM RelationFieldData WHERE PodioItemId = ?id;
            DELETE FROM TextFieldData WHERE PodioItemId = ?id;
            DELETE FROM PodioItem WHERE Id = ?id;";

        /// <summary>
        /// The delete all revisions.
        /// </summary>
        public const string DELETE_ALL_REVISIONS =
            @"DELETE
            pi,co,ca,da,du,lo,me,mo,nu,ph,pr,re,te
            FROM PodioItem pi
            LEFT JOIN CategoryFieldData co ON pi.Id = co.PodioItemId
            LEFT JOIN ContactFieldData ca ON pi.Id = ca.PodioItemId
            LEFT JOIN DateFieldData da ON pi.Id = da.PodioItemId
            LEFT JOIN DurationFieldData du ON pi.Id = du.PodioItemId
            LEFT JOIN LocationFieldData lo ON pi.Id = lo.PodioItemId
            LEFT JOIN MemberFieldData me ON pi.Id = me.PodioItemId
            LEFT JOIN MoneyFieldData mo ON pi.Id = mo.PodioItemId
            LEFT JOIN NumberFieldData nu ON pi.Id = nu.PodioItemId
            LEFT JOIN PhoneEmailFieldData ph ON pi.Id = ph.PodioItemId
            LEFT JOIN ProgressFieldData pr ON pi.Id = pr.PodioItemId
            LEFT JOIN RelationFieldData re ON pi.Id = re.PodioItemId
            LEFT JOIN TextFieldData te ON pi.Id = te.PodioItemId
            WHERE pi.ItemId = ?itemId;
            ";
        
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
        /// <summary>
        /// rebuild everything up to solution-specific views and tables
        /// </summary>
        public const string SP_REBUILD_CORE_TABLES = @"admin_rebuild_core_tables";

        public const string MAIN_PODIO_APP_TABLE_CREATE = @"DROP TABLE IF EXISTS `{0}Table`;
                CREATE TABLE `{0}Table` (
                `PodioItemId` int(11) unsigned NOT NULL,
                `ClientId` varchar(45) DEFAULT NULL,
                `ItemId` int(11) DEFAULT NULL,
                `Revision` smallint(3) DEFAULT NULL,
                `EnvId` varchar(45) DEFAULT NULL,{1}
                ) ENGINE=InnoDB DEFAULT CHARSET=latin1;";

		public const string ADD_TEXT_FIELD_TO_PODIO_APP_TABLE = @"`{0}` TEXT";

        public const string MAIN_PODIO_APP_VIEW_CREATE = @"DROP VIEW IF EXISTS `{0}View`;
                CREATE VIEW `{0}View` AS SELECT 
                `PodioItemFieldViewTable`.`PodioItemId` AS `PodioItemId`,
                `PodioItemFieldViewTable`.`ClientId` AS `ClientId`,
                `PodioItemFieldViewTable`.`ItemId` AS `ItemId`,
                `PodioItemFieldViewTable`.`Revision` AS `Revision`,
                `PodioItemFieldViewTable`.`EnvId` AS `EnvId`,{1}
                FROM `PodioItemFieldViewTable`
                WHERE
                (`PodioItemFieldViewTable`.`PodioAppId` = {2})
                GROUP BY `PodioItemFieldViewTable`.`PodioItemId`;";

        public const string ADD_MAX_FIELD_STATEMENT_TO_PODIO_APP_VIEW = @"MAX((CASE
            WHEN
                ((`PodioItemFieldViewTable`.`Type` = '{0}')
                    AND (`PodioItemFieldViewTable`.`Name` = '{1}'))
            THEN
                `PodioItemFieldViewTable`.`ReferencedData`
            ELSE NULL
        END)) AS `{1}`";

		public const string INSERT_BBC_APP = @"INSERT INTO BbcApp(BbcAppName,BbcAppId,Version) VALUES(?bbcAppName,?bbcAppId,?version);";
		public const string INSERT_PODIO_SPACE = @"INSERT INTO PodioSpace(BbcAppId,PodioSpaceName) VALUES(?bbcAppId,?podioSpaceName);";
        public const string INSERT_PODIO_APP = @"INSERT INTO PodioApp(PodioAppName,PodioSpaceId) VALUES(?appName,?spaceId);";
		public const string INSERT_PODIO_FIELD = @"INSERT INTO PodioField(PodioAppId,`Type`,`Name`,ExternalId) VALUES(?podioAppId,?type,?name,?externalId);";
    }
}
