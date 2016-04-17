CREATE USER [rssmanager]
	FOR LOGIN [rssmanager]
	WITH DEFAULT_SCHEMA = dbo

GO

GRANT CONNECT TO [rssmanager]
GO

exec sp_addrolemember 'db_owner', [rssmanager]
GO
