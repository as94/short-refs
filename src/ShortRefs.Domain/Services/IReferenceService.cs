namespace ShortRefs.Domain.Services
{
    using System.Collections.Generic;
    using System.Threading;
    using System.Threading.Tasks;

    using ShortRefs.Domain.Models.References;

    public interface IReferenceService
    {
        Task<IReadOnlyCollection<Reference>> FindAsync(ReferenceQuery query, CancellationToken cancellationToken);

        Task<string> GetOriginalReferenceAsync(string shortReference, CancellationToken cancellationToken);

        Task<Reference> CreateReferenceAsync(string originalReference, CancellationToken cancellationToken);
    }
}
