CREATE TABLE `BbcApp` (
  `Id` int(11) NOT NULL AUTO_INCREMENT,
  `BbcAppName` varchar(100) NOT NULL,
  `BbcAppId` varchar(100) NOT NULL,
  `Version` varchar(20) DEFAULT NULL,
  PRIMARY KEY (`Id`),
  UNIQUE KEY `BbcAppId_UNIQUE` (`Id`)
) ENGINE=InnoDB AUTO_INCREMENT=7 DEFAULT CHARSET=latin1;

CREATE TABLE `PodioSpace` (
  `Id` int(11) NOT NULL AUTO_INCREMENT,
  `BbcAppId` int(11) NOT NULL,
  `PodioSpaceName` varchar(100) NOT NULL,
  PRIMARY KEY (`Id`),
  UNIQUE KEY `idPodioSpace_UNIQUE` (`Id`),
  KEY `PodioSpaceBbcAppId_idx` (`BbcAppId`),
  CONSTRAINT `PodioSpaceBbcAppId` FOREIGN KEY (`BbcAppId`) REFERENCES `BbcApp` (`Id`) ON DELETE NO ACTION ON UPDATE NO ACTION
) ENGINE=InnoDB AUTO_INCREMENT=45 DEFAULT CHARSET=latin1;

CREATE TABLE `PodioApp` (
  `Id` int(11) NOT NULL AUTO_INCREMENT,
  `PodioAppName` varchar(100) NOT NULL,
  `PodioSpaceId` int(11) DEFAULT NULL,
  PRIMARY KEY (`Id`),
  UNIQUE KEY `idPodioApp_UNIQUE` (`Id`),
  KEY `PodioSpaceId_idx` (`PodioSpaceId`),
  CONSTRAINT `PodioSpaceId` FOREIGN KEY (`PodioSpaceId`) REFERENCES `PodioSpace` (`Id`) ON DELETE NO ACTION ON UPDATE NO ACTION
) ENGINE=InnoDB AUTO_INCREMENT=200 DEFAULT CHARSET=latin1;

CREATE TABLE `PodioItem` (
  `Id` int(11) NOT NULL AUTO_INCREMENT,
  `PodioAppId` int(11) NOT NULL,
  `ItemId` int(11) NOT NULL,
  `Revision` smallint(3) DEFAULT NULL,
  `ClientId` varchar(45) DEFAULT NULL,
  `EnvId` varchar(45) DEFAULT NULL,
  PRIMARY KEY (`Id`),
  UNIQUE KEY `Id_UNIQUE` (`Id`),
  KEY `PodioAppItemId_idx` (`PodioAppId`),
  CONSTRAINT `PodioAppItemId` FOREIGN KEY (`PodioAppId`) REFERENCES `PodioApp` (`Id`) ON DELETE NO ACTION ON UPDATE NO ACTION
) ENGINE=InnoDB AUTO_INCREMENT=541470 DEFAULT CHARSET=latin1;

CREATE TABLE `PodioField` (
  `Id` int(11) NOT NULL AUTO_INCREMENT,
  `PodioAppId` int(11) NOT NULL,
  `Type` char(12) DEFAULT NULL,
  `Name` varchar(150) DEFAULT NULL,
  `ExternalId` varchar(100) DEFAULT NULL,
  PRIMARY KEY (`Id`),
  UNIQUE KEY `Id_UNIQUE` (`Id`),
  KEY `PodioAppId_idx` (`PodioAppId`),
  CONSTRAINT `PodioAppId` FOREIGN KEY (`PodioAppId`) REFERENCES `PodioApp` (`Id`) ON DELETE NO ACTION ON UPDATE NO ACTION
) ENGINE=InnoDB AUTO_INCREMENT=6062 DEFAULT CHARSET=latin1;

CREATE TABLE `PodioContacts` (
  `ProfileId` int(11) unsigned NOT NULL,
  `Phone` varchar(45) DEFAULT NULL,
  `Address` varchar(45) DEFAULT NULL,
  `City` varchar(45) DEFAULT NULL,
  `UserId` int(11) DEFAULT NULL,
  `Name` varchar(45) DEFAULT NULL,
  `Zip` varchar(45) DEFAULT NULL,
  `State` varchar(45) DEFAULT NULL,
  `Email` varchar(45) DEFAULT NULL,
  `Type` varchar(45) DEFAULT NULL
) ENGINE=InnoDB DEFAULT CHARSET=latin1;

