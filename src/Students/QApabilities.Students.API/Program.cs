using QApabilities.Students.API.Data;
using QApabilities.Students.API.Services;
using QApabilities.Students.API.Validators;
using Microsoft.EntityFrameworkCore;
using Serilog;
using FluentValidation;

var builder = WebApplication.CreateBuilder(args);

// Configuração do Serilog
Log.Logger = new LoggerConfiguration()
    .ReadFrom.Configuration(builder.Configuration)
    .CreateLogger();

builder.Host.UseSerilog();

// Add services to the container.
builder.Services.AddControllers();

// Configuração do Entity Framework
builder.Services.AddDbContext<StudentsDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Configuração do AutoMapper
builder.Services.AddAutoMapper(typeof(Program));

// Configuração do FluentValidation
builder.Services.AddFluentValidationAutoValidation();
builder.Services.AddValidatorsFromAssemblyContaining<CreateStudentDtoValidator>();

// Configuração dos serviços
builder.Services.AddScoped<IStudentService, StudentService>();
builder.Services.AddScoped<IStudentRepository, StudentRepository>();

// Configuração do Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new() { Title = "QApabilities Students API", Version = "v1" });
});

// Configuração dos Health Checks
builder.Services.AddHealthChecks()
    .AddDbContextCheck<StudentsDbContext>();

// Configuração do CORS
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

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseCors("AllowAll");

app.UseAuthorization();

app.MapControllers();

// Health Check endpoint
app.MapHealthChecks("/health");

// Migração automática do banco de dados
using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<StudentsDbContext>();
    context.Database.Migrate();
}

try
{
    Log.Information("Iniciando QApabilities Students API");
    app.Run();
}
catch (Exception ex)
{
    Log.Fatal(ex, "Aplicação terminou inesperadamente");
}
finally
{
    Log.CloseAndFlush();
} 