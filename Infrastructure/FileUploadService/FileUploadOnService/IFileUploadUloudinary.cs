using Infrastructure.Utlities;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.FileUploadService.FileUploadOnService
{
    public interface IFileUploadUloudinary
    {


        Task<UploadReturndedData> Delete(string publicId);

        Task<UploadReturndedData> Upload(IFormFile file);


    }
}
