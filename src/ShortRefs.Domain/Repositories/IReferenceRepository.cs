namespace ShortRefs.Domain.Repositories
{
    using System.Collections.Generic;
    using System.Threading;
    using System.Threading.Tasks;

    using ShortRefs.Domain.Models.References;

    public interface IReferenceRepository
    {
        Task<Reference> GetAsync(long id, CancellationToken cancellationToken);

        Task<Reference> FirstOrDefaultAsync(ReferenceQuery query, CancellationToken cancellationToken);

        Task<IReadOnlyCollection<Reference>> FindAsync(ReferenceQuery query, CancellationToken cancellationToken);

        Task CreateAsync(Reference reference, CancellationToken cancellationToken);

        Task UpdateAsync(Reference reference, CancellationToken cancellationToken);

        Task DeleteAsync(Reference reference, CancellationToken cancellationToken);

        Task<long> CountAsync(CancellationToken cancellationToken);
    }
}
