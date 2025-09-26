# GelbaumD-P1
Project 1
# Barbershop API

A simple ASP.NET Core Web API for managing barbers, customers, and appointments in a barbershop scheduling system.  
Built with **.NET 7**, **Entity Framework Core**, and **SQL Server**.

---

## 📦 Features

- Manage **Customers** (create, view by ID).
- Manage **Barbers** (create).
- Manage **Appointments**:
  - Create new appointments with barber(s) and customer.
  - Update an appointment (change barber, date/time, or haircut type).
  - Delete an appointment.
  - Get appointments by barber.
  - Prevent double booking of barbers.
- Supports **enum-based haircut types** with Swagger dropdowns.

---

## 🚀 Getting Started

### Prerequisites
- [.NET 7 SDK](https://dotnet.microsoft.com/download)
- [SQL Server](https://www.microsoft.com/en-us/sql-server/sql-server-downloads)

### Setup
1. Clone this repository.
2. Update the connection string in `ConnectionString.env`.
3. Apply migrations:
   ```bash
   dotnet ef database update
### Run the application
dotnet run
### Open Swagger UI at:
https://localhost:5047/swagger
### Example Endpoints 
POST /customers (create a customer)
{
  "firstName": "John",
  "lastName": "Doe",
  "email": "john@example.com"
}
GET /customers/{id}
POST /appointments (create an appointment)
{
  "appointmentDateAndTime": "2025-09-25T16:30:00",
  "haircutType": "ShapeUp",
  "customerId": 1,
  "barberIds": [1, 2]
}
PUT /appointments/{id}
PUT /appointments/{id}
### Tech Stack
ASP.NET Core Minimal APIs

Entity Framework Core

SQL Server

Swagger / OpenAPI

Serilog (logging)
### Notes
Haircut types are defined in Models/HaircutType.cs.

All DTOs live in the DTOs folder.

To re-seed the database, update Program.cs with seed data and re-run migrations.



