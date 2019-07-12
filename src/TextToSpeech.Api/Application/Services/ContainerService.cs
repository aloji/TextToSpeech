using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;
using System;
using System.IO;
using System.Threading.Tasks;
using TextToSpeech.Api.Application.Options;

namespace TextToSpeech.Api.Application.Services
{

    public class ContainerService : IContainerService
    {
        readonly IOptionsMonitor<StorageOptions> options;
        public ContainerService(IOptionsMonitor<StorageOptions> options)
        {
            this.options = options
                ?? throw new ArgumentNullException(nameof(options));
        }

        public async Task<Uri> GetUrlAsync(string fileName)
        {
            var result = default(Uri);
            var container = await this.GetContainer();
            var blockBlob = container.GetBlockBlobReference(fileName);

            if (await blockBlob.ExistsAsync())
                result = blockBlob.StorageUri.PrimaryUri;
            
            return result;
        }

        public async Task<Uri> UploadFromStreamAsync(string fileName, Stream stream)
        {
            var container = await this.GetContainer();
            var blockBlob = container.GetBlockBlobReference(fileName);

            await blockBlob.UploadFromStreamAsync(stream);

            var result = blockBlob.StorageUri.PrimaryUri;
            return result;
        }

        private async Task<CloudBlobContainer> GetContainer()
        {
            var connectionString = options.CurrentValue.ConnectionString;
            var containerName = options.CurrentValue.ContainerName;

            var storageAccount = CloudStorageAccount.Parse(connectionString);
            var myClient = storageAccount.CreateCloudBlobClient();
            var container = myClient.GetContainerReference(containerName);

            await container.CreateIfNotExistsAsync();

            return container;
        }
    }
}
