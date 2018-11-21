using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using TextToSpeech.Api.Application.Services;

namespace TextToSpeech.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TextToSpeechController : ControllerBase
    {
        readonly ISpeechService iSpeechService;
        public TextToSpeechController(ISpeechService iSpeechService)
        {
            this.iSpeechService = iSpeechService ??
                 throw new ArgumentNullException(nameof(iSpeechService));
        }

        [HttpPost]
        public async Task<ActionResult> PostAsync([FromBody] string text)
        {
            await this.iSpeechService.CreateAndSaveAudioAsync(text);
            return this.Ok();
        }
    }
}
