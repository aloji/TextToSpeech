using Microsoft.Extensions.Options;
using NSubstitute;
using System;
using System.Collections.Generic;
using TextToSpeech.Api.Application.Options;
using TextToSpeech.Api.Application.Services;
using Xunit;

namespace TextToSpeech.UnitTest.Regex
{
    public class ReplaceShould
    {
        [Fact]
        public void ReturnNullIfTextIsNull()
        {
            var optionMonitor = Substitute.For<IOptionsMonitor<List<RegexOptions>>>();
            optionMonitor.CurrentValue.Returns(new List<RegexOptions>());

            var sub = new RegexService(optionMonitor);

            var actual = sub.Replace(null);
            Assert.Null(actual);
        }

        [Fact]
        public void ReturnEmptyIfTextIsEmpty()
        {
            var optionMonitor = Substitute.For<IOptionsMonitor<List<RegexOptions>>>();
            optionMonitor.CurrentValue.Returns(new List<RegexOptions>());

            var sub = new RegexService(optionMonitor);

            var actual = sub.Replace(string.Empty);
            Assert.Equal(string.Empty, actual);
        }

        [Fact]
        public void ReturnStringReplacement()
        {
            var optionMonitor = Substitute.For<IOptionsMonitor<List<RegexOptions>>>();
            optionMonitor.CurrentValue.Returns(new List<RegexOptions>()
            {
                new RegexOptions
                {
                    Pattern = "(\\d+)\\.(\\d{2})(\\d{2})?",
                    Replacement = "$1 point $2 $3"
                }
            });

            var expected = "1 point 23 45 123 point 65 ";

            var sub = new RegexService(optionMonitor);

            var actual = sub.Replace("1.2345 123.65");
            Assert.Equal(expected, actual);
        }
    }
}
