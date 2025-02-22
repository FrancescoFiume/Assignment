using System.Reflection;
using System.Text.Json.Serialization;
using Asp.Versioning;
using Assignment.Db;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddProblemDetails();
builder.Services
    .AddControllers()
    .AddJsonOptions(o =>
    {
        o.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
    });


builder.Services
    .AddApiVersioning(o =>
    {
        o.DefaultApiVersion = new ApiVersion(1, 0);
        o.ApiVersionReader = new HeaderApiVersionReader("X-Api-Version");
    })
    .AddMvc();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
    
    c.IncludeXmlComments(xmlPath);
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "Assignment API",
        Version = "v1",
        Description = "Simple API to showcase few asp.NET skills",
    });
});


builder.Services.AddDbContextFactory<AppDbContext>(options =>
{
    options.UseNpgsql(
        builder.Configuration.GetConnectionString("DefaultConnection")
    );
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.MapControllers();

app.Run();