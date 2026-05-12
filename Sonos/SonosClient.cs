using System.Text;
//-----------------------------------------------------------------------------
// Note: This is only a proof of concept
//-----------------------------------------------------------------------------

namespace Sonos
{
    /// <summary>
    /// Used to interact with a local Sonos speaker.
    /// </summary>
    /// <remarks>
    /// Speakers tested with this client is Sonos Play 1, Software Generation 2
    /// </remarks>
    public class SonosClient : ISonosClient
    {
        /// <summary>
        /// Sonos host address.
        /// </summary>
        /// <example>
        /// http://10.0.0.250:1400
        /// </example>
        /// <remarks>
        /// Make sure to assign the Sonos speaker a fixed IP otherwise this might work randomly.
        /// </remarks>
        private readonly string sonosHostAddress;


        /// <summary>
        /// Audio source endpoint.
        /// </summary>
        /// <example>
        /// http://10.0.0.251:1500/api/audio
        /// </example>
        /// <remarks>
        /// Sonos API requires the URI to end either in .wav or .mp3 otherwise it raises an error.
        /// </remarks>
        private readonly string audioHostEndpoint;


        /// <summary>
        /// Constructor of the SonosClient class
        /// </summary>
        /// <param name="sonosHostAddress">Sonos host address. E.g. http://10.0.0.250:1400</param>
        /// <param name="audioHostEndpoint">Audio source endpoint. E.g. http://10.0.0.251:1500/api/audio</param>
        /// <exception cref="ArgumentNullException">Throws arguement exception when parameters are null or empty</exception>
        public SonosClient(string sonosHostAddress, string audioHostEndpoint)
        {
            if (string.IsNullOrEmpty(sonosHostAddress) || string.IsNullOrEmpty(audioHostEndpoint))
            {
                throw new ArgumentNullException("Parameters sonosHostAddress or audioHostEndpoint or both not set");
            }

            this.sonosHostAddress = sonosHostAddress;
            this.audioHostEndpoint = audioHostEndpoint;
        }


        /// <summary>
        /// Sends a media URI to the Sonos speaker to specify what content to play.
        /// </summary>
        /// <param name="file">File endpoint that gets appended to the audio host endpoint.</param>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        public async Task PostAudioFile(string file)
        {
            string uri = sonosHostAddress + "/MediaRenderer/AVTransport/Control";
            string audioUri = $"{audioHostEndpoint}/{Uri.EscapeDataString(file)}";
            string soapAction = "urn:schemas-upnp-org:service:AVTransport:1#SetAVTransportURI";
            string xmlBody = @$"<?xml version=""1.0"" encoding=""utf-8""?>
<s:Envelope xmlns:s=""http://schemas.xmlsoap.org/soap/envelope/""
  s:encodingStyle=""http://schemas.xmlsoap.org/soap/encoding/"">
  <s:Body>    
    <u:SetAVTransportURI xmlns:u=""urn:schemas-upnp-org:service:AVTransport:1"">
      <InstanceID>0</InstanceID>      
      <CurrentURI>{audioUri}</CurrentURI>
      <CurrentURIMetaData></CurrentURIMetaData>
    </u:SetAVTransportURI>
  </s:Body>
</s:Envelope>";

            HttpClient client = new HttpClient();
            StringContent content = new StringContent(xmlBody, Encoding.UTF8, "text/xml");
            content.Headers.Add("SOAPACTION", soapAction);
            HttpResponseMessage msg = await client.PostAsync(uri, content);

            msg.EnsureSuccessStatusCode();
        }


        /// <summary>
        /// Sends a volume command to the Sonos speaker to control playback volume.
        /// </summary>
        /// <param name="volume">Volume range from 0 (quiet) to 100 (loud)</param>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        /// <exception cref="ArgumentOutOfRangeException">Throws exception when volume is smaller than 0 or higher than 100.</exception>
        public async Task PostAudioVolume(int volume)
        {
            if (volume < 0 || volume > 100)
            {
                throw new ArgumentOutOfRangeException("Volume must be between 0 and 100");
            }

            string uri = sonosHostAddress + "/MediaRenderer/RenderingControl/Control";
            string soapAction = "urn:schemas-upnp-org:service:RenderingControl:1#SetVolume";
            string xmlBody = @$"<s:Envelope xmlns:s=""http://schemas.xmlsoap.org/soap/envelope/"">
  <s:Body>
     <u:SetVolume xmlns:u=""urn:schemas-upnp-org:service:RenderingControl:1"">      
      <InstanceID>0</InstanceID>      
      <Channel>Master</Channel>      
      <DesiredVolume>{volume}</DesiredVolume>
      </u:SetVolume>
 </s:Body>
</s:Envelope>";

            HttpClient client = new HttpClient();
            StringContent content = new StringContent(xmlBody, Encoding.UTF8, "text/xml");
            content.Headers.Add("SOAPACTION", soapAction);
            HttpResponseMessage msg = await client.PostAsync(uri, content);

            msg.EnsureSuccessStatusCode();
        }


        /// <summary>
        /// Sends a play command to the Sonos speaker to start media playback.
        /// </summary>
        /// <param name="speed">Speed to play the audio file.</param>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        public async Task PostAudioPlay(int speed = 1)
        {
            string uri = sonosHostAddress + "/MediaRenderer/AVTransport/Control";
            string soapAction = "urn:schemas-upnp-org:service:AVTransport:1#Play";
            string xmlBody = @$"<?xml version=""1.0"" encoding=""utf-8""?>
<s:Envelope xmlns:s=""http://schemas.xmlsoap.org/soap/envelope/""
  s:encodingStyle=""http://schemas.xmlsoap.org/soap/encoding/"">
  <s:Body>
    <u:Play xmlns:u=""urn:schemas-upnp-org:service:AVTransport:1"">
      <InstanceID>0</InstanceID>
      <Speed>{speed}</Speed>
    </u:Play>
  </s:Body>
</s:Envelope>";

            HttpClient client = new HttpClient();
            StringContent content = new StringContent(xmlBody, Encoding.UTF8, "text/xml");
            content.Headers.Add("SOAPACTION", soapAction);
            HttpResponseMessage msg = await client.PostAsync(uri, content);

            msg.EnsureSuccessStatusCode();
        }

    }
}
