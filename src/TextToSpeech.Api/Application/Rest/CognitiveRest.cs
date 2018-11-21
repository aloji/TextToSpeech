using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using TextToSpeech.Api.Domain.Entities;

namespace TextToSpeech.Api.Application.Rest
{
    public class CognitiveRest
    {
        readonly HttpClient client;
        public CognitiveRest(HttpClient client)
        {
            this.client = client;
        }

        public async Task<Stream> GetAudioAsync(Speech speech, string token)
        {
            if (speech == null)
            {
                throw new ArgumentNullException(nameof(speech));
            }

            var ssml = speech.SSML;

            this.client.DefaultRequestHeaders
                .Add("X-Microsoft-OutputFormat", "raw-16khz-16bit-mono-pcm");

            this.client.DefaultRequestHeaders.Authorization 
                = new AuthenticationHeaderValue("Bearer", token);

            var response = await this.client.PostAsync("", 
                new StringContent(ssml, Encoding.UTF8, "application/ssml+xml"));

            response.EnsureSuccessStatusCode();

            var result = await response.Content.ReadAsStreamAsync();
            return result;
        }
    }
}
