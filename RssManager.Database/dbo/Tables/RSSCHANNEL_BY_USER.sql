﻿CREATE TABLE [dbo].[RSSCHANNEL_BY_USER]
(
	[CHANNEL_ID] BIGINT NOT NULL, 
    [USER_ID] BIGINT NOT NULL, 
    [NAME] NVARCHAR(25) NULL DEFAULT 'RSS-CHANNEL-NAME', 
    CONSTRAINT [PK_CHANNEL_BY_USER] PRIMARY KEY ([CHANNEL_ID], [USER_ID]) ,
	CONSTRAINT [FK_RSSCHANNEL_BY_USER_CHN] FOREIGN KEY ([CHANNEL_ID]) REFERENCES [RSSCHANNEL]([ID]) ON DELETE CASCADE,
	CONSTRAINT [FK_RSSCHANNEL_BY_USER_USR] FOREIGN KEY ([USER_ID]) REFERENCES [USER]([ID]) ON DELETE CASCADE
)