
using MedicalSystem.Domain.Entities;
using MedicalSystem.Domain.Enums;

namespace MedicalSystem.Application.ViewModel.Lab
{
    public class CreateResultViewModel
    {
        public int PatientId { get; set; }
        public int LabTestId { get; set; }
       
        public string? Result { get; set; }
        public Status Status { get; set; }
        public int ClinicId { get; set; }
        public int AppointmentId { get; set; }
     
    }
}