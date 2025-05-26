using Microsoft.EntityFrameworkCore;

namespace Slot4PRN
{
    public static class ServiceExtensions
    {        public static void ConfigureSqlContext(this IServiceCollection services,
    IConfiguration configuration) =>
    services.AddDbContext<RepositoryContext>(opts =>
        opts.UseSqlServer(configuration.GetConnectionString("Slot5PRN")));
    }
}
