

using Collaboration.Endpoints;
using Collaboration.Entities;
using Collaboration.Extensions;
using Collaboration.Repositories.Classes;
using Collaboration.Repositories.Interfaces;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Http.HttpResults;
using System.Xml.Linq;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.ConfigureServicesExtensions(builder.Configuration);
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
// user endpoints
app.RegisterUserEndPoints();




app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