CREATE TABLE `TextFieldData` (
  `Id` int(11) NOT NULL AUTO_INCREMENT,
  `PodioFieldId` int(11) NOT NULL,
  `Text` varchar(3000) DEFAULT NULL,
  `PodioItemId` int(11) DEFAULT NULL,
  PRIMARY KEY (`Id`),
  UNIQUE KEY `Id_UNIQUE` (`Id`),
  KEY `PodioTextFieldId_idx` (`PodioFieldId`),
  KEY `PodioItemId_idx` (`PodioItemId`),
  CONSTRAINT `PodioTextFieldId` FOREIGN KEY (`PodioFieldId`) REFERENCES `PodioField` (`Id`) ON DELETE NO ACTION ON UPDATE NO ACTION,
  CONSTRAINT `PodioTextItemId` FOREIGN KEY (`PodioItemId`) REFERENCES `PodioItem` (`Id`) ON DELETE CASCADE ON UPDATE NO ACTION
) ENGINE=InnoDB AUTO_INCREMENT=3974335 DEFAULT CHARSET=latin1;
CREATE TABLE `RelationFieldData` (
  `Id` int(11) NOT NULL AUTO_INCREMENT,
  `PodioFieldId` int(11) NOT NULL,
  `PodioItemId` int(11) NOT NULL,
  `RefPodioItemId` int(11) DEFAULT NULL,
  PRIMARY KEY (`Id`),
  UNIQUE KEY `Id_UNIQUE` (`Id`),
  KEY `PodioFieldId_idx` (`PodioFieldId`),
  KEY `PodioItemId_idx` (`PodioItemId`),
  CONSTRAINT `PodioRelationFieldId` FOREIGN KEY (`PodioFieldId`) REFERENCES `PodioField` (`Id`) ON DELETE NO ACTION ON UPDATE NO ACTION,
  CONSTRAINT `PodioRelationItemId` FOREIGN KEY (`PodioItemId`) REFERENCES `PodioItem` (`Id`) ON DELETE CASCADE ON UPDATE NO ACTION
) ENGINE=InnoDB AUTO_INCREMENT=456149 DEFAULT CHARSET=latin1;
CREATE TABLE `ProgressFieldData` (
  `Id` int(11) NOT NULL AUTO_INCREMENT,
  `PodioFieldId` int(11) NOT NULL,
  `Progress` tinyint(3) DEFAULT NULL,
  `PodioItemId` int(11) DEFAULT NULL,
  PRIMARY KEY (`Id`),
  UNIQUE KEY `Id_UNIQUE` (`Id`),
  KEY `PodioProgressFieldId_idx` (`PodioFieldId`),
  KEY `PodioItemId_idx` (`PodioItemId`),
  CONSTRAINT `PodioProgressFieldId` FOREIGN KEY (`PodioFieldId`) REFERENCES `PodioField` (`Id`) ON DELETE NO ACTION ON UPDATE NO ACTION,
  CONSTRAINT `PodioProgressItemId` FOREIGN KEY (`PodioItemId`) REFERENCES `PodioItem` (`Id`) ON DELETE CASCADE ON UPDATE NO ACTION
) ENGINE=InnoDB DEFAULT CHARSET=latin1;
CREATE TABLE `PhoneEmailFieldData` (
  `Id` int(11) NOT NULL AUTO_INCREMENT,
  `PodioFieldId` int(11) DEFAULT NULL,
  `PhoneOrEmail` char(1) DEFAULT NULL,
  `Type` char(12) DEFAULT NULL,
  `Value` varchar(256) DEFAULT NULL,
  `PodioItemId` int(11) DEFAULT NULL,
  PRIMARY KEY (`Id`),
  UNIQUE KEY `Id_UNIQUE` (`Id`),
  KEY `PodioPhoneEmailFieldId_idx` (`PodioFieldId`),
  KEY `PodioItemId_idx` (`PodioItemId`),
  CONSTRAINT `PodioPhoneEmailFieldId` FOREIGN KEY (`PodioFieldId`) REFERENCES `PodioField` (`Id`) ON DELETE NO ACTION ON UPDATE NO ACTION,
  CONSTRAINT `PodioPhoneEmailItemId` FOREIGN KEY (`PodioItemId`) REFERENCES `PodioItem` (`Id`) ON DELETE CASCADE ON UPDATE NO ACTION
) ENGINE=InnoDB AUTO_INCREMENT=705943 DEFAULT CHARSET=latin1;
CREATE TABLE `NumberFieldData` (
  `Id` int(11) NOT NULL AUTO_INCREMENT,
  `PodioFieldId` int(11) NOT NULL,
  `Value` double DEFAULT NULL,
  `PodioItemId` int(11) DEFAULT NULL,
  PRIMARY KEY (`Id`),
  UNIQUE KEY `Id_UNIQUE` (`Id`),
  KEY `PodioNumberFieldId_idx` (`PodioFieldId`),
  KEY `PodioItemId_idx` (`PodioItemId`),
  CONSTRAINT `PodioNumberFieldId` FOREIGN KEY (`PodioFieldId`) REFERENCES `PodioField` (`Id`) ON DELETE NO ACTION ON UPDATE NO ACTION,
  CONSTRAINT `PodioNumberItemId` FOREIGN KEY (`PodioItemId`) REFERENCES `PodioItem` (`Id`) ON DELETE CASCADE ON UPDATE NO ACTION
) ENGINE=InnoDB AUTO_INCREMENT=1009728 DEFAULT CHARSET=latin1;
CREATE TABLE `MoneyFieldData` (
  `Id` int(11) NOT NULL AUTO_INCREMENT,
  `PodioFieldId` int(11) NOT NULL,
  `Amount` decimal(11,2) DEFAULT NULL,
  `Currency` varchar(45) DEFAULT NULL,
  `PodioItemId` int(11) DEFAULT NULL,
  PRIMARY KEY (`Id`),
  UNIQUE KEY `Id_UNIQUE` (`Id`),
  KEY `PodioMoneyFieldId_idx` (`PodioFieldId`),
  KEY `PodioItemId_idx` (`PodioItemId`),
  CONSTRAINT `PodioMoneyFieldId` FOREIGN KEY (`PodioFieldId`) REFERENCES `PodioField` (`Id`) ON DELETE NO ACTION ON UPDATE NO ACTION,
  CONSTRAINT `PodioMoneyItemId` FOREIGN KEY (`PodioItemId`) REFERENCES `PodioItem` (`Id`) ON DELETE CASCADE ON UPDATE NO ACTION
) ENGINE=InnoDB AUTO_INCREMENT=365817 DEFAULT CHARSET=latin1;
CREATE TABLE `MemberFieldData` (
  `Id` int(11) NOT NULL AUTO_INCREMENT,
  `PodioFieldId` int(11) NOT NULL,
  `MemberId` int(11) NOT NULL,
  `PodioItemId` int(11) DEFAULT NULL,
  PRIMARY KEY (`Id`),
  UNIQUE KEY `Id_UNIQUE` (`Id`),
  KEY `PodioMemberFieldId_idx` (`PodioFieldId`),
  KEY `PodioItemId_idx` (`PodioItemId`),
  CONSTRAINT `PodioMemberFieldId` FOREIGN KEY (`PodioFieldId`) REFERENCES `PodioField` (`Id`) ON DELETE NO ACTION ON UPDATE NO ACTION,
  CONSTRAINT `PodioMemberItemId` FOREIGN KEY (`PodioItemId`) REFERENCES `PodioItem` (`Id`) ON DELETE CASCADE ON UPDATE NO ACTION
) ENGINE=InnoDB DEFAULT CHARSET=latin1;
CREATE TABLE `LocationFieldData` (
  `Id` int(11) NOT NULL AUTO_INCREMENT,
  `PodioFieldId` int(11) NOT NULL,
  `PodioItemId` int(11) DEFAULT NULL,
  `Location` varchar(200) DEFAULT NULL,
  PRIMARY KEY (`Id`),
  UNIQUE KEY `Id_UNIQUE` (`Id`),
  KEY `PodioLocationFieldId_idx` (`PodioFieldId`),
  KEY `PodioItemId_idx` (`PodioItemId`),
  CONSTRAINT `PodioLocationFieldId` FOREIGN KEY (`PodioFieldId`) REFERENCES `PodioField` (`Id`) ON DELETE NO ACTION ON UPDATE NO ACTION,
  CONSTRAINT `PodioLocationItemId` FOREIGN KEY (`PodioItemId`) REFERENCES `PodioItem` (`Id`) ON DELETE CASCADE ON UPDATE NO ACTION
) ENGINE=InnoDB AUTO_INCREMENT=12846 DEFAULT CHARSET=latin1;
CREATE TABLE `DurationFieldData` (
  `Id` int(11) NOT NULL AUTO_INCREMENT,
  `PodioFieldId` int(11) NOT NULL,
  `SecondsDuration` int(11) DEFAULT NULL,
  `PodioItemId` int(11) DEFAULT NULL,
  PRIMARY KEY (`Id`),
  UNIQUE KEY `Id_UNIQUE` (`Id`),
  KEY `PodioDurationFieldId_idx` (`PodioFieldId`),
  KEY `PodioItemId_idx` (`PodioItemId`),
  CONSTRAINT `PodioDurationFieldId` FOREIGN KEY (`PodioFieldId`) REFERENCES `PodioField` (`Id`) ON DELETE NO ACTION ON UPDATE NO ACTION,
  CONSTRAINT `PodioDurationItemId` FOREIGN KEY (`PodioItemId`) REFERENCES `PodioItem` (`Id`) ON DELETE CASCADE ON UPDATE NO ACTION
) ENGINE=InnoDB AUTO_INCREMENT=14684 DEFAULT CHARSET=latin1;
CREATE TABLE `DateFieldData` (
  `Id` int(11) NOT NULL AUTO_INCREMENT,
  `PodioFieldId` int(11) NOT NULL,
  `StartDate` datetime DEFAULT NULL,
  `EndDate` datetime DEFAULT NULL,
  `PodioItemId` int(11) DEFAULT NULL,
  PRIMARY KEY (`Id`),
  UNIQUE KEY `Id_UNIQUE` (`Id`),
  KEY `PodioDateFieldId_idx` (`PodioFieldId`),
  KEY `PodioItemId_idx` (`PodioItemId`),
  CONSTRAINT `PodioDateFieldId` FOREIGN KEY (`PodioFieldId`) REFERENCES `PodioField` (`Id`) ON DELETE NO ACTION ON UPDATE NO ACTION,
  CONSTRAINT `PodioDateItemId` FOREIGN KEY (`PodioItemId`) REFERENCES `PodioItem` (`Id`) ON DELETE CASCADE ON UPDATE NO ACTION
) ENGINE=InnoDB AUTO_INCREMENT=1940498 DEFAULT CHARSET=latin1;
CREATE TABLE `ContactFieldData` (
  `Id` int(11) NOT NULL AUTO_INCREMENT,
  `PodioFieldId` int(11) NOT NULL,
  `PodioContactId` int(11) NOT NULL,
  `PodioItemId` int(11) DEFAULT NULL,
  PRIMARY KEY (`Id`),
  UNIQUE KEY `Id_UNIQUE` (`Id`),
  KEY `PodioContactFieldId_idx` (`PodioFieldId`),
  KEY `PodioItemId_idx` (`PodioItemId`),
  KEY `PodioContactItemId_idx` (`PodioItemId`),
  CONSTRAINT `PodioContactFieldId` FOREIGN KEY (`PodioFieldId`) REFERENCES `PodioField` (`Id`) ON DELETE NO ACTION ON UPDATE NO ACTION,
  CONSTRAINT `PodioContactItemId` FOREIGN KEY (`PodioItemId`) REFERENCES `PodioItem` (`Id`) ON DELETE CASCADE ON UPDATE NO ACTION
) ENGINE=InnoDB AUTO_INCREMENT=329105 DEFAULT CHARSET=latin1;
CREATE TABLE `CategoryFieldData` (
  `Id` int(11) NOT NULL AUTO_INCREMENT,
  `PodioFieldId` int(11) NOT NULL,
  `CategoryOptionId` int(11) NOT NULL,
  `CategoryOptionText` varchar(100) NOT NULL,
  `PodioItemId` int(11) DEFAULT NULL,
  PRIMARY KEY (`Id`),
  UNIQUE KEY `Id_UNIQUE` (`Id`),
  KEY `PodioFieldId_idx` (`PodioFieldId`),
  KEY `PodioItemId_idx` (`PodioItemId`),
  CONSTRAINT `PodioCategoryFieldId` FOREIGN KEY (`PodioFieldId`) REFERENCES `PodioField` (`Id`) ON DELETE NO ACTION ON UPDATE NO ACTION,
  CONSTRAINT `PodioItemId` FOREIGN KEY (`PodioItemId`) REFERENCES `PodioItem` (`Id`) ON DELETE CASCADE ON UPDATE NO ACTION
) ENGINE=InnoDB AUTO_INCREMENT=5942918 DEFAULT CHARSET=latin1;

