namespace Booking.Application.Dtos
{
    public class OfferPhotoDto
    {
        public string Path { get; set; }
        public string FileName { get; set; }
        public string FullPath 
            => (Path is null || FileName is null) ? "" : System.IO.Path.Combine(Path, FileName);
    }
}
