namespace ShortRefs.Domain.Services
{
    using System;
    using System.Threading;
    using System.Threading.Tasks;

    using ShortRefs.Domain.Models.References;

    public interface IReferenceService
    {
        Task<string> GetOriginalReferenceAsync(string shortReference, CancellationToken cancellationToken);

        Task<Reference> CreateReferenceAsync(string originalReference, Guid ownerId, CancellationToken cancellationToken);
    }
}
