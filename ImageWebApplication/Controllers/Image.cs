using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;

namespace ImageWebApplication.Controllers
{
    public class Image
    {
        public static CloudBlobContainer GetBlobContainer(string containerName)
        {
            string connectionString = $"paste your connection string";

            // Setup the connection to the storage account
            CloudStorageAccount storageAccount = CloudStorageAccount.Parse(connectionString);

            // Connect to the blob storage
            CloudBlobClient serviceClient = storageAccount.CreateCloudBlobClient();
            // Connect to the blob container
            CloudBlobContainer container = serviceClient.GetContainerReference($"{containerName}");
            return container;
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