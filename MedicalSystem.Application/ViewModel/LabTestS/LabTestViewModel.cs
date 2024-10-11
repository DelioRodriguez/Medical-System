
using System.ComponentModel.DataAnnotations;

namespace MedicalSystem.Application.ViewModel.LabTestS
{
    public class LabTestViewModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage ="El nombre de la prueba es obligatorio")]
        [Display(Name = "Nombre")]
        public string Name { get; set; }
        public int ClinicId { get; set; }

    }
}
