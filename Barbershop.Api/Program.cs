using Microsoft.EntityFrameworkCore;
using System.Text.Json.Serialization;
using System.Reflection;
using System.ComponentModel.DataAnnotations;
using Barbershop.Data;
using Barbershop.Models;
using Barbershop.Services;
using Barbershop.Repositories;
using Barbershop.DTOs;
using Serilog;

var builder = WebApplication.CreateBuilder(args);
string CS = File.ReadAllText("./ConnectionString.env");


// Add services to the container
builder.Services.AddControllers().AddJsonOptions(options =>
{
    options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
});
builder.Services.ConfigureHttpJsonOptions(options =>
{
    options.SerializerOptions.Converters.Add(new JsonStringEnumConverter());
});
builder.Services.AddEndpointsApiExplorer();   // ✅ Needed for Swagger/OpenAPI
builder.Services.AddSwaggerGen();            // ✅ Needed for Swagger/OpenAPI
builder.Services.AddOpenApi();

// ✅ Register DbContext BEFORE builder.Build()
builder.Services.AddDbContext<BarbershopDbContext>(options =>
    options.UseSqlServer(CS));

// ✅ You can also register repositories/services here
builder.Services.AddScoped<ICustomerRepository, CustomerRepository>();
builder.Services.AddScoped<IBarberRepository, BarberRepository>();
builder.Services.AddScoped<IAppointmentRepository, AppointmentRepository>();
builder.Services.AddScoped<IAppointmentService, AppointmentService>();
builder.Services.AddScoped<ICustomerService, CustomerService>();
builder.Services.AddScoped<IBarberService, BarberService>();

var app = builder.Build(); // ✅ Build AFTER services registered

// Configure the HTTP request pipeline
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.UseSwagger();
    app.UseSwaggerUI();
    app.UseDeveloperExceptionPage();

}

app.UseHttpsRedirection();
// _____________________________Root_____________________________
app.MapGet("/", () => {
    return "Lets get you a crispy fade!";
});
// _____________________________Customer_____________________________
// Create a customer
app.MapPost("/customers", async (CustomerCreateDto dto, ICustomerService service) =>
{
    var customer = new Customer
    {
        FirstName = dto.FirstName,
        LastName  = dto.LastName,
        Email     = dto.Email
    };

    await service.CreateAsync(customer);

    return Results.Created($"/customers/{customer.Id}", new
    {
        customer.Id,
        customer.FirstName,
        customer.LastName,
        customer.Email
    });
});
// Get Customer by Id
app.MapGet("/customers/{id:int}", async (int id, ICustomerService customerService) =>
{
    var customer = await customerService.GetByIdAsync(id);
    if (customer == null)
        return Results.NotFound(new { message = $"Customer with ID {id} not found." });

    // Map Customer → CustomerDto
    var dto = new CustomerDto
    {
        Id = customer.Id,
        FirstName = customer.FirstName,
        LastName = customer.LastName,
        Email = customer.Email,
        Appointments = customer.Appointments.Select(a => new AppointmentDto
        {
            Id = a.Id,
            AppointmentDateAndTime = a.AppointmentDateAndTime,
            HaircutType = a.HaircutType.ToString(),
            Barbers = a.Barbers.Select(b => new BarberDto
            {
                Id = b.Id,
                Name = b.Name
            }).ToList()
        }).ToList()
    };

    return Results.Ok(dto);
});

// _____________________________Appointments_____________________________
// Helps with Haircut types, now a selection of strings in an Enum...
string GetDisplayName(Enum enumValue) =>
    enumValue.GetType()
             .GetMember(enumValue.ToString())[0]
             .GetCustomAttribute<DisplayAttribute>()?
             .Name ?? enumValue.ToString();

