using BookShopApi.Interfaces;

namespace BookShopApi.Services
{
    public class FileService : IFileService
    {
        private readonly string _imageRootPath;

        public FileService(IWebHostEnvironment environment)
        {
            _imageRootPath = Path.Combine(environment.WebRootPath, "Images");

            if(!Directory.Exists(_imageRootPath))
                Directory.CreateDirectory(_imageRootPath);
        }

        public async Task<string> SaveFileAsync(IFormFile imageFile, string[] allowedFileExtensions, string folderName)
        {
            if (imageFile == null || imageFile.Length == 0)
                throw new ArgumentException("Invalid file.");

            var folderPath = Path.Combine(_imageRootPath, folderName);

            if (!Directory.Exists(folderPath))
                Directory.CreateDirectory(folderPath);


            var fileExtension = Path.GetExtension(imageFile.FileName).ToLower();

            if (!allowedFileExtensions.Contains(fileExtension))
                throw new ArgumentException($"File type {fileExtension} is not allowed.");

            var fileName = $"{Guid.NewGuid()}{fileExtension}";
            var savePath = Path.Combine(_imageRootPath, folderName, fileName);


            using (var stream = new FileStream(savePath, FileMode.Create))
            {
                await imageFile.CopyToAsync(stream);
            }

            return $"/Images/{folderName}/{fileName}";
        }

        public void DeleteFile(string fileName, string folderName)
        {
            var sanitizedFileName = Path.GetFileName(fileName);
            var filePath = Path.Combine(_imageRootPath, sanitizedFileName);

            if (File.Exists(filePath))
                File.Delete(filePath);
        }
    }
}
