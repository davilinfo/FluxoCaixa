using Application.Models.Request;
using Application.Models.Response;

namespace Application.Interfaces
{
  public interface IConsolidadoQueueApplicationService
  {
    Task<ConsolidadoResponse> GenerateConsolidado(GetExtractRequest request);
  }
}