// Create an appointment
app.MapPost("/appointments", async (AppointmentCreateDto dto, IAppointmentService appointmentService) =>
{
    // Validate basic request
    if (dto.CustomerId <= 0)
        return Results.BadRequest(new { message = "CustomerId is required." });
    // if (!dto.BarberIds.Any())
    //     return Results.BadRequest(new { message = "At least one BarberId is required." });

    try
    {
        var appointment = new Appointment
        {
            AppointmentDateAndTime = dto.AppointmentDateAndTime,
            HaircutType = dto.HaircutType,
            CustomerId = dto.CustomerId,
            Barbers = new List<Barber>()
        };

        await appointmentService.CreateAsync(appointment);

        return Results.Created($"/appointments/{appointment.Id}", new
        {
            appointment.Id,
            appointment.AppointmentDateAndTime,
            HaircutType = appointment.HaircutType.ToString(),
            appointment.CustomerId,
            Barbers = dto.BarberIds
        });
    }
    catch (Exception ex)
    {
        // TODO: log exception with Serilog
        return Results.Problem("Failed to create appointment: " + ex.Message);
    }
});
// get an appointment by its Id
app.MapGet("/appointments/{id:int}", async (int id, IAppointmentService appointmentService) =>
{
    var appointment = await appointmentService.GetByIdAsync(id);

    if (appointment == null)
        return Results.NotFound(new { message = $"Appointment with ID {id} not found." });

    var dto = new AppointmentDto
    {
        Id = appointment.Id,
        AppointmentDateAndTime = appointment.AppointmentDateAndTime,
        HaircutType = GetDisplayName(appointment.HaircutType) // "Haircut and Beard"
    };

    return Results.Ok(dto);
});
// Update a current appointment
app.MapPut("/appointments/{id:int}", async (int id, AppointmentUpdateDto dto, IAppointmentService appointmentService) =>
{
    try
    {
        var updated = await appointmentService.UpdateAsync(
            id,
            dto.BarberId,
            dto.AppointmentDateAndTime,
            dto.HaircutType
        );

        if (!updated)
            return Results.NotFound(new { message = $"Appointment with ID {id} not found." });

        return Results.Ok(new
        {
            message = "Appointment updated successfully",
            appointmentId = id,
            dto.BarberId,
            dto.AppointmentDateAndTime,
            dto.HaircutType
        });
    }
    catch (KeyNotFoundException ex)
    {
        return Results.NotFound(new { message = ex.Message });
    }
    catch (InvalidOperationException ex)
    {
        return Results.BadRequest(new { message = ex.Message });
    }
});

// app.MapPut("/appointments/{id:int}", async (int id, AppointmentUpdateDto dto, IAppointmentService appointmentService) =>
// {
//     try
//     {
//         var updated = await appointmentService.UpdateAsync(id, dto.BarberId, dto.AppointmentDateAndTime);

//         if (!updated)
//             return Results.NotFound(new { message = $"Appointment with ID {id} not found." });

//         return Results.Ok(new
//         {
//             message = "Appointment updated successfully",
//             appointmentId = id,
//             dto.BarberId,
//             dto.AppointmentDateAndTime
//         });
//     }
//     catch (KeyNotFoundException ex)
//     {
//         return Results.NotFound(new { message = ex.Message });
//     }
//     catch (InvalidOperationException ex)
//     {
//         return Results.BadRequest(new { message = ex.Message });
//     }
//     catch (Exception ex)
//     {
//         return Results.Problem($"Something went wrong: {ex.Message}");
//     }
// });
// Get all appointments by barberId
app.MapGet("/appointments/barber/{barberId:int}", async (int barberId, IAppointmentService appointmentService) =>
{
    try
    {
        var appointments = await appointmentService.GetAppointmentsByBarberIdAsync(barberId);

        if (!appointments.Any())
            return Results.NotFound(new { message = $"No appointments found for barber {barberId}" });

        var dtoList = appointments.Select(a => new AppointmentDto
        {
            Id = a.Id,
            AppointmentDateAndTime = a.AppointmentDateAndTime,
            HaircutType = a.HaircutType.ToString(),
            Barbers = a.Barbers.Select(b => new BarberDto { Id = b.Id, Name = b.Name }).ToList()
        }).ToList();

        return Results.Ok(dtoList);
    }
    catch (Exception ex)
    {
        Console.WriteLine($"❌ ERROR: {ex}");
        return Results.Problem($"Something went wrong: {ex.Message}");
    }
});
// Delete an existing Appointment
app.MapDelete("/appointments/{id:int}", async (int id, IAppointmentService appointmentService) =>
{
    try
    {
        bool deleted = await appointmentService.DeleteAsync(id);

        if (!deleted)
        {
            return Results.NotFound(new { message = $"Appointment with ID {id} not found." });
        }

        return Results.Ok(new { message = $"Appointment with ID {id} was deleted successfully." });
    }
    catch (Exception ex)
    {
        return Results.Problem("Failed to delete appointment: " + ex.Message);
    }
});

