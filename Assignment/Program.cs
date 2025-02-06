using System.Reflection;
using Assignment.Data;
using Assignment.Data.Collections;
using Assignment.Data.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);
//Controller Registration
builder.Services.AddControllers();

//Swagger setup
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
builder.Services.AddEntityFrameworkNpgsql()
    .AddDbContext<AppDbContext>(options =>
    {
        options.UseNpgsql(
            builder.Configuration.GetConnectionString("DefaultConnection"),
            o=> o.MapEnum<Books.Availability>("availability") 
        );
    });

//Singleton Area
builder.Services.AddSingleton<CustomerCollection>();
builder.Services.AddSingleton<BookCollection>();
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.MapControllers();

app.Run();

