using FluentValidation;
using Project01.Entities;

namespace Project01.Validations {
    public static class ValidationRegister {
        public static void Regis(IServiceCollection services) {
            services.AddTransient<IValidator<Page>, PageValidation>();
            services.AddTransient<IValidator<Role>, RoleValidation>();
            services.AddTransient<IValidator<User>, UserValidation>();
        }
    }
}