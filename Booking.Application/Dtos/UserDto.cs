using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

#nullable disable

namespace Booking.Application.Dtos
{
    public class UserDto
    {
        public string ID { get; set; }
        [Display(Name = "Nazwa użytkownika")]
        public string Username { get; set; }
        [Required(ErrorMessage = "Adres e-mail nie może być pusty.")]
        [EmailAddress(ErrorMessage = "Podany adres e-mail jest niepoprawny.")]
        [Display(Name = "Adres e-mail")]
        public string Email { get; set; }

        //[Required(ErrorMessage = "Numer telefonu nie może być pusty.")]
        [Phone(ErrorMessage = "Podany numer telefonu jest niepoprawny.")]
        [Display(Name = "Numer telefonu")]
        public string PhoneNumber { get; set; }

        [Display(Name = "Imię")]
        public string FirstName { get; set; }

        [Display(Name = "Nazwisko")]
        public string SecondName { get; set; }

        [Display(Name = "Adres")]
        public string AddressLine { get; set; }

        [Display(Name = "Miasto")]
        public int? CityID { get; set; }

        [Display(Name = "Kod pocztowy")]
        public string PostalCode { get; set; }
        [Display(Name = "Opis")]
        public string Description { get; set; }
        public IFormFile Avatar { get; set; }
        public string AvatarPath { get; set; }
    }
}
