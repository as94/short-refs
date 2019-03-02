namespace ShortRefs.Domain.Models.Services
{
    public interface IReferenceService
    {
        string GetShortReference(string originalReference);

        string GetOriginalReference(string shortReference);
    }
}
