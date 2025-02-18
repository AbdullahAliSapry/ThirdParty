using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Infrastructure.FileUploadService
{
    public interface IFileUploadService
    {

        Task<string> UploadFile(IFormFile file, string path, long maxSize);
        Task<string> DeleteFile(string filePath);
        bool IsFileTypeAllowed(IFormFile file);
        bool IsFileSizeAllowed(IFormFile file, long maxSizeInMB);



    }
}
