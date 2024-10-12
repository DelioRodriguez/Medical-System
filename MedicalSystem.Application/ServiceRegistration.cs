using MedicalSystem.Application.Interfaces.Services.Appoint;
using MedicalSystem.Application.Interfaces.Services.Auth;
using MedicalSystem.Application.Interfaces.Services.DoctorSe;
using MedicalSystem.Application.Interfaces.Services.Lab;
using MedicalSystem.Application.Interfaces.Services.Patients;
using MedicalSystem.Application.Interfaces.Services.Test;
using MedicalSystem.Application.Interfaces.Services.UserS;
using MedicalSystem.Application.Services.AppointmentSer;
using MedicalSystem.Application.Services.AuthService;
using MedicalSystem.Application.Services.DoctorSer;
using MedicalSystem.Application.Services.Lab;
using MedicalSystem.Application.Services.PatientSe;
using MedicalSystem.Application.Services.Test;
using MedicalSystem.Application.Services.UserSer;
using Microsoft.Extensions.DependencyInjection;

namespace MedicalSystem.Application
{
    public static class ServiceRegistration
    {
        public static void RegisterServices(IServiceCollection services)
        {
            services.AddScoped<IAuthService, AuthService>();
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IPatientService, PatientService>();
            services.AddScoped<IDoctorService, DoctorServices>();
            services.AddScoped<IAppointmentService, AppointmentService>();
            services.AddScoped<ILabTestService, LabTestService>();
            services.AddScoped<ILabResultService, LabResulService>();
            services.AddScoped<IAuthService, AuthService>();
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IDoctorService, DoctorServices>();
        }
    }
}
