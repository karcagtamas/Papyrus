CREATE TABLE IF NOT EXISTS `__EFMigrationsHistory` (
    `MigrationId` varchar(150) CHARACTER SET utf8mb4 NOT NULL,
    `ProductVersion` varchar(32) CHARACTER SET utf8mb4 NOT NULL,
    CONSTRAINT `PK___EFMigrationsHistory` PRIMARY KEY (`MigrationId`)
) CHARACTER SET=utf8mb4;

START TRANSACTION;

ALTER DATABASE CHARACTER SET utf8mb4;

CREATE TABLE `AspNetRoles` (
    `Id` varchar(255) CHARACTER SET utf8mb4 NOT NULL,
    `Name` varchar(256) CHARACTER SET utf8mb4 NULL,
    `NormalizedName` varchar(256) CHARACTER SET utf8mb4 NULL,
    `ConcurrencyStamp` longtext CHARACTER SET utf8mb4 NULL,
    CONSTRAINT `PK_AspNetRoles` PRIMARY KEY (`Id`)
) CHARACTER SET=utf8mb4;

CREATE TABLE `Languages` (
    `Id` int NOT NULL AUTO_INCREMENT,
    `Name` longtext CHARACTER SET utf8mb4 NOT NULL,
    `ShortName` varchar(255) CHARACTER SET utf8mb4 NOT NULL,
    CONSTRAINT `PK_Languages` PRIMARY KEY (`Id`)
) CHARACTER SET=utf8mb4;

CREATE TABLE `Translations` (
    `Key` varchar(255) CHARACTER SET utf8mb4 NOT NULL,
    `Segment` varchar(255) CHARACTER SET utf8mb4 NOT NULL,
    `Language` varchar(255) CHARACTER SET utf8mb4 NOT NULL,
    `Value` longtext CHARACTER SET utf8mb4 NOT NULL,
    CONSTRAINT `PK_Translations` PRIMARY KEY (`Key`, `Segment`, `Language`)
) CHARACTER SET=utf8mb4;

CREATE TABLE `AspNetRoleClaims` (
    `Id` int NOT NULL AUTO_INCREMENT,
    `RoleId` varchar(255) CHARACTER SET utf8mb4 NOT NULL,
    `ClaimType` longtext CHARACTER SET utf8mb4 NULL,
    `ClaimValue` longtext CHARACTER SET utf8mb4 NULL,
    CONSTRAINT `PK_AspNetRoleClaims` PRIMARY KEY (`Id`),
    CONSTRAINT `FK_AspNetRoleClaims_AspNetRoles_RoleId` FOREIGN KEY (`RoleId`) REFERENCES `AspNetRoles` (`Id`) ON DELETE CASCADE
) CHARACTER SET=utf8mb4;

CREATE TABLE `AspNetUsers` (
    `Id` varchar(255) CHARACTER SET utf8mb4 NOT NULL,
    `OsirisId` longtext CHARACTER SET utf8mb4 NULL,
    `LastOsirisSync` datetime(6) NULL,
    `Registration` datetime(6) NOT NULL,
    `LastLogin` datetime(6) NOT NULL,
    `FullName` varchar(100) CHARACTER SET utf8mb4 NULL,
    `BirthDay` datetime(6) NULL,
    `Disabled` tinyint(1) NOT NULL,
    `ImageId` longtext CHARACTER SET utf8mb4 NULL,
    `LanguageId` int NULL,
    `UserName` varchar(256) CHARACTER SET utf8mb4 NULL,
    `NormalizedUserName` varchar(256) CHARACTER SET utf8mb4 NULL,
    `Email` varchar(256) CHARACTER SET utf8mb4 NULL,
    `NormalizedEmail` varchar(256) CHARACTER SET utf8mb4 NULL,
    `EmailConfirmed` tinyint(1) NOT NULL,
    `PasswordHash` longtext CHARACTER SET utf8mb4 NULL,
    `SecurityStamp` longtext CHARACTER SET utf8mb4 NULL,
    `ConcurrencyStamp` longtext CHARACTER SET utf8mb4 NULL,
    `PhoneNumber` longtext CHARACTER SET utf8mb4 NULL,
    `PhoneNumberConfirmed` tinyint(1) NOT NULL,
    `TwoFactorEnabled` tinyint(1) NOT NULL,
    `LockoutEnd` datetime(6) NULL,
    `LockoutEnabled` tinyint(1) NOT NULL,
    `AccessFailedCount` int NOT NULL,
    CONSTRAINT `PK_AspNetUsers` PRIMARY KEY (`Id`),
    CONSTRAINT `FK_AspNetUsers_Languages_LanguageId` FOREIGN KEY (`LanguageId`) REFERENCES `Languages` (`Id`) ON DELETE SET NULL
) CHARACTER SET=utf8mb4;

CREATE TABLE `ActionLogs` (
    `Id` bigint NOT NULL AUTO_INCREMENT,
    `Key` longtext CHARACTER SET utf8mb4 NOT NULL,
    `Segment` longtext CHARACTER SET utf8mb4 NOT NULL,
    `Language` longtext CHARACTER SET utf8mb4 NOT NULL,
    `Type` longtext CHARACTER SET utf8mb4 NOT NULL,
    `Text` longtext CHARACTER SET utf8mb4 NOT NULL,
    `Creation` datetime(6) NOT NULL,
    `PerformerId` varchar(255) CHARACTER SET utf8mb4 NULL,
    CONSTRAINT `PK_ActionLogs` PRIMARY KEY (`Id`),
    CONSTRAINT `FK_ActionLogs_AspNetUsers_PerformerId` FOREIGN KEY (`PerformerId`) REFERENCES `AspNetUsers` (`Id`) ON DELETE SET NULL
) CHARACTER SET=utf8mb4;

