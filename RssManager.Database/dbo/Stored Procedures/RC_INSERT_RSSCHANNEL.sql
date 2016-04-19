
CREATE PROCEDURE RC_INSERT_RSSCHANNEL 
	-- Add the parameters for the stored procedure here
	@user_id bigint,
	@copyright nvarchar(50),
	@description nvarchar(50),
	@language nvarchar(10),
	@name nvarchar(25),
	@title nvarchar(50), 
	@url nvarchar(100),
	@autorefresh bit = 0
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	DECLARE @channel_id bigint;
	IF NOT EXISTS(SELECT [ID] FROM [dbo].[RSSCHANNEL] WHERE [URL] = @url)
	BEGIN
		INSERT INTO [dbo].[RSSCHANNEL] (
			 [COPYRIGHT] 
			,[DESCRIPTION] 
			,[LANGUAGE] 
			,[TITLE] 
			,[URL]
			,[AUTOREFRESH]
			,[IS_DELETED])
		VALUES (
			 @copyright 
			,@description 
			,@language 
			,@title 
			,@url
			,@autorefresh
			,0);
		SET @channel_id = (SELECT SCOPE_IDENTITY());
	END
	ELSE
	BEGIN
		SET @channel_id = (SELECT [ID] FROM [dbo].[RSSCHANNEL] WHERE [URL] = @url);
	END
	INSERT INTO [dbo].[RSSCHANNEL_BY_USER] ([CHANNEL_ID], [USER_ID], [NAME]) VALUES (@channel_id, @user_id, @name);
	SELECT @channel_id;
END