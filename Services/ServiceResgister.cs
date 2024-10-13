
namespace Project01.Services {
    public static class ServiceRegister {
        public static void Regis(IServiceCollection services) {
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IRoleService, RoleService>();
            services.AddScoped<IPageService, PageService>();
            services.AddScoped<IAccessService, AccessService>();
        }
    }
}