using FluentValidation;
using WorkoutAPI.Application;
using WorkoutAPI.Infrastructure;
using WorkoutAPI.Infrastructure.Data;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container
builder.Services.AddControllers();

// Add CORS
builder.Services.AddCors(options => {
    options.AddPolicy("AllowAll", policy => {
        policy.AllowAnyOrigin()
              .AllowAnyMethod()
              .AllowAnyHeader();
    });
});

// Add Swagger/OpenAPI
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c => {
    c.SwaggerDoc("v1", new() {
        Title = "Workout API",
        Version = "v1",
        Description = "A comprehensive workout tracking API with multi-language support"
    });
});

// Add Application and Infrastructure layers
builder.Services.AddApplication();
builder.Services.AddInfrastructure(builder.Configuration);

// Add FluentValidation
builder.Services.AddValidatorsFromAssemblyContaining<WorkoutAPI.Application.Validators.CreateUserRequestValidator>();

var app = builder.Build();

// Configure the HTTP request pipeline
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c => {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "Workout API v1");
        c.RoutePrefix = string.Empty; // Set Swagger UI at app's root
    });
}

// Enable CORS
app.UseCors("AllowAll");

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

// Ensure database is created
using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<WorkoutDbContext>();
    context.Database.EnsureCreated(); // Commented out to avoid Entity Framework issues
}

app.Run("http://0.0.0.0:5000");

