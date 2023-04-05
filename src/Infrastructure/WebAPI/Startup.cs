using Infrastructure.IoC;
using Microsoft.AspNetCore.Mvc;

namespace Infrastructure.WebAPI
{
    public class Startup
    {
        public IConfiguration Configuration { get; }
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }


        public void ConfigureServices(IServiceCollection services)
        {
            // Adição dos Controllers
            services.AddControllers();

            // Realiza comunicação com os demais Projetos.
            services.AddInfrastructure(Configuration);
            services.AddAdapters();
            services.AddDomain();

            // Configuração do Swagger
            services.AddInfrastructureSwagger();

            // Permite que rotas sejam acessíveis em lowercase
            services.AddRouting(options => options.LowercaseUrls = true);
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI();
            }
            else
            {
                app.UseHsts();
                app.UseHttpsRedirection();
            }

            app.UseHsts();
            app.UseHttpsRedirection();

            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints => endpoints.MapControllers());
        }
    }
}
