CREATE PROCEDURE [dbo].[RI_SET_READ_STATE]
	@item_id bigint = 0,
	@user_id bigint = 0,
	@state smallint = 0
AS
BEGIN
	IF EXISTS(SELECT * FROM [dbo].[RSSITEM_BY_USER] WHERE [USER_ID]=@user_id AND [ITEM_ID]=@item_id)
	BEGIN
		UPDATE [dbo].[RSSITEM_BY_USER]
		SET [READ_STATE]=@state
		WHERE [ITEM_ID]=@item_id AND [USER_ID]=@user_id
	END
	ELSE
	BEGIN
		INSERT INTO [dbo].[RSSITEM_BY_USER] ([ITEM_ID], [USER_ID], [READ_STATE])
		VALUES (@item_id, @user_id, @state)
	END
END