CREATE TABLE `AspNetUserClaims` (
    `Id` int NOT NULL AUTO_INCREMENT,
    `UserId` varchar(255) CHARACTER SET utf8mb4 NOT NULL,
    `ClaimType` longtext CHARACTER SET utf8mb4 NULL,
    `ClaimValue` longtext CHARACTER SET utf8mb4 NULL,
    CONSTRAINT `PK_AspNetUserClaims` PRIMARY KEY (`Id`),
    CONSTRAINT `FK_AspNetUserClaims_AspNetUsers_UserId` FOREIGN KEY (`UserId`) REFERENCES `AspNetUsers` (`Id`) ON DELETE CASCADE
) CHARACTER SET=utf8mb4;

CREATE TABLE `AspNetUserLogins` (
    `LoginProvider` varchar(255) CHARACTER SET utf8mb4 NOT NULL,
    `ProviderKey` varchar(255) CHARACTER SET utf8mb4 NOT NULL,
    `ProviderDisplayName` longtext CHARACTER SET utf8mb4 NULL,
    `UserId` varchar(255) CHARACTER SET utf8mb4 NOT NULL,
    CONSTRAINT `PK_AspNetUserLogins` PRIMARY KEY (`LoginProvider`, `ProviderKey`),
    CONSTRAINT `FK_AspNetUserLogins_AspNetUsers_UserId` FOREIGN KEY (`UserId`) REFERENCES `AspNetUsers` (`Id`) ON DELETE CASCADE
) CHARACTER SET=utf8mb4;

CREATE TABLE `AspNetUserRoles` (
    `UserId` varchar(255) CHARACTER SET utf8mb4 NOT NULL,
    `RoleId` varchar(255) CHARACTER SET utf8mb4 NOT NULL,
    CONSTRAINT `PK_AspNetUserRoles` PRIMARY KEY (`UserId`, `RoleId`),
    CONSTRAINT `FK_AspNetUserRoles_AspNetRoles_RoleId` FOREIGN KEY (`RoleId`) REFERENCES `AspNetRoles` (`Id`) ON DELETE CASCADE,
    CONSTRAINT `FK_AspNetUserRoles_AspNetUsers_UserId` FOREIGN KEY (`UserId`) REFERENCES `AspNetUsers` (`Id`) ON DELETE CASCADE
) CHARACTER SET=utf8mb4;

CREATE TABLE `AspNetUserTokens` (
    `UserId` varchar(255) CHARACTER SET utf8mb4 NOT NULL,
    `LoginProvider` varchar(255) CHARACTER SET utf8mb4 NOT NULL,
    `Name` varchar(255) CHARACTER SET utf8mb4 NOT NULL,
    `Value` longtext CHARACTER SET utf8mb4 NULL,
    CONSTRAINT `PK_AspNetUserTokens` PRIMARY KEY (`UserId`, `LoginProvider`, `Name`),
    CONSTRAINT `FK_AspNetUserTokens_AspNetUsers_UserId` FOREIGN KEY (`UserId`) REFERENCES `AspNetUsers` (`Id`) ON DELETE CASCADE
) CHARACTER SET=utf8mb4;

CREATE TABLE `Groups` (
    `Id` int NOT NULL AUTO_INCREMENT,
    `Name` longtext CHARACTER SET utf8mb4 NOT NULL,
    `Creation` datetime(6) NOT NULL,
    `OwnerId` varchar(255) CHARACTER SET utf8mb4 NOT NULL,
    `IsClosed` tinyint(1) NOT NULL,
    CONSTRAINT `PK_Groups` PRIMARY KEY (`Id`),
    CONSTRAINT `FK_Groups_AspNetUsers_OwnerId` FOREIGN KEY (`OwnerId`) REFERENCES `AspNetUsers` (`Id`) ON DELETE CASCADE
) CHARACTER SET=utf8mb4;

CREATE TABLE `RefreshTokens` (
    `Id` int NOT NULL AUTO_INCREMENT,
    `Token` varchar(255) CHARACTER SET utf8mb4 NOT NULL,
    `Expires` datetime(6) NOT NULL,
    `Created` datetime(6) NOT NULL,
    `Revoked` datetime(6) NULL,
    `UserId` varchar(255) CHARACTER SET utf8mb4 NOT NULL,
    `ClientId` longtext CHARACTER SET utf8mb4 NOT NULL,
    CONSTRAINT `PK_RefreshTokens` PRIMARY KEY (`Id`),
    CONSTRAINT `FK_RefreshTokens_AspNetUsers_UserId` FOREIGN KEY (`UserId`) REFERENCES `AspNetUsers` (`Id`) ON DELETE CASCADE
) CHARACTER SET=utf8mb4;

