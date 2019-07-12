using System;
using System.Threading.Tasks;
using TextToSpeech.Api.Application.Rest;
using TextToSpeech.Api.Domain.Entities;

namespace TextToSpeech.Api.Application.Services
{
    public class SpeechService : ISpeechService
    {
        readonly CognitiveAuthRest iCognitiveAuthRest;
        readonly CognitiveRest iCognitiveRest;
        readonly IContainerService iContainerService;

        public SpeechService(CognitiveAuthRest iCognitiveAuthRest,
            CognitiveRest iCognitiveRest,
            IContainerService iContainerService)
        {
            this.iCognitiveAuthRest = iCognitiveAuthRest ??
                throw new ArgumentNullException(nameof(iCognitiveAuthRest));

            this.iContainerService = iContainerService ??
                throw new ArgumentNullException(nameof(iContainerService));

            this.iCognitiveRest = iCognitiveRest ??
                throw new ArgumentNullException(nameof(iCognitiveRest));
        }

        public async Task<string> CreateAndSaveAudioAsync(string text)
        {
            Validate();

            var result = await this.UpdateAndSaveAudioAsync(Guid.NewGuid(), text);
            return result;

            void Validate()
            {
                if (string.IsNullOrWhiteSpace(text))
                    throw new ArgumentNullException(text);
            }
        }

        public async Task<string> GetUrl(Guid id)
        {
            var result = default(string);
            var fileName = $"{id}.wav";
            var uri = await this.iContainerService.GetUrlAsync(fileName);
            if (uri != null)
            {
                result = uri.AbsoluteUri;
            }
            return result;
        }

        public async Task<string> UpdateAndSaveAudioAsync(Guid id, string text)
        {
            var result = default(string);
            var speech = new Speech
            {
                Gender = Gender.Female,
                Locale = "en-US",
                Name = "Microsoft Server Speech Text to Speech Voice (en-US, ZiraRUS)",
                Text = text
            };

            var token = await this.iCognitiveAuthRest.GetTokenAsync();

            using (var stream = await this.iCognitiveRest.GetAudioAsync(speech, token))
            {
                var fileName = $"{id}.wav";
                var uri = await this.iContainerService.UploadFromStreamAsync(fileName, stream);
                result = uri.AbsoluteUri;
            }

            return result;
        }
    }
}
