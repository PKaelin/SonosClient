# Sonos Client - Proof Of Concept (POC)
This project is a proof of concept for playing `.wav` or `.mp3` audio files on a Sonos speaker.  
Sonos speakers cannot receive audio files directly. Instead, you must provide a URL for the speaker to fetch and play the audio.  

Tested with:
- Sonos Play 1
- Sonos Software Generation 2


## Disclaimer
This project is intended as a proof of concept only.  
**Exception handling, configuration management, logging, code comments, and overall project structure have not been designed for production use.**



## Sonos speakers and Sonos API
There is no comprehensive official documentation for the local Sonos API. 
However, each speaker exposes service description endpoints that can be used to discover available services and API definitions.

To see just the status of your Sonos speaker use:
```
http://[SonosIP]:1400/status
```

To see a set of service descriptions use:
```
http://[SonosIP]:1400/xml/device_description.xml
```

To see details about e.g. the service type "urn:schemas-upnp-org:service:AVTransport:1" use:
```
http://[SonosIP]:1400/xml/AVTransport1.xml
```


## Running this POC
Before running this solution, consider the following:
- Sonos speaker should have a fixed IP otherwise this POC works randomly.
- Sonos speaker and this POC should either run in the same network or Sonos speaker should have access to this API endpoint and vise versa (configure Network/Firewall).
- Environment variables (SONOS_HOST_ADDRESS/AUDIO_HOST_ADDRESS) can be configured through launchSettings.json, docker run or docker compose.
- In Microsoft Visual Studio, this POC uses the Docker port 32788 as defined in launchSettings.json.
- API testing can be performed using the Sonos.WebApi.http REST client file.


## Sample files 
This POC includes only two sample audio files:
- test.mp3
- test.wav


## Sonos WAV File Format Specifications
- Sample Rate: 44.1 kHz (44100 Hz) maximum.
- Bit Depth: 16-bit maximum.
- Channels: Mono or Stereo.
- Encoding: Uncompressed PCM format