CREATE TABLE `GroupRoles` (
    `Id` int NOT NULL AUTO_INCREMENT,
    `Name` varchar(255) CHARACTER SET utf8mb4 NOT NULL,
    `GroupId` int NOT NULL,
    `ReadOnly` tinyint(1) NOT NULL,
    `IsDefault` tinyint(1) NOT NULL,
    `GroupEdit` tinyint(1) NOT NULL,
    `GroupClose` tinyint(1) NOT NULL,
    `ReadNoteList` tinyint(1) NOT NULL,
    `ReadNote` tinyint(1) NOT NULL,
    `CreateNote` tinyint(1) NOT NULL,
    `DeleteNote` tinyint(1) NOT NULL,
    `EditNote` tinyint(1) NOT NULL,
    `ReadMemberList` tinyint(1) NOT NULL,
    `EditMemberList` tinyint(1) NOT NULL,
    `ReadRoleList` tinyint(1) NOT NULL,
    `EditRoleList` tinyint(1) NOT NULL,
    `ReadGroupActionLog` tinyint(1) NOT NULL,
    `ReadNoteActionLog` tinyint(1) NOT NULL,
    `ReadTagList` tinyint(1) NOT NULL,
    `EditTagList` tinyint(1) NOT NULL,
    CONSTRAINT `PK_GroupRoles` PRIMARY KEY (`Id`),
    CONSTRAINT `AK_GroupRoles_GroupId_Name` UNIQUE (`GroupId`, `Name`),
    CONSTRAINT `FK_GroupRoles_Groups_GroupId` FOREIGN KEY (`GroupId`) REFERENCES `Groups` (`Id`) ON DELETE CASCADE
) CHARACTER SET=utf8mb4;

CREATE TABLE `Notes` (
    `Id` varchar(255) CHARACTER SET utf8mb4 NOT NULL,
    `Title` longtext CHARACTER SET utf8mb4 NOT NULL,
    `UserId` varchar(255) CHARACTER SET utf8mb4 NULL,
    `GroupId` int NULL,
    `Public` tinyint(1) NOT NULL,
    `Creation` datetime(6) NOT NULL,
    `LastUpdate` datetime(6) NOT NULL,
    `CreatorId` varchar(255) CHARACTER SET utf8mb4 NULL,
    `LastUpdaterId` varchar(255) CHARACTER SET utf8mb4 NULL,
    `Deleted` tinyint(1) NOT NULL,
    `ContentId` longtext CHARACTER SET utf8mb4 NOT NULL,
    `ContentLastEdit` datetime(6) NULL,
    CONSTRAINT `PK_Notes` PRIMARY KEY (`Id`),
    CONSTRAINT `CK_Note_Owner` CHECK ((GroupId IS NOT NULL OR UserId IS NOT NULL) AND NOT (GroupId IS NOT NULL AND UserId IS NOT NULL)),
    CONSTRAINT `FK_Notes_AspNetUsers_CreatorId` FOREIGN KEY (`CreatorId`) REFERENCES `AspNetUsers` (`Id`) ON DELETE SET NULL,
    CONSTRAINT `FK_Notes_AspNetUsers_LastUpdaterId` FOREIGN KEY (`LastUpdaterId`) REFERENCES `AspNetUsers` (`Id`) ON DELETE SET NULL,
    CONSTRAINT `FK_Notes_AspNetUsers_UserId` FOREIGN KEY (`UserId`) REFERENCES `AspNetUsers` (`Id`) ON DELETE CASCADE,
    CONSTRAINT `FK_Notes_Groups_GroupId` FOREIGN KEY (`GroupId`) REFERENCES `Groups` (`Id`) ON DELETE CASCADE
) CHARACTER SET=utf8mb4;

CREATE TABLE `Tags` (
    `Id` int NOT NULL AUTO_INCREMENT,
    `Caption` longtext CHARACTER SET utf8mb4 NOT NULL,
    `Description` longtext CHARACTER SET utf8mb4 NULL,
    `Color` longtext CHARACTER SET utf8mb4 NOT NULL,
    `GroupId` int NULL,
    `UserId` varchar(255) CHARACTER SET utf8mb4 NULL,
    `ParentId` int NULL,
    CONSTRAINT `PK_Tags` PRIMARY KEY (`Id`),
    CONSTRAINT `CK_Tag_Owner` CHECK ((GroupId IS NOT NULL OR UserId IS NOT NULL) AND NOT (GroupId IS NOT NULL AND UserId IS NOT NULL)),
    CONSTRAINT `FK_Tags_AspNetUsers_UserId` FOREIGN KEY (`UserId`) REFERENCES `AspNetUsers` (`Id`) ON DELETE CASCADE,
    CONSTRAINT `FK_Tags_Groups_GroupId` FOREIGN KEY (`GroupId`) REFERENCES `Groups` (`Id`) ON DELETE CASCADE,
    CONSTRAINT `FK_Tags_Tags_ParentId` FOREIGN KEY (`ParentId`) REFERENCES `Tags` (`Id`) ON DELETE CASCADE
) CHARACTER SET=utf8mb4;

CREATE TABLE `GroupMembers` (
    `Id` int NOT NULL AUTO_INCREMENT,
    `UserId` varchar(255) CHARACTER SET utf8mb4 NOT NULL,
    `GroupId` int NOT NULL,
    `RoleId` int NOT NULL,
    `AddedById` varchar(255) CHARACTER SET utf8mb4 NULL,
    `Creation` datetime(6) NOT NULL,
    CONSTRAINT `PK_GroupMembers` PRIMARY KEY (`Id`),
    CONSTRAINT `AK_GroupMembers_GroupId_UserId` UNIQUE (`GroupId`, `UserId`),
    CONSTRAINT `FK_GroupMembers_AspNetUsers_AddedById` FOREIGN KEY (`AddedById`) REFERENCES `AspNetUsers` (`Id`) ON DELETE SET NULL,
    CONSTRAINT `FK_GroupMembers_AspNetUsers_UserId` FOREIGN KEY (`UserId`) REFERENCES `AspNetUsers` (`Id`) ON DELETE CASCADE,
    CONSTRAINT `FK_GroupMembers_GroupRoles_RoleId` FOREIGN KEY (`RoleId`) REFERENCES `GroupRoles` (`Id`) ON DELETE RESTRICT,
    CONSTRAINT `FK_GroupMembers_Groups_GroupId` FOREIGN KEY (`GroupId`) REFERENCES `Groups` (`Id`) ON DELETE CASCADE
) CHARACTER SET=utf8mb4;

