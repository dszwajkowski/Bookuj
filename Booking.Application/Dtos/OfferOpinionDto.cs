namespace Booking.Application.Dtos
{
    public class OfferOpinionDto
    {
        public int ID { get; set; }
        public string AuthorID { get; set; }
        public string Username { get; set; }
        public string AvatarPath { get; set; }
        public int Rating { get; set; }
        public string Text { get; set; }
        public DateTime DateCreated { get; set; }
    }
}