CREATE TABLE `PodioItemFieldViewTable` (
  `PodioItemId` int(11) NOT NULL DEFAULT '0',
  `ClientId` varchar(45) NOT NULL,
  `EnvId` varchar(45) NOT NULL,
  `PodioAppId` int(11) NOT NULL,
  `Name` varchar(150) NOT NULL,
  `Type` char(12) DEFAULT NULL,
  `ItemId` int(11) DEFAULT NULL,
  `Revision` smallint(3) DEFAULT NULL,
  `ReferencedData` text
) ENGINE=InnoDB DEFAULT CHARSET=latin1;

CREATE TABLE `PodioItemLastRevisionTable` (
  `PodioItemId` int(11) NOT NULL DEFAULT '0',
  `ItemId` int(11) DEFAULT NULL,
  `ClientId` varchar(45) NOT NULL,
  `EnvId` varchar(45) NOT NULL,
  `PodioAppId` int(11) NOT NULL,
  `Revision` smallint(3) DEFAULT NULL,
  `PodioFieldId` int(11) NOT NULL DEFAULT '0',
  `Name` varchar(150) DEFAULT NULL,
  `ExternalId` varchar(100) NOT NULL,
  `Type` char(12) DEFAULT NULL,
  `FieldDataId` bigint(11) DEFAULT NULL
) ENGINE=InnoDB DEFAULT CHARSET=latin1;

