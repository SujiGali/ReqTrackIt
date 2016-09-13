﻿/*
Deployment script for reqtrackit-webdb

This code was generated by a tool.
Changes to this file may cause incorrect behavior and will be lost if
the code is regenerated.
*/

GO
SET ANSI_NULLS, ANSI_PADDING, ANSI_WARNINGS, ARITHABORT, CONCAT_NULL_YIELDS_NULL, QUOTED_IDENTIFIER ON;

SET NUMERIC_ROUNDABORT OFF;


GO
:setvar DatabaseName "reqtrackit-webdb"
:setvar DefaultFilePrefix "reqtrackit-webdb"
:setvar DefaultDataPath ""
:setvar DefaultLogPath ""

GO
:on error exit
GO
/*
Detect SQLCMD mode and disable script execution if SQLCMD mode is not supported.
To re-enable the script after enabling SQLCMD mode, execute the following:
SET NOEXEC OFF; 
*/
:setvar __IsSqlCmdEnabled "True"
GO
IF N'$(__IsSqlCmdEnabled)' NOT LIKE N'True'
    BEGIN
        PRINT N'SQLCMD mode must be enabled to successfully execute this script.';
        SET NOEXEC ON;
    END


GO
/*
The column [dbo].[nuget_request_tracker].[TFS_BranchName] on table [dbo].[nuget_request_tracker] must be added, but the column has no default value and does not allow NULL values. If the table contains data, the ALTER script will not work. To avoid this issue you must either: add a default value to the column, mark it as allowing NULL values, or enable the generation of smart-defaults as a deployment option.
*/

IF EXISTS (select top 1 1 from [dbo].[nuget_request_tracker])
    RAISERROR (N'Rows were detected. The schema update is terminating because data loss might occur.', 16, 127) WITH NOWAIT

GO
PRINT N'Dropping [dbo].[FK_nuget_request_tracker_package_details]...';


GO
ALTER TABLE [dbo].[nuget_request_tracker] DROP CONSTRAINT [FK_nuget_request_tracker_package_details];


GO
PRINT N'Dropping [dbo].[FK_request_tracker_nuget_request_tracker]...';


GO
ALTER TABLE [dbo].[request_tracker] DROP CONSTRAINT [FK_request_tracker_nuget_request_tracker];


GO
PRINT N'Starting rebuilding table [dbo].[nuget_request_tracker]...';


GO
BEGIN TRANSACTION;

SET TRANSACTION ISOLATION LEVEL SERIALIZABLE;

SET XACT_ABORT ON;

CREATE TABLE [dbo].[tmp_ms_xx_nuget_request_tracker] (
    [Id]                NVARCHAR (50)  NOT NULL,
    [PackageID]         INT            NOT NULL,
    [TFS_BranchName]    NVARCHAR (50)  NOT NULL,
    [TFS_changeset]     NVARCHAR (50)  NOT NULL,
    [Version_requested] NVARCHAR (50)  NOT NULL,
    [Description]       NVARCHAR (200) NULL,
    PRIMARY KEY CLUSTERED ([Id] ASC)
);

IF EXISTS (SELECT TOP 1 1 
           FROM   [dbo].[nuget_request_tracker])
    BEGIN
        INSERT INTO [dbo].[tmp_ms_xx_nuget_request_tracker] ([Id], [PackageID], [TFS_changeset], [Version_requested], [Description])
        SELECT   [Id],
                 [PackageID],
                 [TFS_changeset],
                 [Version_requested],
                 [Description]
        FROM     [dbo].[nuget_request_tracker]
        ORDER BY [Id] ASC;
    END

DROP TABLE [dbo].[nuget_request_tracker];

EXECUTE sp_rename N'[dbo].[tmp_ms_xx_nuget_request_tracker]', N'nuget_request_tracker';

COMMIT TRANSACTION;

SET TRANSACTION ISOLATION LEVEL READ COMMITTED;


GO
PRINT N'Creating [dbo].[FK_nuget_request_tracker_package_details]...';


GO
ALTER TABLE [dbo].[nuget_request_tracker] WITH NOCHECK
    ADD CONSTRAINT [FK_nuget_request_tracker_package_details] FOREIGN KEY ([PackageID]) REFERENCES [dbo].[package_details] ([Id]);


GO
PRINT N'Creating [dbo].[FK_request_tracker_nuget_request_tracker]...';


GO
ALTER TABLE [dbo].[request_tracker] WITH NOCHECK
    ADD CONSTRAINT [FK_request_tracker_nuget_request_tracker] FOREIGN KEY ([DetailsID]) REFERENCES [dbo].[nuget_request_tracker] ([Id]);


GO
