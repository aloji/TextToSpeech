using System;
using System.Collections.Generic;
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
        readonly IEnumerable<IReplaceService> iReplaceServices;

        public SpeechService(CognitiveAuthRest iCognitiveAuthRest,
            CognitiveRest iCognitiveRest,
            IContainerService iContainerService,
            IEnumerable<IReplaceService> iReplaceServices)
        {
            this.iCognitiveAuthRest = iCognitiveAuthRest ??
                throw new ArgumentNullException(nameof(iCognitiveAuthRest));

            this.iCognitiveRest = iCognitiveRest ??
                throw new ArgumentNullException(nameof(iCognitiveRest));

            this.iContainerService = iContainerService ??
               throw new ArgumentNullException(nameof(iContainerService));

            this.iReplaceServices = iReplaceServices ??
               throw new ArgumentNullException(nameof(iReplaceServices));
        }

        public async Task<string> CreateAndSaveAudioAsync(string text)
        {
            var result = await this.UpdateAndSaveAudioAsync(Guid.NewGuid(), text);
            return result;
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
            Validate();

            var result = default(string);

            var textToSpeech = this.Replace(text);

            var speech = new Speech
            {
                Gender = Gender.Female,
                Locale = "en-US",
                Name = "Microsoft Server Speech Text to Speech Voice (en-US, ZiraRUS)",
                Text = textToSpeech
            };

            var token = await this.iCognitiveAuthRest.GetTokenAsync();

            using (var stream = await this.iCognitiveRest.GetAudioAsync(speech, token))
            {
                var fileName = $"{id}.wav";
                var uri = await this.iContainerService.UploadFromStreamAsync(fileName, stream);
                result = uri.AbsoluteUri;
            }

            return result;

            void Validate()
            {
                if (string.IsNullOrWhiteSpace(text))
                    throw new ArgumentNullException(nameof(text));
                if (id == Guid.Empty)
                    throw new ArgumentNullException(nameof(id));
            }
        }

        private string Replace(string text)
        {
            var result = text;
            foreach (var item in this.iReplaceServices)
            {
                result = item.Replace(result);
            }
            return result;
        }
    }
}
