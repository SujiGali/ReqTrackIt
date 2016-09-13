CREATE TABLE [dbo].[nuget_request_tracker]
(
	[Id] NVARCHAR(50) NOT NULL PRIMARY KEY,
    [PackageID] INT NOT NULL, 
	[TFS_BranchName] NVARCHAR(50) NOT NULL,
    [TFS_changeset] NVARCHAR(50) NOT NULL, 
    [Version_requested] NVARCHAR(50) NOT NULL, 
    [Description] NVARCHAR(200) NULL, 
    CONSTRAINT [FK_nuget_request_tracker_package_details] FOREIGN KEY ([PackageID]) REFERENCES [package_details]([Id])


)
