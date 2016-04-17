CREATE PROCEDURE RC_UPDATE_RSSCHANNEL 
	-- Add the parameters for the stored procedure here
	@id bigint,
	@copyright nvarchar(50),
	@description nvarchar(50),
	@language nvarchar(10),
	@title nvarchar(50), 
	@url nvarchar(100),
	@user_id bigint
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	UPDATE [dbo].[RSSCHANNEL] 
	SET
		 [COPYRIGHT]=@copyright
		,[DESCRIPTION]=@description
		,[LANGUAGE]=@language
		,[TITLE]=@title
		,[URL]=@url
	WHERE 
		[ID]=@id;
END