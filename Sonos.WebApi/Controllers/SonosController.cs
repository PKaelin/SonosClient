using Microsoft.AspNetCore.Mvc;
using Sonos.WebApi.Dto;
//-----------------------------------------------------------------------------
// Note: This is only a proof of concept
//-----------------------------------------------------------------------------

namespace Sonos.WebApi.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class SonosController : ControllerBase
    {
        private readonly string? sonosHostAddress;
        private readonly string? audioEndpointAddress;

        public SonosController() 
        {
            // Environment variables can be set in launchSettings.json, docker run or docker compose
            sonosHostAddress = Environment.GetEnvironmentVariable("SONOS_HOST_ADDRESS");
            audioEndpointAddress = Environment.GetEnvironmentVariable("AUDIO_ENDPOINT_ADDRESS");

            if(string.IsNullOrEmpty(sonosHostAddress) || string.IsNullOrEmpty(audioEndpointAddress))
            {
                throw new ArgumentNullException("Either SONOS_HOST_ADDRESS or AUDIO_ENDPOINT_ADDRESS or both are not set.");
            }

            if(sonosHostAddress.StartsWith("http://[SONOS_ADDRESS]") || (audioEndpointAddress.StartsWith("http://[AUDIO_ADDRESS]")))
            {
                throw new ArgumentException("SONOS_HOST_ADDRESS and/or AUDIO_ENDPOINT_ADDRESS need to be set with their actual values.");
            }
        }


        [HttpPost]
        public async Task<ActionResult> Process(ProcessTtsDto message)
        {
            // TODO: Inject interface
            ISonosClient sonos = new SonosClient(sonosHostAddress!, audioEndpointAddress!);

            // TODO: Create TTS file like: await textToSpeech.CreateTtsFile(Path.Combine(dataFolder, message.Filename), message.Text);
            await sonos.PostAudioFile(message.Filename);
            await sonos.PostAudioVolume(40);
            await sonos.PostAudioPlay();

            // TODO: The web.api currently does not return an error when this service cannot communicate with the Sonos speakers due to network or firewall issues.

            return Ok();
        }      
    }
}
