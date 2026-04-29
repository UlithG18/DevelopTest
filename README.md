# Sports Complex Reservation System

An internal web application built with ASP.NET Core MVC and Entity Framework Core for managing users, sport areas, and reservations in a sports complex. The system centralizes information, prevents scheduling conflicts, and handles the full reservation lifecycle.

---

## Table of Contents

- [Requirements](#requirements)
- [Technology Stack](#technology-stack)
- [Project Structure](#project-structure)
- [Database Setup](#database-setup)
- [Configuration](#configuration)
- [Running the Application](#running-the-application)
- [Features](#features)
- [Business Rules](#business-rules)
- [Diagrams](#diagrams)

---

## Requirements

- [.NET 10 SDK](https://dotnet.microsoft.com/download)
- MySQL 8.0 or higher
- Git
- EFC

---

## Technology Stack

- ASP.NET Core MVC (.NET 10)
- Entity Framework Core 9 with Pomelo MySQL provider
- MySQL as the relational database
- LINQ for data querying
- `List<T>` and `Dictionary<TKey, TValue>` for in-memory data management

---

## Project Structure

```
DevelopTest/
├── Controllers/
│   ├── HomeController.cs
│   ├── UserController.cs
│   ├── SportAreaController.cs
│   └── ReservationController.cs
├── Models/
│   ├── User.cs
│   ├── SportArea.cs
│   └── Reservations.cs
├── Services/
│   ├── UserService.cs
│   ├── SportAreaService.cs
│   └── ReservationService.cs
├── Data/
│   └── MySqlDbContext.cs
├── Responses/
│   └── ServiceResponse.cs
├── appsettings.json
└── Program.cs
```

---

## Database Setup

1. Create a MySQL database:

```sql
CREATE DATABASE SportComplexDb;
```

2. The application uses EF Core migrations to create and update the schema automatically. After configuring the connection string (see next section), run:

```bash
dotnet ef migrations add InitialCreate
dotnet ef database update
```

If the `dotnet-ef` tool is not installed, install it first:

```bash
dotnet tool install --global dotnet-ef
```

---

## Configuration

Open `appsettings.json` and update the `DefaultConnection` string with your MySQL credentials:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "server=YOUR_HOST;port=3306;database=SportComplexDb;user=YOUR_USER;password=YOUR_PASSWORD"
  }
}
```

Replace `YOUR_HOST`, `YOUR_USER`, and `YOUR_PASSWORD` with the appropriate values for your environment.

---

## Running the Application

1. Clone the repository:

```bash
git clone https://github.com/UlithG18/DevelopTest
cd DevelopTest
```

2. Restore dependencies:

```bash
dotnet restore
```

3. Apply database migrations:

```bash
dotnet ef database update
```

4. Run the application:

```bash
dotnet run
```

5. Open a browser and navigate to `https://localhost:5001` or `http://localhost:5000`.

---

## Features

### User Management

- Register new users with name, identity document, phone number, and email.
- Edit existing user information.
- Prevent duplicate users by validating uniqueness of identity document and email.
- List all registered users.

### Sport Area Management

- Register sport areas with name, type (Soccer, Basketball, Pool, Tennis, Volleyball, Gym, Athletics, SkatePark), opening/closing hours, and capacity.
- Edit sport area information.
- Prevent duplicate sport areas.
- List all sport areas.
- Filter sport areas by type.

### Reservation Management

- Create reservations by linking a user, a sport area, a date, a start time, and an end time.
- Cancel a reservation, changing its status to `Canceled`.
- Mark a reservation as `Finished`.
- List reservations filtered by user.
- List reservations filtered by sport area.

---

## Business Rules

The system enforces the following rules on every reservation operation:

- End time must be later than start time.
- Reservations cannot be created for past dates or times.
- A sport area cannot have two overlapping reservations in the same time range.
- A user cannot have more than one reservation in the same time range.
- All errors are handled with try-catch blocks and clear, user-friendly messages.

---

## Diagrams

The following diagrams are included in the repository root:

- `class-diagram` — Class diagram showing entities, attributes, methods, and relationships.
- `use-case-diagram` — Use case diagram showing actors (User, Administrator, SMTP Server) and system interactions.
