using DisprzTraining.Business;
using DisprzTraining.DataAccess;
using DisprzTraining.validation;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace DisprzTraining.Utils
{
    public static class ConfigureDependenciesExtension
    {
        public static void ConfigureDependencyInjections(this IServiceCollection services)
        {
            services.AddHttpContextAccessor();

            services.AddScoped<IAppointmentBL, AppointmentBL>();
            services.AddScoped<IAppointmentDAL, AppointmentDAL>();
            services.AddScoped<IAppointmentValidation, AppointmentValidation>();
        
        }
    }
}
