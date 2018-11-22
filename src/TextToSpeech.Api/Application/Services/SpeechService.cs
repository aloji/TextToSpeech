using System;
using System.Threading.Tasks;
using TextToSpeech.Api.Application.Rest;
using TextToSpeech.Api.Domain.Entities;

namespace TextToSpeech.Api.Application.Services
{
    public interface ISpeechService
    {
        Task<string> CreateAndSaveAudioAsync(string text);
        Task<string> UpdateAndSaveAudioAsync(Guid id, string text);
        Task<string> GetUrl(Guid id);
    }

    public class SpeechService : ISpeechService
    {
        readonly CognitiveAuthRest cognitiveAuthRest;
        readonly CognitiveRest cognitiveRest;
        readonly IContainerService iContainerService;

        public SpeechService(CognitiveAuthRest cognitiveAuthRest,
            IContainerService iContainerService,
            CognitiveRest cognitiveRest)
        {
            this.cognitiveAuthRest = cognitiveAuthRest ??
                throw new ArgumentNullException(nameof(cognitiveAuthRest));

            this.iContainerService = iContainerService ??
                throw new ArgumentNullException(nameof(iContainerService));

            this.cognitiveRest = cognitiveRest ??
                throw new ArgumentNullException(nameof(cognitiveRest));
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
            var result = default(string);
            var speech = new Speech
            {
                Gender = Gender.Female,
                Locale = "en-US",
                Name = "Microsoft Server Speech Text to Speech Voice (en-US, ZiraRUS)",
                Text = text
            };

            var token = await this.cognitiveAuthRest.GetTokenAsync();

            using (var stream = await this.cognitiveRest.GetAudioAsync(speech, token))
            {
                var fileName = $"{id}.wav";
                var uri = await this.iContainerService.UploadFromStreamAsync(fileName, stream);
                result = uri.AbsoluteUri;
            }

            return result;
        }
    }
}
