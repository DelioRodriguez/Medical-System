using MedicalSystem.Application.ViewModel.LabTestS;
using MedicalSystem.Domain.Enums;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MedicalSystem.Application.ViewModel.Ammoin
{

    public class AppointmentViewModel
    {
        public int Id { get; set; }
        public DateTime Date { get; set; }
        public TimeSpan Time { get; set; }
        public string Cause { get; set; }
        public Status Status { get; set; }
        public int DoctorId { get; set; }
        public List<SelectListItem> Doctors { get; set; } = new List<SelectListItem>();
        public int PatientId { get; set; }
        public List<SelectListItem> Patients { get; set; } = new List<SelectListItem>();
        public int ClinicId { get; set; }
        public string? DoctorName { get; set; }
        public string? PatientName { get; set; }
    }


}
