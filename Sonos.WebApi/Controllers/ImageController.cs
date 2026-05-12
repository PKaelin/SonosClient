using Microsoft.AspNetCore.Mvc;
//-----------------------------------------------------------------------------
// Note: This is only a proof of concept
//-----------------------------------------------------------------------------

namespace Sonos.WebApi.Controllers
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class ImageController : ControllerBase
    {
        // For POC only test.wav and test.mp3 are available
        [HttpGet("{filename}")]
        public async Task<IActionResult> GetItem(string filename)
        {
            if (string.IsNullOrEmpty(filename))
            {
                throw new ArgumentNullException("Parameter filename cannot be null");
            }

            if (System.IO.File.Exists(filename) == false)
            {
                return NotFound();
            }

            FileInfo fileInfo = new FileInfo(filename);
            string contentType;

            if (fileInfo.Extension.ToLower() == ".wav")
            {
                contentType = "audio/wav";
            }
            else if (fileInfo.Extension.ToLower() == ".mp3")
            {
                contentType = "audio/mp3";
            }
            else
            {
                return BadRequest("File type must be either wav or mp3");
            }

            FileStream fileStream = System.IO.File.OpenRead(filename);

            return File(fileStream, contentType, filename, enableRangeProcessing: true);
        }    
    }
}
