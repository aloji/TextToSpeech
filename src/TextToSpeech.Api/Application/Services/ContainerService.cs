using Microsoft.Extensions.Configuration;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;
using System;
using System.IO;
using System.Threading.Tasks;

namespace TextToSpeech.Api.Application.Services
{

    public class ContainerService : IContainerService
    {
        readonly IConfiguration iConfiguration;
        public ContainerService(IConfiguration iConfiguration)
        {
            this.iConfiguration = iConfiguration
                ?? throw new ArgumentNullException(nameof(iConfiguration));
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
            var connectionString = iConfiguration.GetValue<string>("StorageConnectionString");
            var containerName = iConfiguration.GetValue<string>("StorageContainerName");

            var storageAccount = CloudStorageAccount.Parse(connectionString);
            var myClient = storageAccount.CreateCloudBlobClient();
            var container = myClient.GetContainerReference(containerName);

            await container.CreateIfNotExistsAsync();

            return container;
        }
    }
}
