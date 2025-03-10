using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using Infrastructure.Utlities;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.FileUploadService.FileUploadOnService
{
    public class FileUploadUloudinary : IFileUploadUloudinary
    {


        private readonly Attachments _attachment;

        private readonly Cloudinary _cloudanry;

        private readonly HttpClient _httpClient;
        public FileUploadUloudinary(HttpClient httpClient, IOptions<Attachments> options)
        {
            _httpClient = httpClient;
            _attachment = options.Value;
            _cloudanry = new Cloudinary(_attachment.Url);
        }
        public Task<UploadReturndedData> Delete(string publicId)
        {
            throw new NotImplementedException();
        }

        public async Task<UploadReturndedData> Upload(IFormFile file)
        {

            var ex = Path.GetExtension(file.FileName);

            var result = new UploadReturndedData();

            if (!_attachment.extensions.Contains(ex))
            {
                result.Message = "This File Not Provider";
                return result;
            }
            if (file.Length > _attachment.sizeMegaByte * 1024 * 1024)
            {
                result.Message = "Tihs file is large";
                return result;
            }


            using var filestream = file.OpenReadStream();

            var UploadParams = new ImageUploadParams()
            {
                File = new FileDescription(file.FileName, filestream),
                Folder = "ImagesSearch"

            };
            var UploadResult = await _cloudanry.UploadAsync(UploadParams);

            result.Message = "File Uploaded Sucessfully";

            result.Url = UploadResult.SecureUrl.ToString();
            result.PublicId = UploadResult.PublicId.Split("/")[1];
            return result;

        }
    }
}
