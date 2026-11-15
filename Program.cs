using Microsoft.EntityFrameworkCore;
using TicketApi;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
//builder.Services.AddOpenApi();
builder.Services.AddControllers();          // enables Controllers
builder.Services.AddEndpointsApiExplorer(); // needed for Swagger
builder.Services.AddSwaggerGen();           // generates Swagger UI
builder.Services.AddDbContext<TicketDbContext>(options =>
    options.UseSqlite("Data Source=tickets.db"));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    //app.MapOpenApi();
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.MapControllers();      // routes requests to your controller classes

app.Run();