CREATE TABLE `PodioItemViewTable` (
  `PodioItemId` int(11) NOT NULL DEFAULT '0',
  `ItemId` int(11) DEFAULT NULL,
  `ClientId` varchar(45) NOT NULL,
  `EnvId` varchar(45) NOT NULL,
  `PodioAppId` int(11) NOT NULL,
  `Revision` smallint(3) DEFAULT NULL,
  `PodioFieldId` int(11) NOT NULL DEFAULT '0',
  `Name` varchar(150) CHARACTER SET big5 COLLATE big5_bin DEFAULT NULL,
  `ExternalId` varchar(100) NOT NULL,
  `Type` char(12) DEFAULT NULL,
  `FieldDataId` bigint(11) DEFAULT NULL
) ENGINE=InnoDB DEFAULT CHARSET=latin1;

CREATE ALGORITHM=UNDEFINED DEFINER=`mpactpro`@`%` SQL SECURITY DEFINER VIEW `PodioAppView` AS select `bbc`.`BbcAppId` AS `BbcAppId`,`bbc`.`Version` AS `Version`,`space`.`PodioSpaceName` AS `PodioSpaceName`,`app`.`PodioAppName` AS `PodioAppName`,`app`.`Id` AS `PodioAppId`,`field`.`Name` AS `Name`,`field`.`Type` AS `Type`,`field`.`ExternalId` AS `ExternalId`,`field`.`Id` AS `PodioFieldId` from (((`BbcApp` `bbc` join `PodioSpace` `space` on((`bbc`.`Id` = `space`.`BbcAppId`))) join `PodioApp` `app` on((`app`.`PodioSpaceId` = `space`.`Id`))) join `PodioField` `field` on((`field`.`PodioAppId` = `app`.`Id`)));
CREATE ALGORITHM=UNDEFINED DEFINER=`mpactpro`@`%` SQL SECURITY DEFINER VIEW `PodioItemView` AS select `i`.`Id` AS `PodioItemId`,`i`.`ItemId` AS `ItemId`,`i`.`ClientId` AS `ClientId`,`i`.`EnvId` AS `EnvId`,`i`.`PodioAppId` AS `PodioAppId`,`i`.`Revision` AS `Revision`,`f`.`Id` AS `PodioFieldId`,`f`.`Name` AS `Name`,`f`.`ExternalId` AS `ExternalId`,`f`.`Type` AS `Type`,(case `f`.`Type` when 'category' then `cat`.`Id` when 'contact' then `con`.`Id` when 'date' then `dat`.`Id` when 'duration' then `dur`.`Id` when 'location' then `loc`.`Id` when 'member' then `mem`.`Id` when 'money' then `mon`.`Id` when 'number' then `num`.`Id` when 'phone' then `poe`.`Id` when 'email' then `poe`.`Id` when 'progress' then `pro`.`Id` when 'app' then `rel`.`Id` when 'text' then `tex`.`Id` end) AS `FieldDataId` from ((((((((((((((`PodioItem` `i` join `PodioApp` `a` on((`a`.`Id` = `i`.`PodioAppId`))) join `PodioField` `f` on((`f`.`PodioAppId` = `a`.`Id`))) left join `CategoryFieldData` `cat` on(((`i`.`Id` = `cat`.`PodioItemId`) and (`cat`.`PodioFieldId` = `f`.`Id`)))) left join `ContactFieldData` `con` on(((`i`.`Id` = `con`.`PodioItemId`) and (`con`.`PodioFieldId` = `f`.`Id`)))) left join `DateFieldData` `dat` on(((`i`.`Id` = `dat`.`PodioItemId`) and (`dat`.`PodioFieldId` = `f`.`Id`)))) left join `DurationFieldData` `dur` on(((`i`.`Id` = `dur`.`PodioItemId`) and (`dur`.`PodioFieldId` = `f`.`Id`)))) left join `LocationFieldData` `loc` on(((`i`.`Id` = `loc`.`PodioItemId`) and (`loc`.`PodioFieldId` = `f`.`Id`)))) left join `MemberFieldData` `mem` on(((`i`.`Id` = `mem`.`PodioItemId`) and (`mem`.`PodioFieldId` = `f`.`Id`)))) left join `MoneyFieldData` `mon` on(((`i`.`Id` = `mon`.`PodioItemId`) and (`mon`.`PodioFieldId` = `f`.`Id`)))) left join `NumberFieldData` `num` on(((`i`.`Id` = `num`.`PodioItemId`) and (`num`.`PodioFieldId` = `f`.`Id`)))) left join `PhoneEmailFieldData` `poe` on(((`i`.`Id` = `poe`.`PodioItemId`) and (`poe`.`PodioFieldId` = `f`.`Id`)))) left join `ProgressFieldData` `pro` on(((`i`.`Id` = `pro`.`PodioItemId`) and (`pro`.`PodioFieldId` = `f`.`Id`)))) left join `RelationFieldData` `rel` on(((`i`.`Id` = `rel`.`PodioItemId`) and (`rel`.`PodioFieldId` = `f`.`Id`)))) left join `TextFieldData` `tex` on(((`i`.`Id` = `tex`.`PodioItemId`) and (`tex`.`PodioFieldId` = `f`.`Id`)))) order by `i`.`ItemId`,`f`.`Id`;
CREATE ALGORITHM=UNDEFINED DEFINER=`mpactpro`@`%` SQL SECURITY DEFINER VIEW `PodioItemMaxRevision` AS select `PodioItemViewTable`.`ItemId` AS `ItemId`,max(`PodioItemViewTable`.`Revision`) AS `Revision` from `PodioItemViewTable` group by `PodioItemViewTable`.`ItemId`;
CREATE ALGORITHM=UNDEFINED DEFINER=`mpactpro`@`%` SQL SECURITY DEFINER VIEW `PodioItemFieldView` AS select `piv`.`PodioItemId` AS `PodioItemId`,`piv`.`ClientId` AS `ClientId`,`piv`.`EnvId` AS `EnvId`,`piv`.`PodioAppId` AS `PodioAppId`,`piv`.`Name` AS `Name`,`piv`.`Type` AS `Type`,`piv`.`ItemId` AS `ItemId`,`piv`.`Revision` AS `Revision`,(case `piv`.`Type` when 'app' then `rel`.`RefPodioItemId` when 'category' then `cat`.`CategoryOptionText` when 'contact' then `con`.`PodioContactId` when 'date' then `dat`.`StartDate` when 'member' then `mem`.`MemberId` when 'money' then `mon`.`Amount` when 'number' then `num`.`Value` when 'phone' then `poe`.`PhoneOrEmail` when 'email' then `poe`.`PhoneOrEmail` when 'text' then `tex`.`Text` end) AS `ReferencedData` from ((((((((((((`PodioItemLastRevisionTable` `piv` left join `RelationFieldData` `rel` on(((`piv`.`PodioItemId` = `rel`.`PodioItemId`) and (`piv`.`FieldDataId` = `rel`.`Id`)))) left join `CategoryFieldData` `cat` on(((`piv`.`PodioItemId` = `cat`.`PodioItemId`) and (`piv`.`FieldDataId` = `cat`.`Id`)))) left join `ContactFieldData` `con` on(((`piv`.`PodioItemId` = `con`.`PodioItemId`) and (`piv`.`FieldDataId` = `con`.`Id`)))) left join `DateFieldData` `dat` on(((`piv`.`PodioItemId` = `dat`.`PodioItemId`) and (`piv`.`FieldDataId` = `dat`.`Id`)))) left join `DurationFieldData` `dur` on(((`piv`.`PodioItemId` = `dur`.`PodioItemId`) and (`piv`.`FieldDataId` = `dur`.`Id`)))) left join `LocationFieldData` `loc` on(((`piv`.`PodioItemId` = `loc`.`PodioItemId`) and (`piv`.`FieldDataId` = `loc`.`Id`)))) left join `MemberFieldData` `mem` on(((`piv`.`PodioItemId` = `mem`.`PodioItemId`) and (`piv`.`FieldDataId` = `mem`.`Id`)))) left join `MoneyFieldData` `mon` on(((`piv`.`PodioItemId` = `mon`.`PodioItemId`) and (`piv`.`FieldDataId` = `mon`.`Id`)))) left join `NumberFieldData` `num` on(((`piv`.`PodioItemId` = `num`.`PodioItemId`) and (`piv`.`FieldDataId` = `num`.`Id`)))) left join `PhoneEmailFieldData` `poe` on(((`piv`.`PodioItemId` = `poe`.`PodioItemId`) and (`piv`.`FieldDataId` = `poe`.`Id`)))) left join `ProgressFieldData` `pro` on(((`piv`.`PodioItemId` = `pro`.`PodioItemId`) and (`piv`.`FieldDataId` = `pro`.`Id`)))) left join `TextFieldData` `tex` on(((`piv`.`PodioItemId` = `tex`.`PodioItemId`) and (`piv`.`FieldDataId` = `tex`.`Id`))));
CREATE ALGORITHM=UNDEFINED DEFINER=`mpactpro`@`%` SQL SECURITY DEFINER VIEW `PodioItemViewByLastRevision` AS select `piv`.`PodioItemId` AS `PodioItemId`,`piv`.`ItemId` AS `ItemId`,`piv`.`ClientId` AS `ClientId`,`piv`.`EnvId` AS `EnvId`,`piv`.`PodioAppId` AS `PodioAppId`,`piv`.`Revision` AS `CurrentRevision`,`piv`.`PodioFieldId` AS `PodioFieldId`,`piv`.`Name` AS `Name`,`piv`.`ExternalId` AS `ExternalId`,`piv`.`Type` AS `Type`,`piv`.`FieldDataId` AS `FieldDataId` from (`PodioItemViewTable` `piv` join `PodioItemMaxRevision` `rev` on((`rev`.`ItemId` = `piv`.`ItemId`))) where ((`piv`.`FieldDataId` is not null) and (`piv`.`Revision` = `rev`.`Revision`)) group by `piv`.`ItemId`,`piv`.`PodioFieldId` order by `piv`.`PodioItemId`,`piv`.`PodioFieldId`;

