CREATE PROCEDURE [dbo].[RC_DELETE_RSSCHANNEL_BY_USER_ID]
	@channel_id bigint = 0,
	@user_id bigint = 0
AS
BEGIN
	DELETE FROM [dbo].[RSSCHANNEL_BY_USER] 
	WHERE [CHANNEL_ID]=@channel_id AND [USER_ID]=@user_id;
END
