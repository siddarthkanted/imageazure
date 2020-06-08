using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Web;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;

namespace ImageWebApplication.Controllers
{
    public class Image
    {

        static string connectionString = $"";

        public static CloudBlobContainer GetBlobContainer(string containerName)
        {
     
            // Setup the connection to the storage account
            CloudStorageAccount storageAccount = CloudStorageAccount.Parse(connectionString);

            // Connect to the blob storage
            CloudBlobClient serviceClient = storageAccount.CreateCloudBlobClient();
            // Connect to the blob container
            CloudBlobContainer container = serviceClient.GetContainerReference($"{containerName}");
            return container;
        }

        public static PushStreamContent GetPushStreamContent(Stream sourceStream)
        {
            Func<Stream, Task> copyStreamAsync =
                async (stream) =>
                {
                    using (stream)
                    {
                        using (sourceStream)
                        {
                            await sourceStream.CopyToAsync(stream).ConfigureAwait(false);
                        }
                    }
                };

            return new PushStreamContent(
                (stream, httpContent, transportContext) => { copyStreamAsync(stream); },
                new MediaTypeHeaderValue("image/jpeg"));
        }


        public static async Task<Stream> GetFileStream(string fileName)
        {
            fileName = String.IsNullOrEmpty(fileName) ? "office-1356793_1280.png" : fileName;
            CloudBlobContainer container = GetBlobContainer("imagecontainer");
            CloudBlockBlob blockBlob = container.GetBlockBlobReference(fileName);
            return await blockBlob.OpenReadAsync().ConfigureAwait(false);
        }


        public static byte[] GetFileContent(string fileName)
        {
            fileName = String.IsNullOrEmpty(fileName) ? "office-1356793_1280.png" : fileName;
            CloudBlobContainer container = GetBlobContainer("imagecontainer");
            CloudBlockBlob blockBlob = container.GetBlockBlobReference(fileName);
            blockBlob.FetchAttributes();
            long fileByteLength = blockBlob.Properties.Length;
            byte[] fileContent = new byte[fileByteLength];
            for (int i = 0; i < fileByteLength; i++)
            {
                fileContent[i] = 0x20;
            }

            blockBlob.DownloadToByteArray(fileContent, 0);
            return fileContent;
        }
    }
}