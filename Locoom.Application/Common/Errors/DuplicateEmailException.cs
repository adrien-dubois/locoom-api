using System.Net;

namespace Locoom.Application.Common.Errors
{
    public class DuplicateEmailException : Exception, IServiceException
    {
        public HttpStatusCode StatusCode => HttpStatusCode.Conflict;

        public string ErrorMessage => "Cet e-mail est déjà enregistré sur un autre compte.";
    }
}
