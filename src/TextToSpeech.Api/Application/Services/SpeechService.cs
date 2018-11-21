using System;
using System.Threading.Tasks;
using TextToSpeech.Api.Application.Rest;
using TextToSpeech.Api.Domain.Entities;

namespace TextToSpeech.Api.Application.Services
{
    public interface ISpeechService
    {
        Task CreateAndSaveAudioAsync(string text);
    }

    public class SpeechService : ISpeechService
    {
        readonly ICognitiveAuthService iCognitiveAuthService;
        readonly CognitiveRest cognitiveRest;

        public SpeechService(ICognitiveAuthService iCognitiveAuthService,
            CognitiveRest cognitiveRest)
        {
            this.iCognitiveAuthService = iCognitiveAuthService ??
                throw new ArgumentNullException(nameof(iCognitiveAuthService));

            this.cognitiveRest = cognitiveRest ??
                throw new ArgumentNullException(nameof(cognitiveRest));
        }

        public async Task CreateAndSaveAudioAsync(string text)
        {
            var speech = new Speech
            {
                Gender = Gender.Male,
                Locale = "en-US",
                Name = "Microsoft Server Speech Text to Speech Voice (en-US, ZiraRUS)",
                Text = text
            };

            var token = await this.iCognitiveAuthService.GetTokenAsync();

            var stream = await this.cognitiveRest.GetAudioAsync(speech, token);
        }
    }
}
