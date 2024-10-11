using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace MedicalSystem.Application.ViewModel.Patient
{
    public class PatientViewModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "El nombre es obligatorio")]
        [Display(Name = "Nombre")]
        [StringLength(50, ErrorMessage = "El nombre no puede tener más de 50 caracteres.")]
        public string? FirstName { get; set; }

        [Required(ErrorMessage = "El apellido es obligatorio")]
        [Display(Name = "Apellido")]
        [StringLength(50, ErrorMessage = "El apellido no puede tener más de 50 caracteres.")]
        public string? LastName { get; set; }

        [EmailAddress(ErrorMessage = "El correo electrónico no es válido.")]
        [Display(Name = "Correo Electrónico")]
        public string? Email { get; set; }

        [Phone(ErrorMessage = "El número de teléfono no es válido.")]
        [Display(Name = "Teléfono")]
        public string? Phone { get; set; }

        [Display(Name = "Número de Identificación")]
        [StringLength(20, ErrorMessage = "El número de identificación no puede tener más de 20 caracteres.")]
        public string IDCard { get; set; }

        [Display(Name = "Foto")]
        public string? Photo { get; set; }

       
        [Display(Name = "Fecha de Nacimiento")]
        [DataType(DataType.Date)]
        public DateTime DateOfBirth { get; set; }

        [Display(Name = "Fumador")]
        public bool Smoker { get; set; }

        [Display(Name = "¿Tiene alergias?")]
        public bool HasAllergies { get; set; }

        public int ClinicId { get; set; }
        public IEnumerable<SelectListItem>? Clinics { get; set; }

    }
}
