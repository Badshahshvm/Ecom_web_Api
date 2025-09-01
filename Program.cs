using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Ecomm_web_api.Data;

var builder = WebApplication.CreateBuilder(args);

// ✅ Register PostgreSQL DbContext
builder.Services.AddDbContext<DbContextApp>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection"),
        npgsqlOptions => npgsqlOptions.EnableRetryOnFailure())); // retry logic for transient failures

// ✅ Add Controllers + Swagger
builder.Services.AddControllers();

// Customize validation error response
builder.Services.Configure<ApiBehaviorOptions>(options =>
{
    options.InvalidModelStateResponseFactory = context =>
    {
        var errors = context.ModelState
            .Where(e => e.Value.Errors.Count > 0)
            .ToDictionary(
                kvp => kvp.Key,
                kvp => kvp.Value.Errors.Select(e => e.ErrorMessage).ToArray()
            );

        return new BadRequestObjectResult(new
        {
            Title = "Validation Failed",
            Status = 400,
            Errors = errors
        });
    };
});

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// ✅ Enable CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyMethod()
              .AllowAnyHeader();
    });
});

var app = builder.Build();

// ✅ Configure middleware pipeline
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// Global error handler (for production safety)
app.UseExceptionHandler("/error");

app.UseHttpsRedirection();

// ✅ Use CORS
app.UseCors("AllowAll");

app.UseAuthorization();

app.MapControllers();

// ✅ Test root endpoint
app.MapGet("/", () => Results.Ok("🚀 API with PostgreSQL Connected!"));

// ✅ Apply pending migrations on startup
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<DbContextApp>();
    db.Database.Migrate(); // auto-apply migrations at runtime
}

app.Run();
