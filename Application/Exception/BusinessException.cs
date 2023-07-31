using System.Diagnostics.CodeAnalysis;

namespace Application.Exception
{
  [ExcludeFromCodeCoverage]
  public class BusinessException : System.Exception
  {    
    public BusinessException()
    {

    }

    public BusinessException(string message) : base(message)
    {

    }    
  }
}