CREATE TABLE `EditorMembers` (
    `Id` varchar(255) CHARACTER SET utf8mb4 NOT NULL,
    `UserId` varchar(255) CHARACTER SET utf8mb4 NOT NULL,
    `NoteId` varchar(255) CHARACTER SET utf8mb4 NOT NULL,
    `ConnectionId` varchar(255) CHARACTER SET utf8mb4 NOT NULL,
    `Date` datetime(6) NOT NULL,
    CONSTRAINT `PK_EditorMembers` PRIMARY KEY (`Id`),
    CONSTRAINT `AK_EditorMembers_UserId_NoteId_ConnectionId` UNIQUE (`UserId`, `NoteId`, `ConnectionId`),
    CONSTRAINT `FK_EditorMembers_AspNetUsers_UserId` FOREIGN KEY (`UserId`) REFERENCES `AspNetUsers` (`Id`) ON DELETE CASCADE,
    CONSTRAINT `FK_EditorMembers_Notes_NoteId` FOREIGN KEY (`NoteId`) REFERENCES `Notes` (`Id`) ON DELETE CASCADE
) CHARACTER SET=utf8mb4;

CREATE TABLE `NoteTags` (
    `NoteId` varchar(255) CHARACTER SET utf8mb4 NOT NULL,
    `TagId` int NOT NULL,
    CONSTRAINT `PK_NoteTags` PRIMARY KEY (`NoteId`, `TagId`),
    CONSTRAINT `FK_NoteTags_Notes_NoteId` FOREIGN KEY (`NoteId`) REFERENCES `Notes` (`Id`) ON DELETE CASCADE,
    CONSTRAINT `FK_NoteTags_Tags_TagId` FOREIGN KEY (`TagId`) REFERENCES `Tags` (`Id`) ON DELETE CASCADE
) CHARACTER SET=utf8mb4;

INSERT INTO `Languages` (`Id`, `Name`, `ShortName`)
VALUES (1, 'English', 'en-US');
INSERT INTO `Languages` (`Id`, `Name`, `ShortName`)
VALUES (2, 'Hungarian', 'hu-HU');

