using MediatR;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using PdfConverter.Commands;
using PdfConverter.Domain;
using System.Text;

namespace PdfConverter.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ConverterController : ControllerBase
    {
        private readonly IMediator _mediator;
        public ConverterController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost]
        [Route("[action]")]
        public async Task<byte[]> Convert()
        {
            using (StreamReader reader = new StreamReader(Request.Body, Encoding.UTF8))
            {
                string body = await reader.ReadToEndAsync() ?? throw new ArgumentException("body is empty");
                byte[] bytes = JsonConvert.DeserializeObject<byte[]>(body);
                return await _mediator.Send(new ConvertToPdfCommand(bytes));
            }
        }
    }

}
