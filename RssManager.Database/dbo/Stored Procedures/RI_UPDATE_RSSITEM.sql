CREATE PROCEDURE [dbo].[RI_UPDATE_RSSITEM]
	@id bigint,
	@channel_id bigint,
	@author nvarchar(50),
	@category nvarchar(50),
	@comments nvarchar(200),
	@description nvarchar(500),
	@enclosure nvarchar(50),
	@guid nvarchar(400),
	@link nvarchar(400),
	@pub_date nvarchar(100),
	@pub_date_time datetime,
	@source nvarchar(50),
	@title nvarchar(200)
AS
BEGIN
	UPDATE [dbo].[RSSITEM]
	SET
		 [AUTHOR]=@author
		,[CATEGORY]=@category
		,[CHANNEL_ID]=@channel_id
		,[COMMENTS]=@comments
		,[DESCRIPTION]=@description
		,[ENCLOSURE]=@enclosure
		,[GUID]=@guid
		,[LINK]=@link
		,[PUB_DATE]=@pub_date
		,[PUB_DATE_TIME]=@pub_date_time
		,[SOURCE]=@source
		,[TITLE]=@title
	WHERE
		[ID]=@id;
END
