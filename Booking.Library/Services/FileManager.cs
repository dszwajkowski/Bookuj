using Booking.Application.Common.Exceptions;
using Booking.Application.Common.Interfaces;
using Booking.Application.Common.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;

namespace Booking.Infrastructure.Services
{
    public class FileManager : IFileManager
    {
        private readonly IWebHostEnvironment _webHostEnviroment;
        public FileManager(IWebHostEnvironment webHostEnviroment)
        {
            _webHostEnviroment = webHostEnviroment;
        }

        public async Task<FileModel> Upload(IFormFile file, string path)
        {
            var fileModel = new FileModel
            {
                Path = $@"{_webHostEnviroment.WebRootPath}\files\{path}\",
                Url = $@"\files\{path}\",
                FileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName)
            };

            if (!Directory.Exists(fileModel.Path))
            {
                Directory.CreateDirectory(fileModel.Path);
            }

            using (var s = File.Create(fileModel.FullPath))
            {
                await file.CopyToAsync(s);
            }

            return fileModel;
        }

        public async Task<IEnumerable<FileModel>> Upload(IEnumerable<IFormFile> files, string path)
        {
            var fileModels = new List<FileModel>();

            foreach (var file in files)
            {
                fileModels.Add(await Upload(file, path));
            }

            return fileModels;
        }

        public void Delete(string path)
        {
            string fullPath = $@"{_webHostEnviroment.WebRootPath}{path}";
            if (File.Exists(fullPath))
            {
                File.Delete(fullPath);
            }
            else
            {
                throw new NotFoundException();
            }
        }
    }
}