INSERT INTO `Translations` (`Key`, `Language`, `Segment`, `Value`)
VALUES ('Administrator', 'en-US', 'GroupRole', 'Administrator');
INSERT INTO `Translations` (`Key`, `Language`, `Segment`, `Value`)
VALUES ('Administrator', 'hu-HU', 'GroupRole', 'Adminisztrátor');
INSERT INTO `Translations` (`Key`, `Language`, `Segment`, `Value`)
VALUES ('Administrator', 'en-US', 'Role', 'Administrator');
INSERT INTO `Translations` (`Key`, `Language`, `Segment`, `Value`)
VALUES ('Administrator', 'hu-HU', 'Role', 'Adminisztrátor');
INSERT INTO `Translations` (`Key`, `Language`, `Segment`, `Value`)
VALUES ('Close', 'en-US', 'Group', 'Closed');
INSERT INTO `Translations` (`Key`, `Language`, `Segment`, `Value`)
VALUES ('Close', 'hu-HU', 'Group', 'Csoport lezárva');
INSERT INTO `Translations` (`Key`, `Language`, `Segment`, `Value`)
VALUES ('ContentEdit', 'en-US', 'Note', 'Content is edited');
INSERT INTO `Translations` (`Key`, `Language`, `Segment`, `Value`)
VALUES ('ContentEdit', 'hu-HU', 'Note', 'Tartalom szerkesztve');
INSERT INTO `Translations` (`Key`, `Language`, `Segment`, `Value`)
VALUES ('Create', 'en-US', 'Group', 'Created');
INSERT INTO `Translations` (`Key`, `Language`, `Segment`, `Value`)
VALUES ('Create', 'hu-HU', 'Group', 'Csoport létrehozva');
INSERT INTO `Translations` (`Key`, `Language`, `Segment`, `Value`)
VALUES ('Create', 'en-US', 'Note', 'Created');
INSERT INTO `Translations` (`Key`, `Language`, `Segment`, `Value`)
VALUES ('Create', 'hu-HU', 'Note', 'Létrehozva');
INSERT INTO `Translations` (`Key`, `Language`, `Segment`, `Value`)
VALUES ('DataEdit', 'en-US', 'Group', 'Data is edited');
INSERT INTO `Translations` (`Key`, `Language`, `Segment`, `Value`)
VALUES ('DataEdit', 'hu-HU', 'Group', 'Adatok szerkesztve');
INSERT INTO `Translations` (`Key`, `Language`, `Segment`, `Value`)
VALUES ('Default', 'en-US', 'GroupRole', 'Default');
INSERT INTO `Translations` (`Key`, `Language`, `Segment`, `Value`)
VALUES ('Default', 'hu-HU', 'GroupRole', 'Alapértelmezett');
INSERT INTO `Translations` (`Key`, `Language`, `Segment`, `Value`)
VALUES ('Delete', 'en-US', 'Note', 'Deleted');
INSERT INTO `Translations` (`Key`, `Language`, `Segment`, `Value`)
VALUES ('Delete', 'hu-HU', 'Note', 'Törölve');
INSERT INTO `Translations` (`Key`, `Language`, `Segment`, `Value`)
VALUES ('English', 'en-US', 'Language', 'English');
INSERT INTO `Translations` (`Key`, `Language`, `Segment`, `Value`)
VALUES ('English', 'hu-HU', 'Language', 'Angol');
INSERT INTO `Translations` (`Key`, `Language`, `Segment`, `Value`)
VALUES ('Hungarian', 'en-US', 'Language', 'Hungarian');
INSERT INTO `Translations` (`Key`, `Language`, `Segment`, `Value`)
VALUES ('Hungarian', 'hu-HU', 'Language', 'Magyar');
INSERT INTO `Translations` (`Key`, `Language`, `Segment`, `Value`)
VALUES ('MemberAdd', 'en-US', 'Group', 'Member is added');
INSERT INTO `Translations` (`Key`, `Language`, `Segment`, `Value`)
VALUES ('MemberAdd', 'hu-HU', 'Group', 'Tag hozzáadva');
INSERT INTO `Translations` (`Key`, `Language`, `Segment`, `Value`)
VALUES ('MemberEdit', 'en-US', 'Group', 'Member is edited');
INSERT INTO `Translations` (`Key`, `Language`, `Segment`, `Value`)
VALUES ('MemberEdit', 'hu-HU', 'Group', 'Tag szerkesztve');
INSERT INTO `Translations` (`Key`, `Language`, `Segment`, `Value`)
VALUES ('MemberRemove', 'en-US', 'Group', 'Member is removed');
INSERT INTO `Translations` (`Key`, `Language`, `Segment`, `Value`)
VALUES ('MemberRemove', 'hu-HU', 'Group', 'Tag törölve');
INSERT INTO `Translations` (`Key`, `Language`, `Segment`, `Value`)
VALUES ('Moderator', 'en-US', 'GroupRole', 'Moderator');
INSERT INTO `Translations` (`Key`, `Language`, `Segment`, `Value`)
VALUES ('Moderator', 'hu-HU', 'GroupRole', 'Moderátor');
INSERT INTO `Translations` (`Key`, `Language`, `Segment`, `Value`)
VALUES ('Moderator', 'en-US', 'Role', 'Moderator');
INSERT INTO `Translations` (`Key`, `Language`, `Segment`, `Value`)
VALUES ('Moderator', 'hu-HU', 'Role', 'Moderátor');
INSERT INTO `Translations` (`Key`, `Language`, `Segment`, `Value`)
VALUES ('NoteCreate', 'en-US', 'Group', 'Note is created');
INSERT INTO `Translations` (`Key`, `Language`, `Segment`, `Value`)
VALUES ('NoteCreate', 'hu-HU', 'Group', 'Jegyzet létrehozva');
INSERT INTO `Translations` (`Key`, `Language`, `Segment`, `Value`)
VALUES ('Open', 'en-US', 'Group', 'Opened');
INSERT INTO `Translations` (`Key`, `Language`, `Segment`, `Value`)
VALUES ('Open', 'hu-HU', 'Group', 'Csoport kinyitva');
INSERT INTO `Translations` (`Key`, `Language`, `Segment`, `Value`)
VALUES ('Publish', 'en-US', 'Note', 'Public status is changed');
INSERT INTO `Translations` (`Key`, `Language`, `Segment`, `Value`)
VALUES ('Publish', 'hu-HU', 'Note', 'Nyílvános státusz megváltoztatva');
INSERT INTO `Translations` (`Key`, `Language`, `Segment`, `Value`)
VALUES ('RoleCreate', 'en-US', 'Group', 'Role is created');
INSERT INTO `Translations` (`Key`, `Language`, `Segment`, `Value`)
VALUES ('RoleCreate', 'hu-HU', 'Group', 'Szerepkör létrehozva');
INSERT INTO `Translations` (`Key`, `Language`, `Segment`, `Value`)
VALUES ('RoleEdit', 'en-US', 'Group', 'Role is edited');
INSERT INTO `Translations` (`Key`, `Language`, `Segment`, `Value`)
VALUES ('RoleEdit', 'hu-HU', 'Group', 'Szerepkör szerkesztve');
INSERT INTO `Translations` (`Key`, `Language`, `Segment`, `Value`)
VALUES ('RoleRemove', 'en-US', 'Group', 'Role is removed');
INSERT INTO `Translations` (`Key`, `Language`, `Segment`, `Value`)
VALUES ('RoleRemove', 'hu-HU', 'Group', 'Szerepkör törölve');
INSERT INTO `Translations` (`Key`, `Language`, `Segment`, `Value`)
VALUES ('TagCreate', 'en-US', 'Group', 'Tag is created');
INSERT INTO `Translations` (`Key`, `Language`, `Segment`, `Value`)
VALUES ('TagCreate', 'hu-HU', 'Group', 'Címke létrehozva');
INSERT INTO `Translations` (`Key`, `Language`, `Segment`, `Value`)
VALUES ('TagEdit', 'en-US', 'Note', 'Tag(s) added or removed');
INSERT INTO `Translations` (`Key`, `Language`, `Segment`, `Value`)
VALUES ('TagEdit', 'hu-HU', 'Note', 'Címke/Címkék hozzáadva vagy törölve');
INSERT INTO `Translations` (`Key`, `Language`, `Segment`, `Value`)
VALUES ('TagEdite', 'en-US', 'Group', 'Tag is edited');
INSERT INTO `Translations` (`Key`, `Language`, `Segment`, `Value`)
VALUES ('TagEdite', 'hu-HU', 'Group', 'Címke szerkesztve');
INSERT INTO `Translations` (`Key`, `Language`, `Segment`, `Value`)
VALUES ('TagRemove', 'en-US', 'Group', 'Tag is removed');
INSERT INTO `Translations` (`Key`, `Language`, `Segment`, `Value`)
VALUES ('TagRemove', 'hu-HU', 'Group', 'Címke törölve');
INSERT INTO `Translations` (`Key`, `Language`, `Segment`, `Value`)
VALUES ('TitleEdit', 'en-US', 'Note', 'Title is edited');
INSERT INTO `Translations` (`Key`, `Language`, `Segment`, `Value`)
VALUES ('TitleEdit', 'hu-HU', 'Note', 'Cím szerkesztve');
INSERT INTO `Translations` (`Key`, `Language`, `Segment`, `Value`)
VALUES ('User', 'en-US', 'Role', 'User');
INSERT INTO `Translations` (`Key`, `Language`, `Segment`, `Value`)
VALUES ('User', 'hu-HU', 'Role', 'Felhasználó');

