using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace Application
{
    public static class ApplicationServiceRegistration
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services)
        {
            //Assembly'deki çalışan her şeyi mapleyebilecek servisi tanımladık
            services.AddAutoMapper(Assembly.GetExecutingAssembly());
            services.AddMediatR(configuration => //mediatr tüm assembly tara commandları bul sana komut send yaptıysamonu çalıştır
            {
                configuration.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly());
            });
            return services;
        }
    }
}
