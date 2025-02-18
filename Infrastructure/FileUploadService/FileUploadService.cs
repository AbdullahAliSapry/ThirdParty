
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Hosting;


namespace Infrastructure.FileUploadService
{
    public class FileUploadService : IFileUploadService
    {

        private readonly string _uploadFolderPath;



        // solve this errors references  project in .net core 
        public FileUploadService()
        {

            _uploadFolderPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "UploadedFiles");

            if (!Directory.Exists(_uploadFolderPath))
            {
                Directory.CreateDirectory(_uploadFolderPath);
            }

        }
        //we need implemantions 
        public Task<string> DeleteFile(string filePath)
        {
            throw new NotImplementedException();
        }

        public bool IsFileSizeAllowed(IFormFile file, long maxSizeInMB)
        {
            long maxSizeInBytes = maxSizeInMB * 1024 * 1024;
            return file.Length <= maxSizeInBytes;
        }

        public bool IsFileTypeAllowed(IFormFile file)
        {

            var allowedType = new[] { ".jpg", ".png", ".pdf" };

            string fileExtension = Path.GetExtension(file.FileName).ToLower();

            return allowedType.Contains(fileExtension);
        }

        public async Task<string> UploadFile(IFormFile file, string path, long maxSize)
        {
            if (file == null || file.Length == 0) throw new ArgumentException("File is empty");

            if (!IsFileTypeAllowed(file)) throw new ArgumentException("File type is not allowed");

            if (!IsFileSizeAllowed(file, maxSize)) throw new ArgumentException("File size exceeds the limit");

            string fileName = $"{Guid.NewGuid()}_{file.FileName}";

            string filePath = Path.Combine(_uploadFolderPath, fileName);

            using var FileStream = new FileStream(filePath, FileMode.Create);
            await file.CopyToAsync(FileStream);

            return fileName;

        }
    }
}