CREATE INDEX `IX_ActionLogs_PerformerId` ON `ActionLogs` (`PerformerId`);

CREATE INDEX `IX_AspNetRoleClaims_RoleId` ON `AspNetRoleClaims` (`RoleId`);

CREATE UNIQUE INDEX `RoleNameIndex` ON `AspNetRoles` (`NormalizedName`);

CREATE INDEX `IX_AspNetUserClaims_UserId` ON `AspNetUserClaims` (`UserId`);

CREATE INDEX `IX_AspNetUserLogins_UserId` ON `AspNetUserLogins` (`UserId`);

CREATE INDEX `IX_AspNetUserRoles_RoleId` ON `AspNetUserRoles` (`RoleId`);

CREATE INDEX `EmailIndex` ON `AspNetUsers` (`NormalizedEmail`);

CREATE UNIQUE INDEX `IX_AspNetUsers_Email` ON `AspNetUsers` (`Email`);

CREATE INDEX `IX_AspNetUsers_LanguageId` ON `AspNetUsers` (`LanguageId`);

CREATE UNIQUE INDEX `IX_AspNetUsers_UserName` ON `AspNetUsers` (`UserName`);

CREATE UNIQUE INDEX `UserNameIndex` ON `AspNetUsers` (`NormalizedUserName`);

CREATE INDEX `IX_EditorMembers_NoteId` ON `EditorMembers` (`NoteId`);

CREATE INDEX `IX_GroupMembers_AddedById` ON `GroupMembers` (`AddedById`);

CREATE INDEX `IX_GroupMembers_RoleId` ON `GroupMembers` (`RoleId`);

CREATE INDEX `IX_GroupMembers_UserId` ON `GroupMembers` (`UserId`);

CREATE INDEX `IX_Groups_OwnerId` ON `Groups` (`OwnerId`);

CREATE UNIQUE INDEX `IX_Languages_ShortName` ON `Languages` (`ShortName`);

CREATE INDEX `IX_Notes_CreatorId` ON `Notes` (`CreatorId`);

CREATE INDEX `IX_Notes_GroupId` ON `Notes` (`GroupId`);

CREATE INDEX `IX_Notes_LastUpdaterId` ON `Notes` (`LastUpdaterId`);

CREATE INDEX `IX_Notes_UserId` ON `Notes` (`UserId`);

CREATE INDEX `IX_NoteTags_TagId` ON `NoteTags` (`TagId`);

CREATE UNIQUE INDEX `IX_RefreshTokens_Token` ON `RefreshTokens` (`Token`);

CREATE INDEX `IX_RefreshTokens_UserId` ON `RefreshTokens` (`UserId`);

CREATE INDEX `IX_Tags_GroupId` ON `Tags` (`GroupId`);

CREATE INDEX `IX_Tags_ParentId` ON `Tags` (`ParentId`);

CREATE INDEX `IX_Tags_UserId` ON `Tags` (`UserId`);

INSERT INTO `__EFMigrationsHistory` (`MigrationId`, `ProductVersion`)
VALUES ('20220904130327_Init', '6.0.9');

COMMIT;

START TRANSACTION;

ALTER TABLE `GroupRoles` DROP COLUMN `GroupClose`;

INSERT INTO `__EFMigrationsHistory` (`MigrationId`, `ProductVersion`)
VALUES ('20220909193502_RemoveCloseRight', '6.0.9');

COMMIT;

START TRANSACTION;

ALTER TABLE `GroupRoles` DROP COLUMN `CreateNote`;

INSERT INTO `__EFMigrationsHistory` (`MigrationId`, `ProductVersion`)
VALUES ('20220912185943_RemoveNoteCreatRight', '6.0.9');

COMMIT;

START TRANSACTION;

ALTER TABLE `Notes` ADD `Archived` tinyint(1) NOT NULL DEFAULT FALSE;

INSERT INTO `Translations` (`Key`, `Language`, `Segment`, `Value`)
VALUES ('Archived', 'en-US', 'Note', 'Archived status is changed');

