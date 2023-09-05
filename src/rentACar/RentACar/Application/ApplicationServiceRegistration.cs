using Core.Application.Rules;
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

            services.AddSubClassesOfType(Assembly.GetExecutingAssembly(), typeof(BaseBusinessRules));

            services.AddMediatR(configuration => //mediatr tüm assembly tara commandları bul sana komut send yaptıysamonu çalıştır
            {
                configuration.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly());
            });
            return services;
        }
        //Assembly içerisindeki tüm BusinessRulesları veya başkla bir sınıfı bul ve onları IOC'ye ekle
        public static IServiceCollection AddSubClassesOfType(this IServiceCollection services, Assembly assembly, Type type, Func<IServiceCollection, Type, IServiceCollection>? addWithLifeCycle = null)
        {
            var types = assembly.GetTypes().Where(t => t.IsSubclassOf(type) && type != t).ToList();
            foreach (var item in types)
            {
                if (addWithLifeCycle == null)
                    services.AddScoped(item);
                else
                    addWithLifeCycle(services, type);
            }
            return services;
        }
    }
}
