using System.ComponentModel.DataAnnotations;

namespace CarRentalEmployeeApp.ViewModels
{
    public class CustomerCreateViewModel
    {
        [Required(ErrorMessage = "İsim alanı zorunludur")]
        public string Name { get; set; }
        [Required(ErrorMessage = "Soyisim alanı zorunludur")]
        public string Surname { get; set; }
        [Required(ErrorMessage = "Email alanı zorunludur")]
        [EmailAddress(ErrorMessage = "Geçerli bir email adresi girin")]
        public string Email { get; set; }
        [Required(ErrorMessage = "Telefon numarası zorunludur")]
        [Phone(ErrorMessage = "Geçerli bir telefon numarası girin")]
        public string PhoneNumber { get; set; }
        [Required(ErrorMessage = "Adres alanı zorunludur")]
        public string Address { get; set; }


        [Required(ErrorMessage = "Sorumlu personel seçilmelidir")]
        public string EmployeeId { get; set; }


    }
}
