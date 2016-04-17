CREATE PROCEDURE [dbo].[USR_INSERT_USER]
	@first_name nvarchar(50),
	@last_name nvarchar(50),
	@password nvarchar(50),
	@user_name nvarchar(50),
	@guid char(32)
AS
BEGIN
	INSERT INTO [dbo].[USER](
		 [FIRST_NAME]
		,[LAST_NAME]
		,[PASSWORD]
		,[USER_NAME]
		,[GUID]
	)
	VALUES (
		 @first_name
		,@last_name
		,@password
		,@user_name
		,@guid
	);

	SELECT SCOPE_IDENTITY() AS 'SCOPE_IDENTITY';
END
