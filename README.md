# UserService

A simple ASP.NET Core Web API for managing users with subscriptions, backed by PostgreSQL.

---

## Features

* CRUD operations on **User** entities
* Subscription model (`Free`, `Trial`, `Super`)
* EF Core with PostgreSQL
* MediatR-based CQRS commands & queries
* Exception-handling middleware
* Swagger UI for API exploration
* Dockerfile for containerized deployment

---

## Prerequisites

* [.NET 8.0 SDK](https://dotnet.microsoft.com/download)
* [PostgreSQL 16](https://www.postgresql.org/download/) (or Docker)
* (Optional) [Docker & Docker Compose](https://www.docker.com/products/docker-desktop)

---

## Getting Started

### 1. Clone

```bash
git clone https://github.com/andi-techfix/user-service.git
cd user-service/UserService
```

### 2. Configure Database

Copy `appsettings.Development.json` and update the `ConnectionStrings:Default` section:

```json
{
  "ConnectionStrings": {
    "Default": "Host=localhost;Port=5430;Username=postgres;Password=postgres;Database=userservicedb"
  }
}
```

### 3. Run Migrations

```bash
dotnet ef database update
```

### 4. Run Locally

```bash
dotnet run
```

The API will be listening on `https://localhost:5001` and `http://localhost:5000`.
Swagger UI is available at `https://localhost:5001/swagger`.

---
## API Endpoints

* `GET    /api/users`
* `GET    /api/users/{id}`
* `GET    /api/users/bySubscription/{subscriptionType}`
* `POST   /api/users`
* `PUT    /api/users/{id}`
* `DELETE /api/users/{id}`

Refer to Swagger UI for full request/response schemas.

---

## Testing

```bash
cd Tests/Commands
dotnet test
```

---
