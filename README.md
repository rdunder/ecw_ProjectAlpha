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

2. **Configure Appsettings**
   */src/Ui.Asp.Mv/appsettings.json*
     ```JSON
      {
        "ConnectionStrings": {
          "LocalDb": "<Database Connectionstring>"
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

  */src/Data/appsettings.json*    
    ```JSON
    
      {
        "ConnectionStrings": {
          "LocalDb": "<Database Connectionstring>"
        }
      }

3. **Run Database Migrations**
    ```bash
    dotnet ef database update

4. **Run the Application**
     ```bash
     dotnet ef database update
    

