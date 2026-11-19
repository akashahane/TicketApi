# TicketApi

A REST API for managing customer support tickets, built with ASP.NET Core and Entity Framework Core. Users register and log in to receive a JWT, which they use to create, track and resolve tickets.

## Features

- Full CRUD for support tickets: create, list, get by id, update status, delete
- Ticket status workflow: `Open`, `InProgress`, `Closed`
- Persistent storage with Entity Framework Core and SQLite, managed through migrations
- User registration with salted password hashing (ASP.NET Core `PasswordHasher`)
- JWT authentication: login issues a signed token valid for 1 hour, and all ticket endpoints require it
- Input validation: empty fields and duplicate usernames are rejected
- Meaningful HTTP responses: `200 OK`, `400 Bad Request`, `401 Unauthorized`, `404 Not Found`
- Interactive API documentation with Swagger UI, including an **Authorize** button for testing with a token
- Unit tests with xUnit and the EF Core in-memory provider

## Tech stack

- C# / .NET 9
- ASP.NET Core Web API (controllers)
- Entity Framework Core 9 with SQLite
- JWT bearer authentication
- xUnit
- Swashbuckle (Swagger UI)

## Getting started

**Prerequisites:** [.NET 9 SDK](https://dotnet.microsoft.com/download)

```bash
# 1. Clone the repository
git clone https://github.com/akashahane/TicketApi.git
cd TicketApi

# 2. Install the EF Core command-line tool (one-time)
dotnet tool install --global dotnet-ef --version 9.0.11

# 3. Set a JWT signing key (stored on your machine, never in the repository)
dotnet user-secrets set "Jwt:Key" "<a random string of at least 32 characters>"

# 4. Create the local SQLite database from the migrations
dotnet ef database update --project TicketApi.csproj

# 5. Run the API
dotnet run --project TicketApi.csproj
```

Then open `http://localhost:5104/swagger` in your browser.

### Trying it out in Swagger

1. `POST /api/auth/register` with a username and password.
2. `POST /api/auth/login` with the same credentials, and copy the `token` from the response.
3. Click **Authorize** at the top right, paste the token, and click **Authorize**.
4. The ticket endpoints now work. Without a token they return `401 Unauthorized`.

## API endpoints

### Auth

| Method | Endpoint             | Description                                                        |
| ------ | -------------------- | ------------------------------------------------------------------ |
| POST   | `/api/auth/register` | Register a user. Body: `{ "username": "...", "password": "..." }`  |
| POST   | `/api/auth/login`    | Log in and receive a JWT valid for 1 hour. Same body as register   |

### Tickets (require `Authorization: Bearer <token>`)

| Method | Endpoint            | Description                                                  |
| ------ | ------------------- | ------------------------------------------------------------ |
| GET    | `/api/tickets`      | List all tickets                                             |
| GET    | `/api/tickets/{id}` | Get one ticket (404 if not found)                            |
| POST   | `/api/tickets`      | Create a ticket. Body: `{ "title": "..." }`                  |
| PUT    | `/api/tickets/{id}` | Update a ticket's status. Body: `{ "status": "InProgress" }` |
| DELETE | `/api/tickets/{id}` | Delete a ticket (404 if not found)                           |

## Running tests

```bash
dotnet test TicketApi.sln
```

The tests call `TicketsController` directly with an EF Core in-memory database, so they need no running server and never touch the real database. They cover:

- New tickets start as `Open` and keep their title
- Creating a ticket saves it to the database
- Updating a ticket's status is saved to the database
- Deleting a ticket removes it from the database
- Get-by-id and update-status return `404 Not Found` for unknown ids

Each save is verified through a fresh database context, so a missing `SaveChangesAsync()` call makes the tests fail instead of passing silently.

## Configuration

| Setting                                | Where it lives              | Purpose                              |
| -------------------------------------- | --------------------------- | ------------------------------------ |
| `Jwt:Key`                              | .NET user secrets           | Secret key used to sign tokens       |
| `Jwt:Issuer`, `Jwt:Audience`           | `appsettings.json`          | Checked when a token is validated    |
| `ConnectionStrings:DefaultConnection`  | `appsettings.json`          | SQLite database location             |

The signing key is deliberately kept out of the repository: anyone holding it could create valid tokens.

## Project structure

```
TicketApi/
├── controllers/
│   ├── AuthController.cs        # Register and login (JWT)
│   └── TicketsController.cs     # Ticket CRUD endpoints (require a token)
├── Migrations/                  # EF Core migrations (InitialCreate, AddUsers)
├── TicketApi.Tests/
│   ├── TicketTests.cs           # Tests for the Ticket entity
│   └── TicketsControllerTests.cs  # Tests for every TicketsController action
├── Ticket.cs                    # Ticket entity, TicketStatus enum, DTOs
├── User.cs                      # User entity
├── TicketDbContext.cs           # EF Core database context
└── Program.cs                   # Startup: dependency injection, authentication, middleware
```

## Possible improvements

- Link each ticket to the user who created it, and show users only their own tickets
- Pagination and filtering, for example `GET /api/tickets?status=Open`
- Integration tests that send real HTTP requests through the full pipeline
- Switch from SQLite to SQL Server for production use
