#nullable disable

namespace Booking.Application.Common.Models
{
    public class FileModel
    {
        public string Path { get; set; }
        public string Url { get; set; }
        public string FileName { get; set; }
        public string FullPath => System.IO.Path.Combine(Path, FileName);
        public string FileUrl => System.IO.Path.Combine(Url, FileName);
    }
}
