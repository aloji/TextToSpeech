using System;
using System.Threading.Tasks;

namespace TextToSpeech.Api.Application.Services
{
    public interface ISpeechService
    {
        Task<string> CreateAndSaveAudioAsync(string text);
        Task<string> UpdateAndSaveAudioAsync(Guid id, string text);
        Task<string> GetUrl(Guid id);
    }
}
