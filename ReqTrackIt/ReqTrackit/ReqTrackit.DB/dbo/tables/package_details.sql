CREATE TABLE [dbo].[package_details]
(
	[Id] INT IDENTITY(1000,1) NOT NULL PRIMARY KEY, 
    [Name] NVARCHAR(50) NOT NULL, 
    [Version] NVARCHAR(50) NOT NULL, 
    [Date_modified] DATETIME NULL, 
    [Description] NVARCHAR(200) NULL
)
