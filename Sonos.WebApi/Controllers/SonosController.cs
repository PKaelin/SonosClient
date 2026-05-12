using Microsoft.AspNetCore.Mvc;
//-----------------------------------------------------------------------------
// Note: This is only a proof of concept
//-----------------------------------------------------------------------------

namespace Sonos.WebApi.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class SonosController : ControllerBase
    {
        private readonly string sonosHostAddress;
        private readonly string audioHostAddress;


        public SonosController() 
        {
            // Environment variables can be set in launchSettings.json, docker run or docker compose
            string? sonosHost = Environment.GetEnvironmentVariable("SONOS_HOST_ADDRESS");
            string? audioHost = Environment.GetEnvironmentVariable("AUDIO_HOST_ADDRESS");

            if(string.IsNullOrEmpty(sonosHost) || string.IsNullOrEmpty(audioHost))
            {
                throw new ArgumentNullException("Either SONOS_HOST_ADDRESS or AUDIO_HOST_ADDRESS or both are not set.");
            }

            if(sonosHost.StartsWith("http://[SONOS_ADDRESS]") || (audioHost.StartsWith("http://[AUDIO_ADDRESS]")))
            {
                throw new ArgumentException("SONOS_HOST_ADDRESS and/or AUDIO_HOST_ADDRESS need to be set with their actual values.");
            }

            sonosHostAddress = sonosHost;
            audioHostAddress = audioHost;
        }


        [HttpPost]
        public async Task<ActionResult> ProcessWav()
        {
            string filename = "test.wav";
            string audioHostEndpoint = $"{audioHostAddress}/api/image/GetItem";            
            // TODO: Inject interface
            ISonosClient sonos = new SonosClient(sonosHostAddress, audioHostEndpoint);

            await sonos.PostAudioFile(filename);
            await sonos.PostAudioVolume(40);
            await sonos.PostAudioPlay();

            // TODO: The web.api currently does not return an error when this service cannot communicate with the Sonos speakers due to network or firewall issues.

            return Ok();
        }


        [HttpPost]
        public async Task<ActionResult> ProcessMp3()
        {
            string filename = "test.mp3";
            string audioHostEndpoint = $"{audioHostAddress}/api/image/GetItem";
            // TODO: Inject interface
            ISonosClient sonos = new SonosClient(sonosHostAddress, audioHostEndpoint);

            await sonos.PostAudioFile(filename);
            await sonos.PostAudioVolume(40);
            await sonos.PostAudioPlay();

            // TODO: The web.api currently does not return an error when this service cannot communicate with the Sonos speakers due to network or firewall issues.

            return Ok();
        }
    }
}
