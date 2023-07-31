using Application.Interfaces;
using Application.Models.ViewModel;
using Application.Services;
using Domain.Contract;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Hosting.Server;
using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Web;
using Persistence.Context;
using Persistence.Repository;
using static System.Net.Mime.MediaTypeNames;

const string _connectionString = "name=ConnectionStrings:DefaultConnection";

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddMicrosoftIdentityWebApi(builder.Configuration.GetSection("AzureAd"));

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(gen =>
{
});
builder.Services.AddDbContext<FluxoCaixaContext>();

builder.Services.AddScoped<IRepositoryAccount, RepositoryAccount>();
builder.Services.AddScoped<IRepositoryBalance, RepositoryBalance>();
builder.Services.AddScoped<IRepositoryExtract, RepositoryExtract>();
builder.Services.AddScoped<IRepositoryRecord, RepositoryRecord>();

builder.Services.AddScoped<IApplicationServiceAccount, AccountApplicationService>();
builder.Services.AddScoped<IApplicationServiceBalance, BalanceApplicationService>();
builder.Services.AddScoped<IApplicationServiceRecord, RecordApplicationService>();
builder.Services.AddScoped<IApplicationServiceFluxoCaixa, FluxoCaixaApplicationService>();

builder.Services.AddAutoMapper(System.Reflection.Assembly.Load("Application"));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
  app.UseSwagger();
  app.UseSwaggerUI();
}

app.UseCors(cors => cors.AllowAnyOrigin());

app.UseHttpsRedirection();

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();
