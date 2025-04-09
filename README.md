# ecw_ProjectAlpha

## Configuration
### appsettings.json:
```
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
```
