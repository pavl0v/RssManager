﻿CREATE PROCEDURE [dbo].[RI_DELETE_RSSITEM]
	@id bigint = 0
AS
BEGIN
	DELETE FROM [dbo].[RSSITEM] WHERE [dbo].[RSSITEM].[ID]=@id;
END