INSERT INTO `Translations` (`Key`, `Language`, `Segment`, `Value`)
VALUES ('Archived', 'hu-HU', 'Note', 'Archivált státusz megváltoztatva');

INSERT INTO `__EFMigrationsHistory` (`MigrationId`, `ProductVersion`)
VALUES ('20220915072726_NoteArchiveStatus', '6.0.9');

COMMIT;

START TRANSACTION;

ALTER TABLE `AspNetUsers` ADD `Theme` int NOT NULL DEFAULT 0;

INSERT INTO `Translations` (`Key`, `Language`, `Segment`, `Value`)
VALUES ('0', 'en-US', 'Theme', 'Light Theme');
INSERT INTO `Translations` (`Key`, `Language`, `Segment`, `Value`)
VALUES ('0', 'hu-HU', 'Theme', 'Világos Téma');
INSERT INTO `Translations` (`Key`, `Language`, `Segment`, `Value`)
VALUES ('1', 'en-US', 'Theme', 'Dark Theme');
INSERT INTO `Translations` (`Key`, `Language`, `Segment`, `Value`)
VALUES ('1', 'hu-HU', 'Theme', 'Sötét Téma');

INSERT INTO `__EFMigrationsHistory` (`MigrationId`, `ProductVersion`)
VALUES ('20220921201423_AddThemeStore', '6.0.9');

COMMIT;

START TRANSACTION;

CREATE TABLE `Post` (
    `Id` int NOT NULL AUTO_INCREMENT,
    `CreatorId` varchar(255) CHARACTER SET utf8mb4 NULL,
    `Creation` datetime(6) NOT NULL,
    `LastUpdaterId` varchar(255) CHARACTER SET utf8mb4 NULL,
    `LastUpdate` datetime(6) NOT NULL,
    `Content` longtext CHARACTER SET utf8mb4 NOT NULL,
    CONSTRAINT `PK_Post` PRIMARY KEY (`Id`),
    CONSTRAINT `FK_Post_AspNetUsers_CreatorId` FOREIGN KEY (`CreatorId`) REFERENCES `AspNetUsers` (`Id`) ON DELETE SET NULL,
    CONSTRAINT `FK_Post_AspNetUsers_LastUpdaterId` FOREIGN KEY (`LastUpdaterId`) REFERENCES `AspNetUsers` (`Id`) ON DELETE SET NULL
) CHARACTER SET=utf8mb4;

CREATE INDEX `IX_Post_CreatorId` ON `Post` (`CreatorId`);

CREATE INDEX `IX_Post_LastUpdaterId` ON `Post` (`LastUpdaterId`);

INSERT INTO `__EFMigrationsHistory` (`MigrationId`, `ProductVersion`)
VALUES ('20221007123816_AddPosts', '6.0.9');

COMMIT;

START TRANSACTION;

DROP PROCEDURE IF EXISTS `POMELO_BEFORE_DROP_PRIMARY_KEY`;
DELIMITER //
CREATE PROCEDURE `POMELO_BEFORE_DROP_PRIMARY_KEY`(IN `SCHEMA_NAME_ARGUMENT` VARCHAR(255), IN `TABLE_NAME_ARGUMENT` VARCHAR(255))
BEGIN
	DECLARE HAS_AUTO_INCREMENT_ID TINYINT(1);
	DECLARE PRIMARY_KEY_COLUMN_NAME VARCHAR(255);
	DECLARE PRIMARY_KEY_TYPE VARCHAR(255);
	DECLARE SQL_EXP VARCHAR(1000);
	SELECT COUNT(*)
		INTO HAS_AUTO_INCREMENT_ID
		FROM `information_schema`.`COLUMNS`
		WHERE `TABLE_SCHEMA` = (SELECT IFNULL(SCHEMA_NAME_ARGUMENT, SCHEMA()))
			AND `TABLE_NAME` = TABLE_NAME_ARGUMENT
			AND `Extra` = 'auto_increment'
			AND `COLUMN_KEY` = 'PRI'
			LIMIT 1;
	IF HAS_AUTO_INCREMENT_ID THEN
		SELECT `COLUMN_TYPE`
			INTO PRIMARY_KEY_TYPE
			FROM `information_schema`.`COLUMNS`
			WHERE `TABLE_SCHEMA` = (SELECT IFNULL(SCHEMA_NAME_ARGUMENT, SCHEMA()))
				AND `TABLE_NAME` = TABLE_NAME_ARGUMENT
				AND `COLUMN_KEY` = 'PRI'
			LIMIT 1;
		SELECT `COLUMN_NAME`
			INTO PRIMARY_KEY_COLUMN_NAME
			FROM `information_schema`.`COLUMNS`
			WHERE `TABLE_SCHEMA` = (SELECT IFNULL(SCHEMA_NAME_ARGUMENT, SCHEMA()))
				AND `TABLE_NAME` = TABLE_NAME_ARGUMENT
				AND `COLUMN_KEY` = 'PRI'
			LIMIT 1;
		SET SQL_EXP = CONCAT('ALTER TABLE `', (SELECT IFNULL(SCHEMA_NAME_ARGUMENT, SCHEMA())), '`.`', TABLE_NAME_ARGUMENT, '` MODIFY COLUMN `', PRIMARY_KEY_COLUMN_NAME, '` ', PRIMARY_KEY_TYPE, ' NOT NULL;');
		SET @SQL_EXP = SQL_EXP;
		PREPARE SQL_EXP_EXECUTE FROM @SQL_EXP;
		EXECUTE SQL_EXP_EXECUTE;
		DEALLOCATE PREPARE SQL_EXP_EXECUTE;
	END IF;
