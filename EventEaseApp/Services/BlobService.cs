using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using Microsoft.AspNetCore.Http;
using System;
using System.IO;
using System.Threading.Tasks;

namespace EventEaseApp.Services
{
    public class BlobService
    {
        private readonly BlobServiceClient _blobServiceClient;
        private readonly BlobContainerClient _containerClient;

        public BlobService(BlobServiceClient blobServiceClient)
        {
            _blobServiceClient = blobServiceClient;

            // Connects to the "venue-images" container. 
            // We only get the reference here (no network call yet).
            _containerClient = _blobServiceClient.GetBlobContainerClient("venue-images");
        }

        // Uploads a venue or event image to Azurite blob storage and returns the URL
        /// <summary>
        /// AZURE BLOB STORAGE REFERENCE:
        /// Uploads an image file to Azure Blob Storage container
        /// Implementation based on Microsoft Learn documentation
        /// Source: Microsoft Docs - Upload a blob (2024)
        /// URL: https://learn.microsoft.com/en-us/azure/storage/blobs/storage-upload-process-images?tabs=docker
        /// Accessed: May 2026
        /// </summary>
        public async Task<string> UploadAsync(IFormFile file)
        {
            // 1. Basic validation
            if (file == null || file.Length == 0)
                throw new ArgumentException("File is empty.");

            // 2. Creates the container if it doesn't exist ASYNCHRONOUSLY
            // PublicAccessType.Blob allows the images to be viewed in your HTML <img> tags.
            await _containerClient.CreateIfNotExistsAsync(PublicAccessType.Blob);

            // 3. Generate a unique file name to prevent overwriting other users' images
            var fileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);

            // 4. Get a reference to the specific blob (file)
            var blobClient = _containerClient.GetBlobClient(fileName);

            // 5. Upload the image stream
            // The 'using' block ensures the file stream is closed properly after upload
            using (var stream = file.OpenReadStream())
            {
                await blobClient.UploadAsync(stream, overwrite: true);
            }

            // 6. Return the full URL of the uploaded image to be saved in your DB
            return blobClient.Uri.ToString();
        }

        // Deletes a blob (image) from Azure/Azurite blob storage using the image URL

        /// <summary>
        /// AZURE BLOB STORAGE REFERENCE:
        /// Deletes a blob from Azure Storage using its URL
        /// Source: Microsoft Docs - Delete a blob (2024)
        /// URL: https://learn.microsoft.com/en-us/azure/storage/blobs/storage-manage-blobs-dotnet#delete-a-blob
        /// Accessed: May 2026
        /// </summary>
        public async Task<bool> DeleteAsync(string imageUrl)
        {
            try
            {
                // 1. Basic validation - check if URL is provided
                if (string.IsNullOrEmpty(imageUrl))
                    return false;

                // 2. Extract the blob name (file name) from the full URL                
                // We need to get just "filename.jpg"
                var uri = new Uri(imageUrl);
                var blobName = Path.GetFileName(uri.LocalPath);

                // 3. If no blob name could be extracted, return false
                if (string.IsNullOrEmpty(blobName))
                    return false;

                // 4. Gets a reference to the specific blob (file)
                var blobClient = _containerClient.GetBlobClient(blobName);

                // 5. Checks if the blob exists before trying to delete
                if (!await blobClient.ExistsAsync())
                    return false;

                // 6. Deletes the blob
                var response = await blobClient.DeleteAsync();

                // 7. Return true if deletion was successful (status code 202 or 204)
                return response.Status == 202 || response.Status == 204;
            }
            catch (Exception)
            {
                // If any error occurs, return false
                return false;
            }
        }

        // Alternative Delete method that uses the blob name directly
        public async Task<bool> DeleteByBlobNameAsync(string blobName)
        {
            try
            {
                // 1. Basic validation
                if (string.IsNullOrEmpty(blobName))
                    return false;

                // 2. Get a reference to the specific blob (file)
                var blobClient = _containerClient.GetBlobClient(blobName);

                // 3. Check if the blob exists before trying to delete
                if (!await blobClient.ExistsAsync())
                    return false;

                // 4. Delete the blob
                var response = await blobClient.DeleteAsync();

                // 5. Return true if deletion was successful
                return response.Status == 202 || response.Status == 204;
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}