// _____________________________Barber_____________________________
// Create a barber
app.MapPost("/barbers", async (BarberCreateDto dto, IBarberService barberService) =>
{
    if (string.IsNullOrWhiteSpace(dto.Name))
    {
        return Results.BadRequest(new { message = "Barber name is required." });
    }

    try
    {
        var barber = new Barber
        {
            Name = dto.Name
        };

        await barberService.CreateAsync(barber);

        return Results.Created($"/barbers/{barber.Id}", new
        {
            barber.Id,
            barber.Name
        });
    }
    catch (Exception ex)
    {
        return Results.Problem("Failed to create barber: " + ex.Message);
    }
});
using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<BarbershopDbContext>();

    if (!context.Barbers.Any() && !context.Customers.Any())
    {
        // ✅ Barbers
        var barber1 = new Barber { Name = "Alex Fadez" };
        var barber2 = new Barber { Name = "Chris Linez" };
        var barber3 = new Barber { Name = "Jordan Clippers" };

        // ✅ Customers
        var customer1 = new Customer { FirstName = "John", LastName = "Doe", Email = "john@example.com" };
        var customer2 = new Customer { FirstName = "Jane", LastName = "Smith", Email = "jane@example.com" };
        var customer3 = new Customer { FirstName = "Sam", LastName = "Williams", Email = "sam@example.com" };
        var customer4 = new Customer { FirstName = "Emily", LastName = "Brown", Email = "emily@example.com" };

        // ✅ Appointments (each has at least 1 barber assigned)
        var appointment1 = new Appointment
        {
            AppointmentDateAndTime = DateTime.Today.AddHours(9),
            HaircutType = HaircutType.Haircut,
            Customer = customer1,
            Barbers = new List<Barber> { barber1 }
        };

        var appointment2 = new Appointment
        {
            AppointmentDateAndTime = DateTime.Today.AddHours(10),
            HaircutType = HaircutType.HaircutAndBeard,
            Customer = customer2,
            Barbers = new List<Barber> { barber2 }
        };

        var appointment3 = new Appointment
        {
            AppointmentDateAndTime = DateTime.Today.AddHours(11),
            HaircutType = HaircutType.ShapeUp,
            Customer = customer3,
            Barbers = new List<Barber> { barber3 }
        };

        var appointment4 = new Appointment
        {
            AppointmentDateAndTime = DateTime.Today.AddHours(13),
            HaircutType = HaircutType.Shampoo,
            Customer = customer4,
            Barbers = new List<Barber> { barber1, barber2 }
        };

        var appointment5 = new Appointment
        {
            AppointmentDateAndTime = DateTime.Today.AddHours(15),
            HaircutType = HaircutType.TheWorks,
            Customer = customer1,
            Barbers = new List<Barber> { barber2, barber3 }
        };

        var appointment6 = new Appointment
        {
            AppointmentDateAndTime = DateTime.Today.AddHours(16),
            HaircutType = HaircutType.Haircut,
            Customer = customer2,
            Barbers = new List<Barber> { barber1, barber3 }
        };

        // ✅ Add everything to context
        context.AddRange(barber1, barber2, barber3);
        context.AddRange(customer1, customer2, customer3, customer4);
        context.AddRange(appointment1, appointment2, appointment3, appointment4, appointment5, appointment6);

        context.SaveChanges();
    }
}

app.Run();

