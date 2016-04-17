CREATE PROCEDURE [dbo].[RI_SELECT_RSSITEMS_BY_RSSCHANNEL_ID_PAGE]
	@channel_id bigint = 0,
	@page_no int = 1,
	@page_size int = 1,
	@user_id bigint = 0
AS
BEGIN
	SELECT 
		 q.[AUTHOR]
		,q.[CATEGORY]
		,q.[CHANNEL_ID]
		,q.[COMMENTS]
		,q.[DESCRIPTION]
		,q.[ENCLOSURE]
		,q.[GUID]
		,q.[ID]
		,q.[LINK]
		,q.[PUB_DATE]
		,q.[PUB_DATE_TIME]
		,q.[SOURCE]
		,q.[TITLE]
		,q.[READ_STATE]
	FROM
		(SELECT [dbo].[RSSITEM].*, TT.[READ_STATE], ROW_NUMBER() OVER (ORDER BY [dbo].[RSSITEM].[PUB_DATE_TIME] DESC) rn
        FROM 
			[dbo].[RSSITEM] LEFT JOIN 
				(SELECT * FROM [dbo].[RSSITEM_BY_USER] WHERE [USER_ID]=@user_id) TT
			ON [dbo].[RSSITEM].[ID]=TT.[ITEM_ID]
		WHERE [dbo].[RSSITEM].[CHANNEL_ID]=@channel_id) q
	WHERE
		rn BETWEEN ((@page_no - 1) * @page_size + 1) AND ((@page_no - 1) * @page_size + @page_size)
	ORDER BY
		[PUB_DATE_TIME] DESC;
END
