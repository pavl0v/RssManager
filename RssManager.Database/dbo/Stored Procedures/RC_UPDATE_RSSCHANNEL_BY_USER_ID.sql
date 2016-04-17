CREATE PROCEDURE [dbo].[RC_UPDATE_RSSCHANNEL_BY_USER_ID]
	@channel_id bigint = 0,
	@user_id bigint = 0,
	@name nvarchar(25)
AS
BEGIN
	UPDATE [dbo].[RSSCHANNEL_BY_USER]
	SET [NAME]=@name
	WHERE [CHANNEL_ID]=@channel_id AND [USER_ID]=@user_id
END
