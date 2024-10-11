using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MedicalSystem.Application.ViewModel.Lab
{
    public class ReportResultViewModel
    {
        public int ResultLaboratoryId { get; set; }

        [Required(ErrorMessage = "El resultado es obligatorio.")]
        [StringLength(500, ErrorMessage = "El resultado debe tener un máximo de 500 caracteres.")]
        public string? Result { get; set; }
    }
}
