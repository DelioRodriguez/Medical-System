using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MedicalSystem.Application.ViewModel.Lab
{
    public class PendingResultsViewModel
    {
        public string PatientIDCard { get; set; }

        public List<ResultLaboratoryListViewModel>? Results { get; set; }
    }
}
