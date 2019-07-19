using Microsoft.Extensions.Options;
using NSubstitute;
using System;
using System.Collections.Generic;
using TextToSpeech.Api.Application.Options;
using TextToSpeech.Api.Application.Services;
using Xunit;

namespace TextToSpeech.UnitTest.Acronyms
{
    public class ReplaceShould
    {
        [Fact]
        public void ReturnNullIfTextIsNull()
        {
            var optionMonitor = Substitute.For<IOptionsMonitor<List<AcronymsOptions>>>();
            optionMonitor.CurrentValue.Returns(new List<AcronymsOptions>());

            var sub = new AcronymsService(optionMonitor);

            var actual = sub.Replace(null);
            Assert.Null(actual);
        }

        [Fact]
        public void ReturnEmptyIfTextIsEmpty()
        {
            var optionMonitor = Substitute.For<IOptionsMonitor<List<AcronymsOptions>>>();
            optionMonitor.CurrentValue.Returns(new List<AcronymsOptions>());

            var sub = new AcronymsService(optionMonitor);

            var actual = sub.Replace(string.Empty);
            Assert.Equal(string.Empty, actual);
        }

        [Fact]
        public void ReturnStringWithValues()
        {
            var optionMonitor = Substitute.For<IOptionsMonitor<List<AcronymsOptions>>>();
            optionMonitor.CurrentValue.Returns(new List<AcronymsOptions>()
            {
                new AcronymsOptions{ Key = "/", Value = " " },
                new AcronymsOptions{ Key = "EUR", Value = "euro" },
                new AcronymsOptions{ Key = "USD", Value = "dollar" }
            });

            var expected = "euro dollar";

            var sub = new AcronymsService(optionMonitor);

            var actual = sub.Replace("EUR/USD");
            Assert.Equal(expected, actual);
        }
    }
}