DELIMITER $$
CREATE DEFINER=`mpactpro`@`%` PROCEDURE `admin_delete_old_item_versions`(
    IN EnvId VARCHAR(45))
BEGIN

	drop temporary table if exists temp;

    create temporary table temp
	SELECT pi1.Id FROM PodioItem pi1
	WHERE pi1.Id NOT IN
	(SELECT MAX(pi2.Id) AS Id FROM PodioItem pi2 WHERE pi2.EnvId = EnvId GROUP BY pi2.ItemId)
	AND pi1.EnvId = EnvId;

	DELETE FROM PodioItem WHERE Id IN (SELECT Id FROM temp);

    drop temporary table temp;



END$$
DELIMITER ;

DELIMITER $$
CREATE DEFINER=`mpactpro`@`%` PROCEDURE `admin_rebuild_core_tables`(
IN BbcAppId VARCHAR(100),
IN Version VARCHAR(20)
)
BEGIN
START TRANSACTION;

DROP TEMPORARY TABLE IF EXISTS `PodioApps`;
CREATE TEMPORARY TABLE `PodioApps` ENGINE=MEMORY SELECT DISTINCT PodioAppId
FROM PodioAppView v WHERE v.BbcAppId = BbcAppId AND v.Version = Version;
 
DELETE p
FROM PodioItemViewTable p
INNER JOIN `PodioApps` v ON p.PodioAppId = v.PodioAppId;

