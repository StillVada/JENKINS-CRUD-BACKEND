using Api.Interfaces;
using Api.Repositories;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo
    {
        Title = "CRUD API",
        Version = "v1",
        Description = "Простое REST API для демонстрации CRUD операций с продуктами",
        Contact = new Microsoft.OpenApi.Models.OpenApiContact
        {
            Name = "CRUD API Team",
            Email = "api@example.com"
        }
    });

    // Включаем XML комментарии для Swagger
    var xmlFile = $"{System.Reflection.Assembly.GetExecutingAssembly().GetName().Name}.xml";
    var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
    options.IncludeXmlComments(xmlPath);
});

// Configure database connection
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection")
    ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");

// Register repository
builder.Services.AddScoped<IProductRepository>(provider =>
    new ProductRepository(connectionString));

var app = builder.Build();

// Configure the HTTP request pipeline.
app.UseSwagger();
app.UseSwaggerUI(options =>
{
    options.SwaggerEndpoint("/swagger/v1/swagger.json", "CRUD API v1");
    options.RoutePrefix = ""; // Доступно по корневому пути /
    options.DocumentTitle = "CRUD API - Swagger Documentation";
    options.DisplayRequestDuration(); // Показывать время выполнения запросов
});

// Добавим перенаправление с /swagger на корневой путь
app.MapGet("/swagger", context =>
{
    context.Response.Redirect("/");
    return Task.CompletedTask;
});

app.UseHttpsRedirection();
app.UseAuthorization();

// Добавим endpoint для проверки здоровья API
app.MapGet("/", () => Results.Redirect("/swagger"));
app.MapGet("/health", () => Results.Ok(new { status = "healthy", timestamp = DateTime.UtcNow }));

app.MapControllers();

app.Run();
