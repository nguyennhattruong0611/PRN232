using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using Microsoft.AspNetCore.OData;

namespace Slot5PRNDemo1
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddDbContext<MyWorldDbContext>(options =>
                options.UseSqlServer(builder.Configuration.GetConnectionString("MyWorldDbConnection")));

            builder.Services.AddAuthorization();

            builder.Services.AddControllers().AddOData(option => option
                .Select()
                .Filter()
                .Count()
                .OrderBy()
                .Expand()
                .SetMaxTop(100)
                .AddRouteComponents("odata", ODataModelBuilderConfig.GetEdmModel()));

            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "ODataASPNETCoreDemo", Version = "v1" });
            });

            var app = builder.Build();

            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI(c =>
                {
                    c.SwaggerEndpoint("/swagger/v1/swagger.json", "ODataASPNETCoreDemo v1");
                });
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.MapControllers();

            app.Run();

        }
    }
}
