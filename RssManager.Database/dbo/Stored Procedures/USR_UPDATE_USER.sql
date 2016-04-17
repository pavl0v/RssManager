CREATE PROCEDURE [dbo].[USR_UPDATE_USER]
	@id bigint = 0,
	@first_name nvarchar(50) = '',
	@last_name nvarchar(50) = '',
	@password nvarchar(50) = '',
	@user_name nvarchar(50) = ''
AS
BEGIN
	UPDATE [dbo].[USER]
	SET
		[dbo].[USER].[FIRST_NAME]=@first_name
		,[dbo].[USER].[LAST_NAME]=@last_name
		,[dbo].[USER].[PASSWORD]=@password
		,[dbo].[USER].[USER_NAME]=@user_name
	WHERE
		[dbo].[USER].[ID]=@id;
END