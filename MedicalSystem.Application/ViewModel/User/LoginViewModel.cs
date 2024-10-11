using System.ComponentModel.DataAnnotations;

namespace MedicalSystem.Application.ViewModel.User
{
    public class LoginViewModel
    {
        [Required(ErrorMessage ="El nombre de usuario es obligatorio")]
        [Display(Name ="Nombre de usuario")]
        public string username { get; set; }

        [Required(ErrorMessage ="La contraseña es obligatoria")]
        [DataType(DataType.Password)]
        [MinLength(8,ErrorMessage = "La contraseña debe de tener mas de 8 caracteres")]
        [Display(Name = "Contraseña")]
        public string password { get; set; }
    }
}
