namespace ShortRefs.Api
{
    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;

    using ShortRefs.Api.Pipelines;
    using ShortRefs.Data.Mongo.Registries;
    using ShortRefs.Domain.Registries;
    using ShortRefs.Domain.Repositories;

    public class Startup
    {
        private readonly IConfiguration configuration;

        public Startup(IConfiguration configuration)
        {
            this.configuration = configuration;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            var mongoConnectionString = this.configuration.GetConnectionString("ReferenceDb");

            services.RegisterMongo(mongoConnectionString);
            services.RegisterCounters();
            services.RegisterReferences();
            services.RegisterReferenceEncoder();

            services.AddMvc(options => options.Filters.Add<ValidateModelStateFilter>())
                .SetCompatibilityVersion(CompatibilityVersion.Version_2_1)
                .AddJsonOptions(options =>
                {
                    options.SerializerSettings.Converters.Add(new Newtonsoft.Json.Converters.StringEnumConverter());
                    options.SerializerSettings.NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore;
                });

            var serviceProvider = services.BuildServiceProvider();
            var sequenceCounterRepository = serviceProvider.GetService<ISequenceCounterRepository>();
            sequenceCounterRepository.CreateIfNotExists("referenceId");
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseMiddleware<AuthenticationMiddleware>();
            app.UseMiddleware<ErrorHandlingMiddleware>();

            app.UseMvc(routes =>
            {
                routes.MapRoute(name: "default", template: "{Controller=Reference}/{Action=GetMyReferenceStatAsync}/{id?}");

                app.Map("/favicon.ico", delegate { });
            });
        }
    }
}
