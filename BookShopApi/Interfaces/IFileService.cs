namespace BookShopApi.Interfaces
{
    public interface IFileService
    {
        Task<string> SaveFileAsync(IFormFile imageFile, string[] allowedFileExtensions, string folderName);
        void DeleteFile(string fileName, string folderName);
    }
}
