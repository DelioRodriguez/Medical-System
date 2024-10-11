using MedicalSystem.Application.ViewModel.LabTestS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MedicalSystem.Application.ViewModel.Extras
{
    public class ConsultLabTestsViewModel
    {
        public int AppointmentId { get; set; }
        public int PatientId { get; set; }
        public int ClinicId { get; set; }
        public List<int> SelectedLabTestIds { get; set; } = new List<int>(); 
        public IEnumerable<LabTestViewModel> LabTests { get; set; } 
    }
}
