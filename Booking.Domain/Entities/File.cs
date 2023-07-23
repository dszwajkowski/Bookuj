using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

#nullable disable

namespace Booking.Domain.Entities
{
    public class File<TKey> where TKey : IEquatable<TKey>
    {
        public TKey ID { get; set; }
        [Required, MaxLength(450)]
        public string Path { get; set; }
        [Required, MaxLength(256)]
        public string FileName { get; set; }
        [NotMapped]
        public string FullPath
            => (Path is null || FileName is null) ? "" : System.IO.Path.Combine(Path, FileName);
    }
}
