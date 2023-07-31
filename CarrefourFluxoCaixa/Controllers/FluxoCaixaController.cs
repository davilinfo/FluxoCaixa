using Application.Interfaces;
using Application.Models.Request;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

namespace CarrefourFluxoCaixa.Controllers
{
  //[Authorize]
  [ApiController]
  [Route("[controller]")]
  //[RequiredScope(RequiredScopesConfigurationKey = "AzureAd:Scopes")]
  public class FluxoCaixaController : ControllerBase
  {
    
    private readonly ILogger<FluxoCaixaController> _logger;
    private readonly IApplicationServiceAccount _applicationServiceAccount;
    private readonly IApplicationServiceBalance _applicationServiceBalance;
    private readonly IApplicationServiceRecord _applicationServiceRecord;

    public FluxoCaixaController(
      ILogger<FluxoCaixaController> logger, 
      IApplicationServiceAccount applicationServiceAccount,
      IApplicationServiceBalance applicationServiceBalance,
      IApplicationServiceRecord applicationServiceRecord
      )
    {
      _logger = logger;
      _applicationServiceAccount = applicationServiceAccount;
      _applicationServiceBalance = applicationServiceBalance;
      _applicationServiceRecord = applicationServiceRecord;
    }

    /// <summary>
    /// Cria conta de fluxo de caixa
    /// </summary>
    /// <param name="request">Name e email</param>
    /// <returns></returns>
    [HttpPost("CriaContaFluxo")]
    public async Task<IActionResult> CreateAccount([FromBody] AccountRequest request)
    {
      _logger.LogInformation($"Create account request: {JsonSerializer.Serialize(request)}");
      try
      {
        if (ModelState.IsValid)
        {          
          var result = await _applicationServiceAccount.AddAsync(request);
          return Created($"fluxocaixa/getconta/{request.Email}", result);
        }
        foreach (var item in ModelState.Values)
        {
          foreach (var erro in item.Errors)
          {
            _logger.LogError($"Date: {DateTime.UtcNow}, Error: requisição inválida {erro.ErrorMessage}");
          }
        }
        return BadRequest(ModelState);

      }
      catch(Exception e) {
        _logger.LogError($"{e.Message}", e);
        var internalServerError = new JsonResult($"Aconteceu o seguinte erro: {e.Message}");
        internalServerError.StatusCode = 500;
        return internalServerError;
      }      
    }
    
    /// <summary>
    /// Para retornar uma conta informe um email
    /// </summary>
    /// <param name="request">Email</param>
    /// <returns></returns>
    [HttpGet("GetConta", Name = "GetAccount")]
    public IActionResult GetAccount([FromQuery]GetAccountRequest request)
    {
      _logger.LogInformation($"Get account request: {JsonSerializer.Serialize(request)}");
      try
      {
        if (ModelState.IsValid)
        {
          var result = _applicationServiceAccount.GetByEmail(request.Email);

          if (result == null)
          {
            return NotFound();
          }
          return Ok(result);
        }
        foreach (var item in ModelState.Values)
        {
          foreach (var erro in item.Errors)
          {
            _logger.LogError($"Date: {DateTime.UtcNow}, Error: requisição inválida {erro.ErrorMessage}");
          }
        }
        return BadRequest(ModelState);        
      }
      catch (Exception e)
      {
        _logger.LogError($"{e.Message}", e);
        var internalServerError = new JsonResult($"Aconteceu o seguinte erro: {e.Message}");
        internalServerError.StatusCode = 500;
        return internalServerError;
      }
    }
    
    /// <summary>
    /// Para retornar o saldo informe o número da conta e o email da conta
    /// </summary>
    /// <param name="request">AccountNumber e Email</param>
    /// <returns></returns>
    [HttpGet("GetSaldo",Name = "GetBalance")]
    public async Task<IActionResult> GetBalance([FromQuery]GetBalanceRequest request)
    {
      _logger.LogInformation($"Get balance request: {JsonSerializer.Serialize(request)}");
      try
      {
        if (ModelState.IsValid)
        {
          var result = await _applicationServiceBalance.GetByAccountId(Guid.Parse(request.AccountId));
          if (result == null)
          {
            return NotFound("Não foi realizada nenhuma movimentação ainda!");
          }
          return Ok(result);
        }
        foreach (var item in ModelState.Values)
        {
          foreach (var erro in item.Errors)
          {
            _logger.LogError($"Date: {DateTime.UtcNow}, Error: requisição inválida {erro.ErrorMessage}");
          }
        }
        return BadRequest(ModelState);
      }
      catch (Exception e)
      {
        _logger.LogError($"{e.Message}", e);
        var internalServerError = new JsonResult($"Aconteceu o seguinte erro: {e.Message}");
        internalServerError.StatusCode = 500;
        return internalServerError;
      }
    }
   
    /// <summary>
    /// Para retornar o extrato diário informe o dia mês e ano no formato ddmmaaaa
    /// </summary>
    /// <param name="request">Account number, email e dia</param>
    /// <returns></returns>
    [HttpGet("GetExtrato", Name = "GetExtract")]
    public async Task<IActionResult> GetExtract(GetExtractRequest request)
    {
      _logger.LogInformation($"Get extract request: {JsonSerializer.Serialize(request)}");
      try
      {
        if (ModelState.IsValid)
        {
          return Ok("");
        }
        foreach (var item in ModelState.Values)
        {
          foreach (var erro in item.Errors)
          {
            _logger.LogError($"Date: {DateTime.UtcNow}, Error: requisição inválida {erro.ErrorMessage}");
          }
        }
        return BadRequest(ModelState);
      }
      catch (Exception e)
      {
        _logger.LogError($"{e.Message}", e);
        var internalServerError = new JsonResult($"Aconteceu o seguinte erro: {e.Message}");
        internalServerError.StatusCode = 500;
        return internalServerError;
      }
    }
    
    /// <summary>
    /// Para realizar um lançamento informe o número da conta, email, descrição do lançamento, tipo C ou D (Crédito ou Débito), valor
    /// </summary>
    /// <param name="request">AccountNumber = número conta, Email = email, Description = descrição lançamento, RecordType = tipo, Value = valor</param>
    /// <returns></returns>
    [HttpPost("CriaLancamento",Name = "CreateRecord")]
    public async Task<IActionResult> CreateRecord([FromBody]RecordRequest request)
    {
      _logger.LogInformation($"Record request: {JsonSerializer.Serialize(request)}");
      try
      {
        if (ModelState.IsValid)
        {
          var model = await _applicationServiceRecord.AddAsync(request);
          return Created($"fluxocaixa/getsaldo/{model?.IdAccountNavigation?.Id}", model);
        }
        foreach (var item in ModelState.Values)
        {
          foreach (var erro in item.Errors)
          {
            _logger.LogError($"Date: {DateTime.UtcNow}, Error: requisição inválida {erro.ErrorMessage}");
          }
        }
        return BadRequest(ModelState);
      }
      catch (Exception e)
      {
        _logger.LogError($"{e.Message}", e);
        var internalServerError = new JsonResult($"Aconteceu o seguinte erro: {e.Message}");
        internalServerError.StatusCode = 500;
        return internalServerError;
      }
    }
  }
}