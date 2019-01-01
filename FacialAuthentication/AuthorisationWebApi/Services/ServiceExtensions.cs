using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Authorisation.Core;
using Autofac;

using Swashbuckle.AspNetCore.Swagger;
using Authorisation.Infrastructure;
using Authorisation.Adaptor;
using Autofac.Extensions.DependencyInjection;

namespace AuthorisationWebApi.Services
{
    public static class ServiceExtensions
    {

        public static System.IServiceProvider RegisterServicesViaAutofac(this IServiceCollection services)
        {
            var builder = new ContainerBuilder();

            builder.Populate(services);

            builder.RegisterModule(new CoreModule());

            builder.RegisterModule(new InfrastructureModule());

            return new AutofacServiceProvider(builder.Build());
        }

        public static void ConfigureCors(this IServiceCollection services)
        {
            // todo: autofac mapping
            services.AddCors(options =>
            {
                options.AddPolicy("CorsPolicy",
                    builder => builder.AllowAnyOrigin()
                    .AllowAnyMethod()
                    .AllowAnyHeader()
                    .AllowCredentials());
            }
            );

            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);

            // In production, the Angular files will be served from this directory
            services.AddSpaStaticFiles(configuration =>
            {
                configuration.RootPath = "ClientApp/dist";
            });

            // Register the Swagger generator, defining 1 or more Swagger documents
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new Info { Title = "My API", Version = "v1" });
            });
        }

    }
}
