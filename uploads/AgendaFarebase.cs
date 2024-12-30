using Firebase.Storage;
using Microsoft.AspNetCore.Http;
using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Agenda.Services
{
    public class FirebaseImageService
    {
        private const long MaxFileSize = 5 * 1024 * 1024; // 5 MB
        private readonly string _firebaseBucket = "animated-cinema-392321.appspot.com";


        public async Task<string> UploadImageAsync(IFormFile image)
        {
            // Validação do tamanho do arquivo

            if (image.Length > MaxFileSize)
                throw new Exception("A imagem excede o tamanho máximo permitido de 5MB.");

            // Validação do nome do arquivo
            string[] validExtensions = { ".jpg", ".jpeg", ".png", ".gif" };
            string extension = Path.GetExtension(image.FileName).ToLower();
            if (!validExtensions.Contains(extension))
                throw new Exception("Formato de arquivo não suportado. Use JPG, JPEG, PNG ou GIF.");

            // Nome único para o arquivo
            string uniqueFileName = $"{Guid.NewGuid()}{extension}";

            // Caminho no Firebase
            string folder = "ImagemAgenda";
            string firebasePath = $"{folder}/{uniqueFileName}";

            // Inicializar o Firebase Admin SDK
            var storage = new FirebaseStorage(_firebaseBucket, new FirebaseStorageOptions
            {
                AuthTokenAsyncFactory = () => Task.FromResult("firebase-admi.json")
            });

            // Upload para o Firebase
            using (var stream = image.OpenReadStream())
            {
                await storage
                    .Child(firebasePath)
                    .PutAsync(stream);
            }

            // Retorna o URL público
            string publicUrl = await storage
                .Child(firebasePath)
                .GetDownloadUrlAsync();

            return publicUrl;
        }
    }
}