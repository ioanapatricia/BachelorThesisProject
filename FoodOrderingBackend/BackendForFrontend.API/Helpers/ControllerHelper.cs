using System;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace BackendForFrontend.API.Helpers
{
    public class ControllerHelper : ControllerBase, IControllerHelper
    {
        public async Task<IActionResult> ParseActionResult(HttpResponseMessage incomingHttpResponseMessage, string createdAtRouteLocation = null)
        {
            var content = await incomingHttpResponseMessage.Content.ReadAsStringAsync();

            return (int) incomingHttpResponseMessage.StatusCode switch
            {           
                200 => Ok(JsonConvert.DeserializeObject(content)),  
                201 => await CustomCreatedAtRouteResult(incomingHttpResponseMessage, createdAtRouteLocation),
                204 => NoContent(),
                400 => BadRequest(content),
                401 => Unauthorized(),  
                403 => Forbid(),
                404 => NotFound(content),
                405 => StatusCode(StatusCodes.Status405MethodNotAllowed, content),
                409 => Conflict(content),   
                412 => StatusCode(StatusCodes.Status412PreconditionFailed, content),
                500 => StatusCode(StatusCodes.Status500InternalServerError, content),
                _ => StatusCode(StatusCodes.Status500InternalServerError, "unknown exception occurred")
            };
        }


        public async Task<IActionResult> ParseImageResult(HttpResponseMessage incomingHttpResponseMessage, HttpResponse outgoingHttpResponse)
        {
            if ((int)incomingHttpResponseMessage.StatusCode == 404)
            {
                var content = await incomingHttpResponseMessage.Content.ReadAsStringAsync();
                return NotFound(content);
            }


            var file = await incomingHttpResponseMessage.Content.ReadAsStreamAsync();


            var cd = new System.Net.Mime.ContentDisposition
            {
                Inline = true
            };  

            outgoingHttpResponse.Headers.Add("Content-Disposition", cd.ToString());

            var contentType = incomingHttpResponseMessage.Content.Headers.ContentType.ToString();

            return File(file, contentType);
        }


        public StringContent GetObjectAsStringContent(object obj)   
            => new(JsonConvert.SerializeObject(obj), Encoding.UTF8, "application/json");


        private async Task<IActionResult> CustomCreatedAtRouteResult(HttpResponseMessage incomingHttpResponseMessage, string createdAtRouteLocation)
        {
            var locationHeader = incomingHttpResponseMessage.Headers.GetValues("Location")
                .FirstOrDefault();

            if (locationHeader == null)
                return StatusCode(StatusCodes.Status500InternalServerError, "Error in BFF.API. Calling API did not return location header.");

            var idPosition = locationHeader.LastIndexOf("/", StringComparison.Ordinal) + 1;
            var id = locationHeader.Substring(idPosition, locationHeader.Length - idPosition);

            var content = await incomingHttpResponseMessage.Content.ReadAsStringAsync();

            return CreatedAtRoute(createdAtRouteLocation, new { id }, JsonConvert.DeserializeObject(content));
        }
    }
}
