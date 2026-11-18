# TicketApi

A REST API for managing customer support tickets, built with ASP.NET Core and Entity Framework Core.

## Features

- Full CRUD for support tickets: create, list, get by id, update status, delete
- Ticket status workflow: `Open`, `InProgress`, `Closed`
- Persistent storage with Entity Framework Core and SQLite, managed through migrations
- User registration with salted password hashing (ASP.NET Core `PasswordHasher`)
- Duplicate-username and empty-input validation
- Proper HTTP responses: `200 OK`, `400 Bad Request`, `404 Not Found`
- Interactive API documentation with Swagger UI
- JWT authentication with protected endpoints

## Tech stack

- C# / .NET 9
- ASP.NET Core Web API (controllers)
- Entity Framework Core 9 with SQLite
- Swashbuckle (Swagger UI)

## Getting started

**Prerequisites:** [.NET 9 SDK](https://dotnet.microsoft.com/download)

```bash
# Clone the repository
git clone https://github.com/akashahane/TicketApi.git
cd TicketApi

# Install the EF Core command-line tool (one-time)
dotnet tool install --global dotnet-ef --version 9.0.11

# Create the local SQLite database from the migrations
dotnet ef database update

# Run the API
dotnet run
```

Then open `http://localhost:<port>/swagger` in your browser. The port is printed in the terminal when the app starts.

## API endpoints

### Tickets

| Method | Endpoint            | Description                                                  |
| ------ | ------------------- | ------------------------------------------------------------ |
| GET    | `/api/tickets`      | List all tickets                                             |
| GET    | `/api/tickets/{id}` | Get one ticket (404 if not found)                            |
| POST   | `/api/tickets`      | Create a ticket. Body: `{ "title": "..." }`                  |
| PUT    | `/api/tickets/{id}` | Update a ticket's status. Body: `{ "status": "InProgress" }` |
| DELETE | `/api/tickets/{id}` | Delete a ticket (404 if not found)                           |

### Auth

| Method | Endpoint             | Description                                                       |
| ------ | -------------------- | ----------------------------------------------------------------- |
| POST   | `/api/auth/register` | Register a user. Body: `{ "username": "...", "password": "..." }` |

## Project structure

```
TicketApi/
├── controllers/
│   ├── TicketsController.cs   # Ticket CRUD endpoints
│   └── AuthController.cs      # User registration
├── Migrations/                # EF Core migrations (InitialCreate, AddUsers)
├── Ticket.cs                  # Ticket entity, TicketStatus enum, DTOs
├── User.cs                    # User entity
├── TicketDbContext.cs         # EF Core database context
└── Program.cs                 # App startup, dependency injection, middleware
```

## Roadmap

- [ ] Login endpoint returning a JWT
- [ ] Protect ticket endpoints with `[Authorize]`
- [ ] Unit tests with xUnit

### Auth

| Method | Endpoint | Description |
|--------|----------|-------------|
| POST | `/api/auth/register` | Register a user. Body: `{ "username": "...", "password": "..." }` |
| POST | `/api/auth/login` | Log in and receive a JWT valid for 1 hour |

All `/api/tickets` endpoints require a token: `Authorization: Bearer <token>`.
In Swagger, call login, then click **Authorize** and paste the token.

## Configuration

The JWT signing key is not stored in the repository. Set one locally with .NET user secrets:

```bash
dotnet user-secrets set "Jwt:Key" "<a random string of at least 32 characters>"
```

## Roadmap

- [x] Login endpoint returning a JWT
- [x] Protect ticket endpoints with `[Authorize]`
- [ ] Unit tests with xUnit