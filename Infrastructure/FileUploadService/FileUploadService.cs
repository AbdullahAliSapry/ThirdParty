
using Microsoft.AspNetCore.Http;

namespace Infrastructure.FileUploadService
{
    public class FileUploadService : IFileUploadService
    {

        private readonly string _uploadFolderPath;



        // solve this errors references  project in .net core 
        public FileUploadService(string path)
        {
            _uploadFolderPath = Path.Combine(path, "UploadedFiles");

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

        public class ReturnDataFile
        {
            public string FilePath { get; set; }
            public string FileName { get; set; }

        }
        public async Task<ReturnDataFile> UploadFile(IFormFile file, string folderName, long maxSize)
        {
            try
            {
                if (file == null || file.Length == 0)
                    throw new ArgumentException("File is empty");

                if (!IsFileTypeAllowed(file))
                    throw new ArgumentException("File type is not allowed");

                if (!IsFileSizeAllowed(file, maxSize))
                    throw new ArgumentException("File size exceeds the limit");

                string fileName = $"{Guid.NewGuid()}_{file.FileName}";

                string folderPath = Path.Combine(_uploadFolderPath, folderName);

                if (!Directory.Exists(folderPath))
                {
                    Directory.CreateDirectory(folderPath);
                }

                string filePath = Path.Combine(folderPath, fileName);

                using var fileStream = new FileStream(filePath, FileMode.Create);
                await file.CopyToAsync(fileStream);

                var result = new ReturnDataFile
                {
                    FileName = fileName,
                    FilePath = filePath,
                };

                return result;
            }
            catch (Exception ex)
            {
                // Log the error or handle it as required
                Console.WriteLine($"Error occurred while uploading the file: {ex.Message}");
                throw;
            }
        }

    }
}
