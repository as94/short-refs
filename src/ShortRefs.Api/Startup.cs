namespace ShortRefs.Api
{
    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.DependencyInjection;

    using ShortRefs.Api.Pipelines;
    using ShortRefs.Data.Mongo.Registries;
    using ShortRefs.Domain.Registries;

    public class Startup
    {
        public void ConfigureServices(IServiceCollection services)
        {
            // TODO: from config
            services.RegisterMongo("mongodb://localhost:27017");
            services.RegisterReferences();
            services.RegisterReferenceEncoder();

            services.AddMvc(options => options.Filters.Add<ValidateModelStateFilter>())
                .SetCompatibilityVersion(CompatibilityVersion.Version_2_1)
                .AddJsonOptions(options =>
                {
                    options.SerializerSettings.Converters.Add(new Newtonsoft.Json.Converters.StringEnumConverter());
                    options.SerializerSettings.NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore;
                });
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseMiddleware<AuthenticationMiddleware>();

            app.UseMvc(routes =>
            {
                routes.MapRoute(name: "default", template: "{Controller=Reference}/{Action=GetMyReferenceStatAsync}/{id?}");

                app.Map("/favicon.ico", delegate { });
            });
        }
    }
}
