using Microsoft.AspNetCore.Mvc;
using NSwag.Annotations;
using System;
using System.Threading.Tasks;
using WebApplication1.API.Services;
using WebApplication1.Data.DTOs;
using WebApplication1.DTO;

namespace WebApplication1.Controllers
{
    [Route("api/[controller]")]
    [SwaggerTag("Fs", Description = "Endpoint to handle all Fs related operations.")]
    [ApiController]
    public class FsController : Controller
    {
        private readonly IFSService _iFSService;
        public FsController(IFSService iFSService)
        {
            _iFSService = iFSService ?? throw new ArgumentNullException(nameof(iFSService));
        }

        [HttpGet("samplecontents")]
        public async Task<IActionResult> GetSampleContents()
        {
            var sampleContents = await _iFSService.GetSampleContents();

            if (sampleContents == null)
            {
                var response = new { IsSuccess = false, Comments = "No visit templates found." };
                return NotFound(response);
            }

            return Ok(sampleContents);
        }

        [HttpPost("saverecord")]
        public async Task<IActionResult> SaveRecord([FromBody] SampleContent newRecord)
        {
            Response response = null;

            var result= await _iFSService.SaveRecord(newRecord);

            if (string.IsNullOrEmpty(result))
            {
                response = new Response { Id = newRecord.Id, IsSuccess = false, Comments = "Unable to save record." };
                return Conflict(response);
            }

            response = new Response { Id = newRecord.Id, IsSuccess = true, Comments = "Record saved successfully" };

            return Ok(response);
        }
    }
}
