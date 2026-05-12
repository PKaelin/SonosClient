//-----------------------------------------------------------------------------
// Note: This is only a proof of concept
//-----------------------------------------------------------------------------
namespace Sonos
{
    public interface ISonosClient
    {
        /// <summary>
        /// Sends a media URI to the Sonos speaker to specify what content to play.
        /// </summary>
        /// <param name="file">File endpoint that gets appended to the audio host endpoint.</param>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        Task PostAudioFile(string file);


        /// <summary>
        /// Sends a volume command to the Sonos speaker to control playback volume.
        /// </summary>
        /// <param name="volume">Volume in a range from 0 (quiet) to 100 (loud)</param>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        /// <exception cref="ArgumentOutOfRangeException">Throws exception when volume is smaller than 0 or higher than 100.</exception>
        Task PostAudioVolume(int volume);


        /// <summary>
        /// Sends a play command to the Sonos speaker to start media playback.
        /// </summary>
        /// <param name="speed">Speed to play the audio file.</param>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        Task PostAudioPlay(int speed = 1);

    }
}
