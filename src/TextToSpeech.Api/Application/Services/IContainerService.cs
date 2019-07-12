using System;
using System.IO;
using System.Threading.Tasks;

namespace TextToSpeech.Api.Application.Services
{
    public interface IContainerService
    {
        Task<Uri> UploadFromStreamAsync(string fileName, Stream stream);
        Task<Uri> GetUrlAsync(string fileName);
    }
}
