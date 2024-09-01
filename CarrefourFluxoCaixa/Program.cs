using Application.Interfaces;
using Application.Services;
using Domain.Contract;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Identity.Web;
using Microsoft.OpenApi.Models;
using Persistence.Context;
using Persistence.Repository;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddMicrosoftIdentityWebApi(builder.Configuration.GetSection("AzureAd"));

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(gen =>
{
  gen.SwaggerDoc("v1", new OpenApiInfo
  {
    Version = "v1",
    Title = "Teste - Carrefour API",
    Description = "Um Web API em ASP.NET Core Web API para gerenciamento de fluxo de caixa. � necess�rio voc� estar executando o RabbitMQ na porta padr�o para retornar extrato de conta!",    
    Contact = new OpenApiContact
    {
      Name = "Davi Lima Alves",
      Url = new Uri("https://linkedin.com/in/davilalves")
    }    
  });

  var xmlFileName = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
  gen.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, xmlFileName));
});
builder.Services.AddDbContext<FluxoCaixaContext>();

builder.Services.AddScoped<IRepositoryAccount, RepositoryAccount>();
builder.Services.AddScoped<IRepositoryBalance, RepositoryBalance>();
builder.Services.AddScoped<IRepositoryExtract, RepositoryExtract>();
builder.Services.AddScoped<IRepositoryRecord, RepositoryRecord>();
builder.Services.AddScoped<IRepositoryFluxoCaixa, RepositoryFluxoCaixa>();

builder.Services.AddScoped<IApplicationServiceAccount, AccountApplicationService>();
builder.Services.AddScoped<IApplicationServiceBalance, BalanceApplicationService>();
builder.Services.AddScoped<IApplicationServiceRecord, RecordApplicationService>();
builder.Services.AddScoped<IApplicationServiceFluxoCaixa, FluxoCaixaApplicationService>();
builder.Services.AddScoped<IConsolidadoQueueApplicationService, ConsolidadoQueueApplicationService>();

builder.Services.AddAutoMapper(System.Reflection.Assembly.Load("Application"));
builder.Services.AddApplicationInsightsTelemetry();

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
