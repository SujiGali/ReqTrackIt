CREATE TABLE [dbo].[request_tracker]
(
	[Id] INT IDENTITY(4000,1) NOT NULL PRIMARY KEY, 
    [TypeID] INT NOT NULL, 
    [StatusID] INT NOT NULL, 
    [DetailsID] NVARCHAR(50) NOT NULL UNIQUE, 
    [UserID] VARCHAR(50) NOT NULL, 
    [Date_requested] DATETIME NOT NULL, 
    [Date_completed] DATETIME NULL, 
    CONSTRAINT [FK_request_tracker_request_type] FOREIGN KEY ([TypeID]) REFERENCES [request_type]([Id]), 
    CONSTRAINT [FK_request_tracker_request_status] FOREIGN KEY ([StatusID]) REFERENCES [request_status]([Id]),
	CONSTRAINT [FK_request_tracker_nuget_request_tracker] FOREIGN KEY ([DetailsID]) REFERENCES [nuget_request_tracker](Id)
)
