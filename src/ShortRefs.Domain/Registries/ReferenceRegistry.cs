namespace ShortRefs.Domain.Registries
{
    using Microsoft.Extensions.DependencyInjection;

    using ShortRefs.Domain.Services;

    public static class ReferenceRegistry
    {
        public static void RegisterReferenceEncoder(this IServiceCollection services)
        {
            services.AddSingleton<IReferenceEncoder, ReferenceEncoder>();
        }
    }
}
