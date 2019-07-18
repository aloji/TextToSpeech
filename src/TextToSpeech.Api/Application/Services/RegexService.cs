using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace TextToSpeech.Api.Application.Services
{
    public class RegexService : IReplaceService
    {
        readonly IOptionsMonitor<List<Options.RegexOptions>> options;
        public RegexService(IOptionsMonitor<List<Options.RegexOptions>> options)
        {
            this.options = options
                ?? throw new ArgumentNullException(nameof(options));
        }
        public string Replace(string text)
        {
            var result = text;
            var regexs = this.options.CurrentValue;
            if (!string.IsNullOrWhiteSpace(result) && regexs != null && regexs.Any())
            {
                foreach (var option in regexs)
                {
                    var regex = new Regex(option.Pattern);
                    result = regex.Replace(result, option.Replacement);
                }
            }
            return result;
        }
    }
}
