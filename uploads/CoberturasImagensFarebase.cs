using Firebase.Storage;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace CoberturaImagens.Services
{
    public class CoberturaImagensServices
    {
        private const long MaxFileSize = 5 * 1024 * 1024; // 5 MB
        private readonly string _firebaseBucket = "animated-cinema-392321.appspot.com";

        public async Task<List<(string FileName, string Url)>> UploadImagesAsync(IEnumerable<IFormFile> images)
        {
            var result = new List<(string FileName, string Url)>();

            foreach (var image in images)
            {
                if (image.Length > MaxFileSize)
                    throw new Exception("Uma das imagens excede o tamanho máximo permitido de 5MB.");

                string[] validExtensions = { ".jpg", ".jpeg", ".png", ".gif" };
                string extension = Path.GetExtension(image.FileName).ToLower();
                if (!validExtensions.Contains(extension))
                    throw new Exception($"Formato de arquivo não suportado: {image.FileName}. Use JPG, JPEG, PNG ou GIF.");

                string uniqueFileName = $"{Guid.NewGuid()}{extension}";
                string folder = "ImagemAgenda";
                string firebasePath = $"{folder}/{uniqueFileName}";

                var storage = new FirebaseStorage(_firebaseBucket, new FirebaseStorageOptions
                {
                    AuthTokenAsyncFactory = () => Task.FromResult("firebase-admi.json")
                });

                using (var stream = image.OpenReadStream())
                {
                    await storage
                        .Child(firebasePath)
                        .PutAsync(stream);
                }

                string publicUrl = await storage
                    .Child(firebasePath)
                    .GetDownloadUrlAsync();

                result.Add((image.FileName, publicUrl));
            }

            return result;
        }
    }
}