INSERT INTO PodioItemViewTable
SELECT v.* FROM PodioItemView v
INNER JOIN `PodioApps` v2 ON v.PodioAppId = v2.PodioAppId;

DELETE p
FROM PodioItemLastRevisionTable p
INNER JOIN `PodioApps` v ON p.PodioAppId = v.PodioAppId;

INSERT INTO PodioItemLastRevisionTable
SELECT v.* FROM PodioItemViewByLastRevision v
INNER JOIN `PodioApps` v2 ON v.PodioAppId = v2.PodioAppId;

DELETE p
FROM PodioItemFieldViewTable p
INNER JOIN `PodioApps` v ON p.PodioAppId = v.PodioAppId;

INSERT INTO PodioItemFieldViewTable
SELECT v.* FROM PodioItemFieldView v
INNER JOIN `PodioApps` v2 ON v.PodioAppId = v2.PodioAppId;

DROP TEMPORARY TABLE IF EXISTS `PodioApps`;
COMMIT;
END$$
DELIMITER ;

DELIMITER $$
CREATE DEFINER=`mpactpro`@`%` PROCEDURE `admin_rebuild_app_tables`(
IN BbcAppId VARCHAR(100),
IN Version VARCHAR(20),
IN RebuildSources VARCHAR(1)
)
BEGIN 
  DECLARE SPACENAME VARCHAR(100);
  DECLARE APPNAME VARCHAR(100);
  DECLARE VIEWNAME VARCHAR(200);
  DECLARE TABLENAME VARCHAR(200);
  DECLARE cur1 CURSOR FOR SELECT DISTINCT PodioSpaceName, PodioAppName FROM PodioAppView WHERE BbcAppId = BbcAppId AND Version = Version;
  TRUNCATE TABLE podioTest.MasterClientProfile;
