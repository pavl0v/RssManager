# RssManager

1. Restore necessary NuGet packages

2. Deploy RssManager.Database project on any SQL Server instance

3. Modify "connectionStrings" section in Web.config of RssManager.WebAPI project

4. Modify Scripts/app/module-common.js (getApiBaseUrl() function) if deploy RssManager.WebAPI

5. Set multiple startup projects in solution properties:

  5.1. RssManager.WebAPI: Start

  5.2. RssManager.WebApp: Start without debugging

6. Check your firewall settings if needed

7. Start solution

Description
-
Token based authentication/authorization implemented in the project. Create new account using 'Sign Up' function before first login.
Default iterval to update RSS channels is equal to 5 minutes and can be changed in Web.config of RssManager.WebAPI project.
Notification on RSS channels have new updates is implemented based on SignalR.

Keywords
-
ASP.NET WebAPI, OWIN, Ninject, ADO.NET, SignalR, AngularJS
