﻿namespace ShortRefs.Api.ClientModels
{
    public sealed class ReferenceStatItem
    {
        public string Original { get; set; }

        public string Short { get; set; }

        public long RedirectsCount { get; set; }
    }
}
