using Booking.Application.Dtos;
using Booking.Domain.Entities;
using System.ComponentModel.DataAnnotations;

namespace Bookuj.WebUI.ViewModels
{
    public class EditOfferViewModel
    {
        // TODO display name
        [Required(ErrorMessage = "Tytuł nie może być pusty.")]
        [StringLength(50, MinimumLength = 5, ErrorMessage = "Tytuł musi mieć od {2} do {1} znaków.")]
        public string? Title { get; set; }
        [Required(ErrorMessage = "Opis nie może być pusty.")]
        [StringLength(1000, MinimumLength = 20, ErrorMessage = "Opis musi mieć od {2} do {1} znaków.")]
        public string? Description { get; set; }
        [Required(ErrorMessage = "Adres nie może być pusty.")]
        [StringLength(255, MinimumLength = 5, ErrorMessage = "Adres musi mieć od {2} do {1} znaków.")]
        public string? AddressLine { get; set; }
        [Required(ErrorMessage = "Miasto nie może być puste.")]
        public int? CityID { get; set; }
        [Required(ErrorMessage = "Kod pocztowy nie może być pusty.")]
        [StringLength(20, MinimumLength = 6, ErrorMessage = "Kod pocztowy musi mieć od {2} do {1} znaków.")]
        public string? PostalCode { get; set; }
        [Required(ErrorMessage = "Oferta musi posiadać co najmniej jedną opcję noclegową.")]
        public IList<LodgingOptionDto>? LodgingOptions { get; set; }
        [Required(ErrorMessage = "Oferta musi posiadać co najmniej jedno zdjęcie.")]
        public ICollection<OfferPhotoDto>? Photos { get; set; }
    }
}
