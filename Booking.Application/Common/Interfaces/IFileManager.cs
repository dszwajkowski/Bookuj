using Booking.Application.Common.Models;
using Microsoft.AspNetCore.Http;

namespace Booking.Application.Common.Interfaces
{
    public interface IFileManager
    {
        Task<FileModel> Upload(IFormFile file, string path);
        Task<IEnumerable<FileModel>> Upload(IEnumerable<IFormFile> files, string path);
        void Delete(string path);
    }
}
