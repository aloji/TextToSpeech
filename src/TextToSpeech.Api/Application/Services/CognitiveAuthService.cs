using System;
using System.Threading.Tasks;
using TextToSpeech.Api.Application.Rest;

namespace TextToSpeech.Api.Application.Services
{
    public interface ICognitiveAuthService
    {
        Task<string> GetTokenAsync();
    }

    public class CognitiveAuthService : ICognitiveAuthService
    {
        readonly CognitiveAuthRest cognitiveAuthRest;
        public CognitiveAuthService(CognitiveAuthRest cognitiveAuthRest)
        {
            this.cognitiveAuthRest = cognitiveAuthRest ??
                throw new ArgumentNullException(nameof(cognitiveAuthRest));
        }

        public async Task<string> GetTokenAsync()
        {
            var result = await this.cognitiveAuthRest.GetTokenAsync();
            return result;
        }
    }
}