﻿CREATE PROCEDURE [dbo].[USR_SELECT_USER_BY_USER_NAME]
	@username nvarchar(50)
AS
BEGIN
	SELECT 
		 [ID]
		,[USER_NAME]
		,[PASSWORD]
		,[FIRST_NAME]
		,[LAST_NAME]
		,[GUID]
	FROM
		[dbo].[USER]
	WHERE
		[USER_NAME]=@username;
END
