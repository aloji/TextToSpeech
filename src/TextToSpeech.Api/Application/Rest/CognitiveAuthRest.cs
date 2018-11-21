using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace TextToSpeech.Api.Application.Rest
{
    public class CognitiveAuthRest
    {
        readonly HttpClient client;
        public CognitiveAuthRest(HttpClient client)
        {
            this.client = client;
        }

        public async Task<string> GetTokenAsync()
        {
            var response = await this.client.PostAsync("", 
                new StringContent("", Encoding.UTF8, "application/x-www-form-urlencoded"));

            response.EnsureSuccessStatusCode();

            var result = await response.Content.ReadAsStringAsync();
            return result;
        }
    }
}
