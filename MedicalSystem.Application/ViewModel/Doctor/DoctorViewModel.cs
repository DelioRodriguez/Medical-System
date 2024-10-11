
using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;
using System.Drawing;

namespace MedicalSystem.Application.ViewModel.Doctor
{
    public class DoctorViewModel
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "El nombre es obligatorio")]
        [Display(Name = "Nombre")]

        public string Firstname { get; set; }

        [Required(ErrorMessage = "El apellido es obligatorio")]
        [Display(Name ="Apellido")]
        public string Lastname { get; set; }

        [Required(ErrorMessage ="El correo es obligatorio")]
        [EmailAddress(ErrorMessage = "El correo no es valido")]
        [Display(Name = "Correo")]
        public string Email { get; set; }

        [Required(ErrorMessage = "El telefono esa obligatorio")]
        [Display(Name = "Telefono")]
        public string Phone { get; set; }

        [Required(ErrorMessage = "la cedula es obligatoria")]
        [Display(Name ="Cedula")]
        public string IDCard { get; set; }

        public string? Photo {  get; set; }

        [Display(Name = "Clinica")]

        public int ClinicID { get; set; }
        public IEnumerable<SelectListItem>? Clinics { get; set; }
    }
    
}
