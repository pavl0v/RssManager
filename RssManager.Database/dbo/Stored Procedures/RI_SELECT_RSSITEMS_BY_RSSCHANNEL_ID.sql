CREATE PROCEDURE [dbo].[RI_SELECT_RSSITEMS_BY_RSSCHANNEL_ID]
	@channel_id bigint = 0
AS
BEGIN
	SELECT 
		 [dbo].[RSSITEM].[AUTHOR]
		,[dbo].[RSSITEM].[CATEGORY]
		,[dbo].[RSSITEM].[CHANNEL_ID]
		,[dbo].[RSSITEM].[COMMENTS]
		,[dbo].[RSSITEM].[DESCRIPTION]
		,[dbo].[RSSITEM].[ENCLOSURE]
		,[dbo].[RSSITEM].[GUID]
		,[dbo].[RSSITEM].[ID]
		,[dbo].[RSSITEM].[LINK]
		,[dbo].[RSSITEM].[PUB_DATE]
		,[dbo].[RSSITEM].[PUB_DATE_TIME]
		,[dbo].[RSSITEM].[SOURCE]
		,[dbo].[RSSITEM].[TITLE]
	FROM
		[dbo].[RSSITEM] 
	WHERE
		[dbo].[RSSITEM].[CHANNEL_ID]=@channel_id
	ORDER BY
		[PUB_DATE_TIME] DESC;
END
