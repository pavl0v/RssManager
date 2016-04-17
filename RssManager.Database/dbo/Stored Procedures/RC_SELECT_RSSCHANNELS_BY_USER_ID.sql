﻿CREATE PROCEDURE [dbo].[RC_SELECT_RSSCHANNELS_BY_USER_ID]
	@user_id bigint
AS
BEGIN
	SELECT 
		 [dbo].[RSSCHANNEL_BY_USER].[NAME]
		,[dbo].[RSSCHANNEL].[COPYRIGHT]
		,[dbo].[RSSCHANNEL].[DESCRIPTION]
		,[dbo].[RSSCHANNEL].[ID]
		,[dbo].[RSSCHANNEL].[LANGUAGE]
		,[dbo].[RSSCHANNEL].[TITLE]
		,[dbo].[RSSCHANNEL].[URL]
		--,[dbo].[RSSCHANNEL].[USER_ID]
	FROM
		[dbo].[RSSCHANNEL] INNER JOIN [dbo].[RSSCHANNEL_BY_USER]
		ON [dbo].[RSSCHANNEL_BY_USER].[CHANNEL_ID]=[dbo].[RSSCHANNEL].[ID]
	WHERE
		--[dbo].[RSSCHANNEL].[USER_ID]=@user_id
		[dbo].[RSSCHANNEL_BY_USER].[USER_ID]=@user_id
	ORDER BY
		[dbo].[RSSCHANNEL].[ID]
END
