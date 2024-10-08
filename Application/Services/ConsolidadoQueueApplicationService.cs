﻿using Application.Interfaces;
using Application.Models.Request;
using Application.Models.Response;
using Application.Models.ViewModel;
using AutoMapper;
using Domain.Contract;
using Microsoft.Extensions.Configuration;
using RabbitMQ.Client;
using System.Diagnostics.CodeAnalysis;
using System.Text;

namespace Application.Services
{
  [ExcludeFromCodeCoverage]
  public class ConsolidadoQueueApplicationService : IConsolidadoQueueApplicationService
  {
    private IRepositoryExtract _repositoryExtract;
    private IMapper _mapper;
    private IConfiguration _config;    
    public ConsolidadoQueueApplicationService(IMapper mapper, IRepositoryExtract repositoryExtract, IConfiguration configuration)
    {
      _repositoryExtract = repositoryExtract;
      _mapper = mapper;
      _config = configuration;
    }

    public async Task<ConsolidadoResponse> GenerateConsolidado(GetExtractRequest request)
    {
      var consolidadoResponse = await Generate(request);

      if (consolidadoResponse != null)
      {
        if (bool.Parse(_config.GetSection("AMQP:Activated").Value) == true)
        {
          SendToFluxoQueue(consolidadoResponse);
        }
        return consolidadoResponse;
      }

      return null;
    }     
    
    private async Task<ConsolidadoResponse> Generate(GetExtractRequest request)
    {
      var consolidadoResponse = new ConsolidadoResponse();
      consolidadoResponse.Id = Guid.NewGuid();
      var listRecords = new List<RecordViewModel>();
      var date = new DateTime(int.Parse(request.DiaMesAno.Substring(4)), int.Parse(request.DiaMesAno.Substring(2,2)), int.Parse(request.DiaMesAno.Substring(0,2)));

      var records = _repositoryExtract.All().Where(e => e.IdAccountNavigation.Id == Guid.Parse(request.AccountId)
        && e.Created.Date == date).ToList();

      if (records.Count > 0)
      {
        consolidadoResponse.Created = records.Last().Created;
        consolidadoResponse.BalanceValue = records.Last().Value;
        foreach (var record in records.Select(e => e.IdRecordNavigation))
        {
          var recordViewModel = _mapper.Map<RecordViewModel>(record);
          listRecords.Add(recordViewModel);
        }
        consolidadoResponse.Records = listRecords;
        consolidadoResponse.IdAccount = _mapper.Map<AccountViewModel>(records.First().IdAccountNavigation);
      }
      else
      {
        var nearestRecord = _repositoryExtract.All().Where(e => e.IdAccountNavigation.Id == Guid.Parse(request.AccountId)
        && e.Created.Date < date).OrderByDescending(o => o.Created).FirstOrDefault();

        if (nearestRecord != null)
        {
          records = _repositoryExtract.All().Where(e => e.IdAccountNavigation.Id == Guid.Parse(request.AccountId)
          && e.Created.Date == nearestRecord.Created.Date).ToList();

          consolidadoResponse.Created = records.Last().Created;
          consolidadoResponse.BalanceValue = records.Last().Value;
          foreach (var record in records.Select(e => e.IdRecordNavigation))
          {
            var recordViewModel = _mapper.Map<RecordViewModel>(record);
            listRecords.Add(recordViewModel);
          }
          consolidadoResponse.Records = listRecords;
          consolidadoResponse.IdAccount = _mapper.Map<AccountViewModel>(records.First().IdAccountNavigation);
        }
        else
        {
          return null;
        }
      }
      return consolidadoResponse;
    }

    private void SendToFluxoQueue(ConsolidadoResponse consolidadoResponse)
    {      
      var amqpPort = _config.GetSection("AMQP:Port").Value != null ? int.Parse(_config.GetSection("AMQP:Port").Value) : 5672;
      var factory = new ConnectionFactory() { HostName = _config.GetSection("AMQP:Hostname").Value, UserName = "guest", Password = "guest", Port = amqpPort };

      using (var connection = factory.CreateConnection())
      {
        using (var channel = connection.CreateModel())
        {
          channel.QueueDeclare(queue: "queueFluxo",
                              durable: false,
                              exclusive: false,
                              autoDelete: false,
                              arguments: null);          
          var serialized = System.Text.Json.JsonSerializer.Serialize<ConsolidadoResponse>(consolidadoResponse);
          var complexBody = Encoding.UTF8.GetBytes(serialized);
          channel.BasicPublish(exchange: "",
                            routingKey: "queueFluxo",
                            basicProperties: null,
                            body: complexBody);
        }
        connection.Close();
      }
    }
  }
}
