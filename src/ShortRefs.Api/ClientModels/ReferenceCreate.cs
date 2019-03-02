namespace ShortRefs.Api.ClientModels
{
    using System.ComponentModel.DataAnnotations;

    public sealed class ReferenceCreate
    {
        [Required]
        [Url]
        public string Reference { get; set; }
    }
}