INSERT INTO podioTest.MasterClientProfile
SELECT * FROM podioTest.MasterClientProfileView;

  OPEN cur1; 
  read_loop: LOOP
	   FETCH cur1 INTO SPACENAME, APPNAME;
       /*SELECT SPACENAME, APPNAME;*/
       SET VIEWNAME = CONCAT(SPACENAME,'-',APPNAME,'View');
       SET TABLENAME = CONCAT(SPACENAME,'-',APPNAME,'Table');
       /*SELECT VIEWNAME, TABLENAME;
       SELECT VIEWNAME;
       			SELECT * 
			FROM information_schema.tables
			WHERE table_schema = 'podioTest'
			AND table_name = VIEWNAME
			LIMIT 1;*/
       IF EXISTS(
			SELECT * 
			FROM information_schema.tables
			WHERE table_schema = 'podioTest' 
			AND table_name = VIEWNAME
			LIMIT 1)
       THEN
			SET @statement = CONCAT('TRUNCATE TABLE `', TABLENAME, '`;');
			PREPARE statement FROM @statement;
			EXECUTE statement;
			SET @statement = CONCAT('INSERT INTO `', TABLENAME, '` SELECT * FROM `', VIEWNAME, '`;');
			PREPARE statement FROM @statement;
			EXECUTE statement;
       END IF;
  END LOOP;
  CLOSE cur1;

END$$
DELIMITER ;

