# OszkApi
OSzK API is an open-source ASP.NET Core Web API for the National Széchenyi Library of Hungary.
It provides a standard REST api endpoint to query Books, AudioBooks and other publication.

## Usage
```powershell
# Restore and Run project
dotnet restore
dotnet run --project ./src/OszkApi

# Test API endpoints
curl 'http://localhost:5000/api/audiobooks'
curl 'http://localhost:5000/api/audiobooks?query=gardonyi'
```

### Hosting
The project can be hosted both in `docker` containers and in Azure App Services.
Publicly accessible DEV endpoint in Azure is:
 - http://oszkapi-dev.azurewebsites.net/api/audiobooks


