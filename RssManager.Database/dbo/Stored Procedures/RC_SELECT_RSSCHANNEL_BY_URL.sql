﻿CREATE PROCEDURE [dbo].[RC_SELECT_RSSCHANNEL_BY_URL]
	@url nvarchar(100) = ''
AS
BEGIN
	SELECT 
		 [dbo].[RSSCHANNEL].[COPYRIGHT]
		,[dbo].[RSSCHANNEL].[DESCRIPTION]
		,[dbo].[RSSCHANNEL].[ID]
		,[dbo].[RSSCHANNEL].[LANGUAGE]
		,[dbo].[RSSCHANNEL].[TITLE]
		,[dbo].[RSSCHANNEL].[URL]
	FROM
		[dbo].[RSSCHANNEL]
	WHERE 
		[URL]=@url;
END
