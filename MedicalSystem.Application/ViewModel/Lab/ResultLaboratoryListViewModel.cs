using MedicalSystem.Domain.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MedicalSystem.Application.ViewModel.Lab
{
    public class ResultLaboratoryListViewModel
    {
        public int Id { get; set; }

        public string PatientFullName { get; set; }

        [Required(ErrorMessage = "La cédula es obligatoria.")]
        public string PatientIdCard { get; set; }
        public string TestName { get; set; }

        public Status Status { get; set; }
    }
}
