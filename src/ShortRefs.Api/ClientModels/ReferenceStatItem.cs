namespace ShortRefs.Api.ClientModels
{
    public sealed class ReferenceStatItem
    {
        public string Original { get; set; }

        public string Short { get; set; }

        public int RedirectsCount { get; set; }
    }
}
