﻿CREATE TABLE [dbo].[USER]
(
	[ID] BIGINT NOT NULL IDENTITY, 
    [USER_NAME] NVARCHAR(50) NOT NULL, 
    [PASSWORD] NVARCHAR(50) NOT NULL, 
    [FIRST_NAME] NVARCHAR(50) NOT NULL, 
    [LAST_NAME] NVARCHAR(50) NOT NULL,
	[GUID] CHAR(32) NULL, 
    CONSTRAINT [PK_USER_ID] PRIMARY KEY CLUSTERED ([ID] ASC)
);

GO

CREATE UNIQUE INDEX [IX_USER_USER_NAME] ON [dbo].[USER] ([USER_NAME])