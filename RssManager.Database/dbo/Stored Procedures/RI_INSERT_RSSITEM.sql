CREATE PROCEDURE [dbo].[RI_INSERT_RSSITEM]
	@channel_id bigint,
	@author nvarchar(50),
	@category nvarchar(50),
	@comments nvarchar(200),
	@description nvarchar(2000),
	@enclosure nvarchar(50),
	@guid nvarchar(400),
	@link nvarchar(400),
	@pub_date nvarchar(100),
	@pub_date_time datetime,
	@source nvarchar(50),
	@title nvarchar(200)
AS
BEGIN
	DECLARE @item_id bigint;
	IF NOT EXISTS(SELECT [ID] FROM [dbo].[RSSITEM] WHERE [GUID] = @guid)
	BEGIN
		INSERT INTO [dbo].[RSSITEM] (
			 [CHANNEL_ID]
			,[AUTHOR]
			,[CATEGORY]
			,[COMMENTS]
			,[DESCRIPTION]
			,[ENCLOSURE]
			,[GUID]
			,[LINK]
			,[PUB_DATE]
			,[PUB_DATE_TIME]
			,[SOURCE]
			,[TITLE])
		VALUES (
			 @channel_id
			,@author
			,@category
			,@comments
			,@description
			,@enclosure
			,@guid
			,@link
			,@pub_date
			,@pub_date_time
			,@source
			,@title);
		SET @item_id = (SELECT SCOPE_IDENTITY());
	END
	ELSE
	BEGIN
		SET @item_id = (SELECT [ID] FROM [dbo].[RSSITEM] WHERE [GUID] = @guid);
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
			[ID]=@item_id;
	END
	SELECT @item_id;
END