# DnD Solution 🏰⚔️

DnDSolution is a .NET project consisting of a Web API and background worker service to synchronize spell data from the D&D 5e API. This project uses modern technologies to provide a robust solution for D&D enthusiasts and developers.

## Project Structure 📂
![image](https://github.com/user-attachments/assets/50bec3c8-8710-47da-8dbd-b18752f225f7)

## Technologies Used 🛠️

- .NET 9
- Entity Framework Core 9.0.3
- Hangfire 1.8.18
- SQL Server
- Docker
- Swagger (API documentation)

## Development Environment Setup ⚙️

### Prerequisites

- [.NET 9 SDK](https://dotnet.microsoft.com/download/dotnet/9.0)
- [Docker](https://www.docker.com/get-started)
- [SQL Server](https://www.microsoft.com/en-us/sql-server/sql-server-downloads) (or use Docker)

### Database Setup 🗃️

```bash
docker run -e 'ACCEPT_EULA=Y' -e 'SA_PASSWORD=Your_password123' \
  -p 1433:1433 --name sql_server \
  -d mcr.microsoft.com/mssql/server:2019-latest
```
## Project Configuration 🔧
1. Clone the repository:

```bash
git clone https://github.com/your-username/DnDSolution.git
cd DnDSolution
```
2. Configure connection strings in 
```bash 
appsettings.json
```
3. Apply database migrations
```bash 
cd DnDSolution.Infrastructure
dotnet ef database update
```

## Running the Project 🚀
### Web API
```bash 
cd DnDSolution.WebApi
dotnet run
```

### Rorker Service
```bash 
cd DnDSolution.Worker
dotnet run
```

### Docker Setup 🐳
```bash 
docker-compose up --build
```

## API Endpoints 🌐
### Spell Synchronization
```bash 
POST /api/spells/sync
```
🔁 Synchronizes spells from the D&D 5e API

### List Spells
```bash 
GET /api/spells
```
📜 Retrieves paginated list of spells

### Query Parameters:
```bash 
page (default: 1)
pageSize (default: 20)
search (optional)
level (optional)
school (optional)
```

### Get Spell Details
```bash 
GET /api/spells/{id}
```
🔍 Returns details of a specific spell

### Contribution 🤝
Contributions are welcome! Please follow these steps:
```bash 
1. Fork the repository
2. Create your feature branch (git checkout -b feature/AmazingFeature)
3. Commit your changes (git commit -m 'Add some amazing feature')
4. Push to the branch (git push origin feature/AmazingFeature)
5. Open a Pull Request
```

### License 📜
This project is licensed under the MIT License - see the [LICENSE](https://opensource.org/license/mit) file for details.
