# CRUD API

A simple REST API built with ASP.NET Core and SQL Server for demonstrating CI/CD pipelines with Jenkins.

## Features

- **CRUD Operations**: Create, Read, Update, Delete products
- **SQL Server Database**: Persistent storage with ADO.NET
- **RESTful API**: Clean REST endpoints
- **Swagger Documentation**: Interactive API documentation
- **Unit Tests**: Repository layer testing
- **Integration Tests**: End-to-end API testing
- **Docker Support**: Containerized deployment

## Project Structure

```
CRUD_API/
├── Api/                          # Main ASP.NET Core API project
│   ├── Controllers/              # API controllers
│   ├── Interfaces/               # Repository interfaces
│   ├── Models/                   # Data models
│   ├── Repositories/             # Data access layer
│   ├── Program.cs               # Application entry point
│   └── appsettings.json         # Configuration
├── Api.UnitTests/                # Unit tests
├── Api.IntegrationTests/         # Integration tests
├── database-setup.sql           # Database initialization script
├── Dockerfile                   # Docker build configuration
├── docker-compose.yml           # Local development setup
└── README.md                    # This file
```

## API Endpoints

| Method | Endpoint | Description |
|--------|----------|-------------|
| GET | `/api/products` | Get all products |
| GET | `/api/products/{id}` | Get product by ID |
| POST | `/api/products` | Create new product |
| PUT | `/api/products/{id}` | Update existing product |
| DELETE | `/api/products/{id}` | Delete product |

## Swagger UI

API включает интерактивную документацию Swagger для удобного тестирования:

- **URL**: `http://localhost:{port}/` (корневой путь, автоматически перенаправляет на Swagger)
- **Альтернативный URL**: `http://localhost:{port}/swagger`
- **Возможности**:
  - Просмотр всех доступных endpoints
  - Интерактивное тестирование API
  - Просмотр схем данных и примеров
  - Автоматическая генерация запросов

### Как использовать Swagger UI:

1. Запустите приложение
2. Откройте браузер и перейдите на `http://localhost:{port}/`
3. Выберите интересующий endpoint
4. Нажмите "Try it out"
5. Заполните необходимые параметры
6. Нажмите "Execute" для отправки запроса

### Дополнительные endpoints:

- **GET /** - Перенаправление на Swagger UI
- **GET /health** - Проверка здоровья API
- **GET /swagger** - Альтернативный путь к Swagger UI

## Product Model

```json
{
  "id": 1,
  "name": "Product Name",
  "description": "Product Description",
  "price": 99.99,
  "createdAt": "2023-12-25T10:00:00Z",
  "updatedAt": "2023-12-25T10:00:00Z"
}
```

## Local Development

### Prerequisites

- .NET 8.0 SDK
- SQL Server (or Docker)
- Docker (optional, for containerized setup)

### Running with Docker Compose

1. Clone the repository
2. Run the application:
   ```bash
   docker-compose up --build
   ```
3. Access the API at `http://localhost:8080`
4. View Swagger documentation at `http://localhost:8080/swagger`

### Running Locally

1. **Setup Database**:
   - Create a SQL Server database
   - Run the `database-setup.sql` script

2. **Configure Connection String**:
   - Update `Api/appsettings.json` with your database connection string

3. **Run the Application**:
   ```bash
   cd Api
   dotnet run
   ```

4. **Access Swagger UI**:
   - Open browser: `http://localhost:{port}/swagger`
   - Test API endpoints interactively

4. **Run Tests**:
   ```bash
   dotnet test
   ```

## Deployment

### Build Docker Image

```bash
docker build -t crud-api .
```

### Run Container

```bash
docker run -p 8080:80 crud-api
```

### Environment Variables

For production deployment, set the following environment variables:

- `ConnectionStrings__DefaultConnection`: Database connection string
- `ASPNETCORE_ENVIRONMENT`: Environment (Production)
- `ASPNETCORE_URLS`: URLs to listen on

## CI/CD Pipeline

This project is designed for Jenkins CI/CD pipelines. The pipeline should:

1. **Build**: Restore dependencies and build the application
2. **Test**: Run unit and integration tests
3. **Package**: Create Docker image
4. **Deploy**: Deploy to cloud environment

Example Jenkins pipeline stages:

```groovy
pipeline {
    agent any

    stages {
        stage('Build') {
            steps {
                sh 'dotnet restore'
                sh 'dotnet build --configuration Release'
            }
        }

        stage('Test') {
            steps {
                sh 'dotnet test --configuration Release --no-build'
            }
        }

        stage('Docker Build') {
            steps {
                sh 'docker build -t crud-api:$BUILD_NUMBER .'
            }
        }

        stage('Deploy') {
            steps {
                // Deploy to your cloud environment
                sh 'docker-compose up -d'
            }
        }
    }
}
```

## Database Schema

The application uses a single `Products` table:

```sql
CREATE TABLE Products (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    Name NVARCHAR(100) NOT NULL,
    Description NVARCHAR(500) NOT NULL,
    Price DECIMAL(18,2) NOT NULL,
    CreatedAt DATETIME2 NOT NULL,
    UpdatedAt DATETIME2 NOT NULL
);
```

## Technologies Used

- **ASP.NET Core 8.0**: Web framework
- **SQL Server**: Database
- **ADO.NET**: Data access
- **xUnit**: Testing framework
- **Swagger/OpenAPI**: API documentation
- **Docker**: Containerization
- **Entity Framework**: ORM (not used, raw SQL for simplicity)

## Contributing

1. Fork the repository
2. Create a feature branch
3. Make your changes
4. Add tests
5. Submit a pull request

## License

This project is for educational purposes and CI/CD demonstration.
