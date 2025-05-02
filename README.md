# ProjectAlpha

**ProjectAlpha** is a role-based project management application built with **ASP.NET Core MVC** and **SQL Server**. It provides functionality for managing projects, users, and customers, with a clear hierarchy of roles and permissions. The application also supports authentication via GitHub and Google, and includes a built-in mail service.

## ✨ Features

- 🔐 **User Authentication**
  - Register and log in with email
  - Social login via **GitHub** and **Google**

- 🗂️ **Project Management**
  - Create, update, and delete projects
  - Assign users to projects

- 👥 **User Management**
  - Admins and can add users
  - Role-based access control

- 🧑‍💼 **Customer Management**
  - Manage customers and link them to projects

- 📛 **Job Title Management**
  - Admins can define and manage job titles

- 📬 **Mail Service**
  - Confirmations and Password Reset via Email

## 🛡️ Roles

The application uses **statically defined roles** to control access:

- `Viewer`: Read-only access to projects
- `User`: Basic project management
- `Manager`: Project and customer management privileges
- `Administrator`: Full access, including system configuration and job title management

## 🛠️ Tech Stack

- **Backend** / **Frontent**: ASP.NET Core MVC
- **Database**: SQL Server
- **Authentication**: ASP.NET Identity + External Providers (GitHub, Google)
- **Mail Service**: Built-in email system for notifications and account actions

## 🚀 Getting Started

1. **Clone the repository**
   ```bash
   git clone https://github.com/rdunder/ecw_ProjectAlpha.git
   cd ProjectAlpha


> ⚠️ **A Note About appsettings.json** 
> Never store sensitive information such as passwords, API keys, or connection strings in plain text in `appsettings.json`, especially in production environments or source control.  
>  
> **Recommended practices:**  
> - Use [User Secrets](https://learn.microsoft.com/en-us/aspnet/core/security/app-secrets) for local development.  
> - Use environment variables or a secure secrets manager (e.g., Azure Key Vault) in production.  
> - Add `appsettings.*.json` to `.gitignore` if it contains sensitive values.



2. **Configure Appsettings**

   */src/Ui.Asp.Mv/appsettings.json*
   *(Admin Password must meet the requirements: min 8 characters, min one digit, min one uppercase letter, min one special character)*
   ```json
    {
      "ConnectionStrings": {
        "LocalDb": "<Database Connectionstring>"
      },

     "DefaultAdmin": {
        "Email": "<Email>",
        "Password": "<Password>" 
      },
    
      "Authentication": {
        "Google": {
          "ClientId": "<Client ID>",
          "ClientSecret": "<Client Secret>"
        },
        "Github": {
          "ClientId": "<Client ID",
          "ClientSecret": "Client Secret"
        }
      },
      
      "EmailProvider": {
        "Address": "<Address to Mail Service",
        "Port": <Port as number>,
        "ApiKey": "<ApiKey>",
        "Secret": "<Secret>",
        "SenderEmail": "<Sender Email>"
      }
    }
   ```

    */src/Data/appsettings.json*    
    ```json
    {
      "ConnectionStrings": {
        "LocalDb": "<Database Connectionstring>"
      }
    }
    ```
  

4. **Run Database Migrations**
    ```bash
    dotnet ef database update

5. **Run the Application**
     ```bash
     dotnet ef database update
  
  *When the application starts first time with clean database, a default admin will be created*
  *The credentials will be taken from appsettings.json*





