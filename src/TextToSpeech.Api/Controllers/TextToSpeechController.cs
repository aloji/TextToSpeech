using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using TextToSpeech.Api.Application.Services;
using TextToSpeech.Api.Models.Request;

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

        [HttpGet]
        [Route("{id:guid}")]
        public async Task<ActionResult> GetAsync(Guid id)
        {
            var url = await this.iSpeechService.GetUrl(id);
            if (string.IsNullOrWhiteSpace(url))
            {
                return this.NotFound();
            }
            else
            {
                return this.Ok(url);
            }
        }

        [HttpPost]
        public async Task<ActionResult> PostAsync(TextRequest request)
        {
            var url = await this.iSpeechService.CreateAndSaveAudioAsync(request.Content);
            return this.Created(url, null);
        }

        [HttpPut]
        [Route("{id:guid}")]
        public async Task<ActionResult> PutAsync(Guid id, TextRequest request)
        {
            var url = await this.iSpeechService.UpdateAndSaveAudioAsync(id, request.Content);
            return this.Ok(url);
        }
    }
}
