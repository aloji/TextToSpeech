using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using TextToSpeech.Api.Application.Options;

namespace TextToSpeech.Api.Application.Services
{
    public class AcronymsService : IAcronymsService
    {
        readonly IOptionsMonitor<List<AcronymsOptions>> options;
        public AcronymsService(IOptionsMonitor<List<AcronymsOptions>> options)
        {
            this.options = options
                ?? throw new ArgumentNullException(nameof(options));
        }

        public string Replace(string text)
        {
            var result = text;
            var acronyms = this.options.CurrentValue;
            if (!string.IsNullOrWhiteSpace(text) && acronyms != null && acronyms.Any())
            {
                foreach (var acronym in acronyms)
                {
                    result = result.Replace(acronym.Key, acronym.Value);
                }
            }
            return result;
        }
    }
}
