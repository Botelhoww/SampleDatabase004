using Microsoft.EntityFrameworkCore;
using SampleDatabase004;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var connection = string.Empty;
connection = builder.Configuration.GetConnectionString("AZURE_SQL_CONNECTION_STRING");

builder.Services.AddDbContext<Db2netr004Context>(options =>
    options.UseSqlServer(connection));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.MapGet("/api/GetByIdWithDetails/{id}", async (int id, Db2netr004Context db) =>
{
    var result = await db.SalesOrderHeaders
        .Include(x => x.SalesOrderDetails)
        .FirstOrDefaultAsync(x => x.SalesOrderId == id);

    if (result == null)
        return Results.NotFound();

    return Results.Ok(result);
});

app.Run();