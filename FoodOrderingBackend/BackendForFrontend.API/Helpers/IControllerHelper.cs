using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BackendForFrontend.API.Helpers
{
    public interface IControllerHelper
    {
        Task<IActionResult> ParseActionResult(HttpResponseMessage incomingHttpResponseMessage, string createdAtRouteLocation = null);
        Task<IActionResult> ParseImageResult(HttpResponseMessage incomingHttpResponseMessage, HttpResponse outgoingHttpResponse);
        StringContent GetObjectAsStringContent(object obj);
    }
}
