
using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace MedicalSystem.Application.ViewModel.User
{
    public class RegisterViewModel
    {
        [Required(ErrorMessage = "El nombre es obligatorio.")]
        [MaxLength(20, ErrorMessage = "El nombre no puede tener más de 20 caracteres.")]
        [Display(Name = "Nombre")]
        public string FirsName { get; set; }

        [Required(ErrorMessage = "El apellido es obligatorio")]
        [MaxLength(20, ErrorMessage = "El apellido no puede tener más de 20 caracteres.")]
        [Display(Name = "Apellido")]
        public string LastName { get; set; }

        [Required(ErrorMessage = "El correo electrónico es obligatorio.")]
        [EmailAddress(ErrorMessage = "El formato del correo electrónico es inválido.")]
        [Display(Name = "Correo Electronico")]
        public string Email { get; set; }

        [Required(ErrorMessage = "El nombre de usuario es obligatorio.")]
        [MaxLength(20, ErrorMessage = "El nombre de usuario debe ser menor a 20 caracteres")]
        [MinLength(3, ErrorMessage = "El nombre de usuario debe de ser mayor a 3 caracteres")]
        [Display(Name = "Nombre de usuario")]
        public string Username { get; set; }

        [Required(ErrorMessage = "La contraseña es obligatoria")]
        [DataType(DataType.Password)]
        [MaxLength(100, ErrorMessage = "La contraseña debe de ser menor de 100 caracteres")]
        [MinLength(8, ErrorMessage = "La contraseña debe de contener más de 8 caracteres")]
        [Display(Name = "Contraseña")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirmar contraseña")]
        [Compare("Password", ErrorMessage = "La contraseña no coincide")]
        public string ConfirmPassword { get; set; }
        [Display(Name = "Clinica")]
        [Required(ErrorMessage = "Clinica obligatoria")]
        public int ClinicID { get; set; }

        public string? ClinicName { get; set; }


        public IEnumerable<SelectListItem>? Clinics { get; set; }
        [Display(Name = "Rol")]
        public string? Role { get; set; }

        public IEnumerable<SelectListItem>? Roles { get; set; }
    }
}