END //
DELIMITER ;

DROP PROCEDURE IF EXISTS `POMELO_AFTER_ADD_PRIMARY_KEY`;
DELIMITER //
CREATE PROCEDURE `POMELO_AFTER_ADD_PRIMARY_KEY`(IN `SCHEMA_NAME_ARGUMENT` VARCHAR(255), IN `TABLE_NAME_ARGUMENT` VARCHAR(255), IN `COLUMN_NAME_ARGUMENT` VARCHAR(255))
BEGIN
	DECLARE HAS_AUTO_INCREMENT_ID INT(11);
	DECLARE PRIMARY_KEY_COLUMN_NAME VARCHAR(255);
	DECLARE PRIMARY_KEY_TYPE VARCHAR(255);
	DECLARE SQL_EXP VARCHAR(1000);
	SELECT COUNT(*)
		INTO HAS_AUTO_INCREMENT_ID
		FROM `information_schema`.`COLUMNS`
		WHERE `TABLE_SCHEMA` = (SELECT IFNULL(SCHEMA_NAME_ARGUMENT, SCHEMA()))
			AND `TABLE_NAME` = TABLE_NAME_ARGUMENT
			AND `COLUMN_NAME` = COLUMN_NAME_ARGUMENT
			AND `COLUMN_TYPE` LIKE '%int%'
			AND `COLUMN_KEY` = 'PRI';
	IF HAS_AUTO_INCREMENT_ID THEN
		SELECT `COLUMN_TYPE`
			INTO PRIMARY_KEY_TYPE
			FROM `information_schema`.`COLUMNS`
			WHERE `TABLE_SCHEMA` = (SELECT IFNULL(SCHEMA_NAME_ARGUMENT, SCHEMA()))
				AND `TABLE_NAME` = TABLE_NAME_ARGUMENT
				AND `COLUMN_NAME` = COLUMN_NAME_ARGUMENT
				AND `COLUMN_TYPE` LIKE '%int%'
				AND `COLUMN_KEY` = 'PRI';
		SELECT `COLUMN_NAME`
			INTO PRIMARY_KEY_COLUMN_NAME
			FROM `information_schema`.`COLUMNS`
			WHERE `TABLE_SCHEMA` = (SELECT IFNULL(SCHEMA_NAME_ARGUMENT, SCHEMA()))
				AND `TABLE_NAME` = TABLE_NAME_ARGUMENT
				AND `COLUMN_NAME` = COLUMN_NAME_ARGUMENT
				AND `COLUMN_TYPE` LIKE '%int%'
				AND `COLUMN_KEY` = 'PRI';
		SET SQL_EXP = CONCAT('ALTER TABLE `', (SELECT IFNULL(SCHEMA_NAME_ARGUMENT, SCHEMA())), '`.`', TABLE_NAME_ARGUMENT, '` MODIFY COLUMN `', PRIMARY_KEY_COLUMN_NAME, '` ', PRIMARY_KEY_TYPE, ' NOT NULL AUTO_INCREMENT;');
		SET @SQL_EXP = SQL_EXP;
		PREPARE SQL_EXP_EXECUTE FROM @SQL_EXP;
		EXECUTE SQL_EXP_EXECUTE;
		DEALLOCATE PREPARE SQL_EXP_EXECUTE;
	END IF;
END //
DELIMITER ;

ALTER TABLE `Post` DROP FOREIGN KEY `FK_Post_AspNetUsers_CreatorId`;

ALTER TABLE `Post` DROP FOREIGN KEY `FK_Post_AspNetUsers_LastUpdaterId`;

CALL POMELO_BEFORE_DROP_PRIMARY_KEY(NULL, 'Post');
ALTER TABLE `Post` DROP PRIMARY KEY;

ALTER TABLE `Post` RENAME `Posts`;

ALTER TABLE `Posts` DROP INDEX `IX_Post_LastUpdaterId`;

CREATE INDEX `IX_Posts_LastUpdaterId` ON `Posts` (`LastUpdaterId`);

ALTER TABLE `Posts` DROP INDEX `IX_Post_CreatorId`;

CREATE INDEX `IX_Posts_CreatorId` ON `Posts` (`CreatorId`);

ALTER TABLE `Posts` ADD CONSTRAINT `PK_Posts` PRIMARY KEY (`Id`);
CALL POMELO_AFTER_ADD_PRIMARY_KEY(NULL, 'Posts', 'Id');

ALTER TABLE `Posts` ADD CONSTRAINT `FK_Posts_AspNetUsers_CreatorId` FOREIGN KEY (`CreatorId`) REFERENCES `AspNetUsers` (`Id`) ON DELETE SET NULL;

ALTER TABLE `Posts` ADD CONSTRAINT `FK_Posts_AspNetUsers_LastUpdaterId` FOREIGN KEY (`LastUpdaterId`) REFERENCES `AspNetUsers` (`Id`) ON DELETE SET NULL;

INSERT INTO `__EFMigrationsHistory` (`MigrationId`, `ProductVersion`)
VALUES ('20221007172339_AddPostTableRename', '6.0.9');

DROP PROCEDURE `POMELO_BEFORE_DROP_PRIMARY_KEY`;

DROP PROCEDURE `POMELO_AFTER_ADD_PRIMARY_KEY`;

COMMIT;

