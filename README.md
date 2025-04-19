# Management User Web API

### Introduction
Management User Web API is a web-based application API designed to enable system administrators to manage user accounts, roles, and permissions.

### Management User Web API Features
* Users can signup and login to their accounts
* Public (non-authenticated) users can not access to the API
* Authenticated users interact with the API based on their assigned permissions
* For example, administrators can create, update, and delete user details, roles, and permissions

### Prerequisites
* [.NET 8.0](https://dotnet.microsoft.com/en-us/download)
* [Visual Studio](https://visualstudio.microsoft.com/downloads/)
* [SQL Server](https://www.microsoft.com/en-us/sql-server/sql-server-downloads)
* [SQL Server Management Studio](https://learn.microsoft.com/en-us/ssms/download-sql-server-management-studio-ssms)

  
### Installation Guide
* Clone this repository [here](https://github.com/AlabicaCoff/management-user-api.git)
* Open the solution file (ManagementUser.API.sln) using Visual Studio or Visual Studio Code
* Run ```dotnet restore``` to install NuGet packages and other dependencies
* Use the default SQL Server setup, or Configure your preferred database in appsettings.json
  
### Usage
* Run ```dotnet run``` to start the application
* Connect to the API using Postman or Swagger on port 7263
* Use the email ``SuperAdmin@ManagementUser.com`` and the password ``SuperAdmin@123`` to log in as the default super administrator of the system.

### API Endpoints
| HTTP Verbs | Endpoints | Action |
| --- | --- | --- |
| POST | /api/auth/login | To login an existing user account and get the token and roles |
| GET | /api/users/DataTable | To retrieve all existing users details, also can sorting, filtering and pagination |
| GET | /api/users/:id | To retrieve details of a single user |
| POST | /api/users | To create a new user |
| PUT | /api/users/:id | To update user details, roles and permissions |
| DELETE | /api/users/:id | To delete user |
| GET | /api/users/get-id | To generate new user id for creating user later |
| GET | /api/roles | To retrieve all roles that administrators can assign to any user later |
| GET | /api/permissions | To retrieve all permissions same as roles |

### Technologies Used
* [C#](https://learn.microsoft.com/en-us/dotnet/csharp/)
* [ASP.NET WEB API](https://dotnet.microsoft.com/en-us/apps/aspnet/apis)
* [SQL Server](https://www.microsoft.com/en-us/sql-server/)

### Authors
* [AlabicaCoff](https://github.com/AlabicaCoff)
