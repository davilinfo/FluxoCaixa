using Application.Interfaces;
using Application.Models.Request;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics.CodeAnalysis;
using System.Text.Json;

namespace CarrefourFluxoCaixa.Controllers
{
  /// <summary>
  /// Controller responsável por criar conta fluxo de caixa a partir de um email
  /// </summary>
  [ExcludeFromCodeCoverage]
  //[Authorize]
  [ApiController]
  [Route("[controller]")]
  [Produces("application/json")]
  //[RequiredScope(RequiredScopesConfigurationKey = "AzureAd:Scopes")]
  public class FluxoCaixaController : ControllerBase
  {
    
    private readonly ILogger<FluxoCaixaController> _logger;
    private readonly IApplicationServiceAccount _applicationServiceAccount;
    private readonly IApplicationServiceBalance _applicationServiceBalance;
    private readonly IApplicationServiceFluxoCaixa _applicationServiceFluxoCaixa;
    private readonly IConsolidadoQueueApplicationService _consolidadoQueueApplicationService;

    /// <summary>
    /// Injeção de dependência em construtor
    /// </summary>
    /// <param name="logger"></param>
    /// <param name="applicationServiceAccount"></param>
    /// <param name="applicationServiceBalance"></param>
    /// <param name="applicationServiceFluxoCaixa"></param>
    /// <param name="consolidadoQueueApplicationService"></param>
    public FluxoCaixaController(
      ILogger<FluxoCaixaController> logger, 
      IApplicationServiceAccount applicationServiceAccount,
      IApplicationServiceBalance applicationServiceBalance,
      IApplicationServiceFluxoCaixa applicationServiceFluxoCaixa,
      IConsolidadoQueueApplicationService consolidadoQueueApplicationService
      )
    {
      _logger = logger;
      _applicationServiceAccount = applicationServiceAccount;
      _applicationServiceBalance = applicationServiceBalance;
      _applicationServiceFluxoCaixa = applicationServiceFluxoCaixa;
      _consolidadoQueueApplicationService = consolidadoQueueApplicationService;
    }

    /// <summary>
    /// Cria conta de fluxo de caixa. Tudo inicia aqui, então crie uma conta de fluxo de caixa se ainda não criou
    /// </summary>
    /// <param name="request">Name e email</param>
    /// <returns></returns>
    [HttpPost("CriaContaFluxo")]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
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
    /// Para retornar uma conta informe um email (Observação: utilize o identificador da conta retornado nos métodos seguintes: id)
    /// </summary>
    /// <param name="request">Email</param>
    /// <returns></returns>
    [HttpGet("GetConta", Name = "GetAccount")]
    [ProducesResponseType(StatusCodes.Status200OK)]    
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
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
    /// Para retornar o saldo informe o identificador da conta e o email da conta
    /// </summary>
    /// <param name="request">AccountNumber e Email</param>
    /// <returns></returns>
    [HttpGet("GetSaldo",Name = "GetBalance")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetBalance([FromQuery]GetBalanceRequest request)
    {
      _logger.LogInformation($"Get balance request: {JsonSerializer.Serialize(request)}");
      try
      {
        Guid id;
        if (!Guid.TryParse(request.AccountId, out id))
        {
          ModelState.AddModelError("Guid", "Identificador em formato inválido => guid");
        }
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
    /// Para retornar o extrato diário informe o idenficador da conta, o dia mês e ano no formato ddmmaaaa
    /// </summary>
    /// <param name="request">Account number, email e dia</param>
    /// <returns></returns>
    [HttpGet("GetExtrato", Name = "GetExtract")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetExtract([FromQuery]GetExtractRequest request)
    {
      _logger.LogInformation($"Get extract request: {JsonSerializer.Serialize(request)}");
      try
      {
        Guid id;
        if (!Guid.TryParse(request.AccountId, out id))
        {
          ModelState.AddModelError("Guid", "Identificador em formato inválido => guid");
        }
        DateTime date;
        if (!DateTime.TryParse($"{int.Parse(request.DiaMesAno.Substring(4))}/{int.Parse(request.DiaMesAno.Substring(2, 2))}/{int.Parse(request.DiaMesAno.Substring(0, 2))}", out date))
        {
          ModelState.AddModelError("diamesano", "Data inválida");
        }

        if (ModelState.IsValid)
        {
          var result = await _consolidadoQueueApplicationService.GenerateConsolidado(request);   
          if (result == null)
          {
            return NotFound("Fluxo de Conta ainda sem movimentação");
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
    /// Para realizar um lançamento informe o número da conta, email, descrição do lançamento, type C ou D (C=Crédito, D=Débito), valor
    /// </summary>
    /// <param name="request">AccountNumber = número conta, Email = email, Description = descrição lançamento, RecordType = tipo, Value = valor</param>
    /// <returns></returns>
    [HttpPost("CriaLancamento",Name = "CreateRecord")]
    public async Task<IActionResult> CreateRecord([FromBody]RecordRequest request)
    {
      _logger.LogInformation($"Record request: {JsonSerializer.Serialize(request)}");
      try
      {
        Guid id;
        if (!Guid.TryParse(request.AccountId, out id))
        {
          ModelState.AddModelError("Guid", "Identificador em formato inválido => guid");
        }
        if (ModelState.IsValid)
        {
          var model = await _applicationServiceFluxoCaixa.AddAsync(request);
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