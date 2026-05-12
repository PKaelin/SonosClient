namespace Sonos.WebApi.Dto
{
    public class ProcessTtsDto
    {
        /// <summary>
        /// File name that the TTS file is stored under.
        /// </summary>
        public string Filename { get; set; } = "test.wav";

        /// <summary>
        /// Text used to create the TTS file.
        /// </summary>
        // public required string Text { get; set; }

        /// <summary>
        /// Specific Sonos host address. If not defined the default address will be taken.
        /// </summary>
        public string? SonosHostAddress { get; set; }
    }
}
