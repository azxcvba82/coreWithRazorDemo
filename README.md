# Welcome to Demo!

This demo split to three parts: API server, Classic MVC page, and SPA page

## API server
** prerequisite: prepare MS SQL database with specified schema in appendix.
Firstly, Run API server by open solution Visual Studio, set WebApplicationDemo as default project.
Check ConnectionStrings.demoDatabase, JWT config in appsettings.json are setup correctly.
Start application as IIS Express mode, and test all API by Swagger page.
> Note-1: server link like: https://localhost:0000/swagger/index.html
> Note-2: some API endpoint require JWT authentication with format Bearer `token`.

## Classic MVC page

Second, Run Classic MVC page by setting MVCRazor as default project.
Start application as IIS Express mode, and test Register & Login by the following step:
Register Step:
Enter url in browser: https://localhost:0000/account/register
Fill user name, password, and confirm
Makesure password more than four words, and click Register button.
Login Step:
Enter url in browser: https://localhost:0000/account/login or the default page in browser start.
Fill user name and password
Makesure user name and password correctly, and click Login button.
If password correct, page will redirect to dashboard.
> Note: All register user data are not store in memory, and it will disappear after app closed.

## SPA page

In this demo, we have to run API Server and SAP page application at the same time by following procedure:
Run API server by setting WebApplicationDemo as default project.
Start application as IIS Express mode, and check browser successfully open.
Then, Open another Visual Studio with same solution and set SPAWebAssembly as default project.
Check variable BaseAddress in Program.cs is point to correct API server.
`new HttpClient { BaseAddress = new Uri("https://localhost:0000/") });`
Start application as IIS Express mode, and test Register & Login by the following step:
Register Step:
Enter url in browser: https://localhost:0000/register
Fill user name, password, and confirm
Makesure password more than four words, and click Register button.
Login Step:
Enter url in browser: https://localhost:0000/login
Fill user name and password
Makesure user name and password correctly, and click Login button.
If password correct, page will redirect to dashboard with user profile.

## Appendix

SQL schema

    CREATE TABLE [dbo].[tUser](
    	[username] [nvarchar](100) NOT NULL,
    	[password] [nvarchar](400) NOT NULL,
     CONSTRAINT [PK_tUser] PRIMARY KEY CLUSTERED 
    ( [username] ASC ) WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
    ) ON [PRIMARY]

