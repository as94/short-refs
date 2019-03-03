namespace ShortRefs.Domain.Repositories
{
    using System.Collections.Generic;
    using System.Threading;
    using System.Threading.Tasks;

    using ShortRefs.Domain.Models.References;
    using ShortRefs.Domain.Models.Users;

    public interface IReferenceRepository
    {
        Task<Reference> GetAsync(int id, CancellationToken cancellationToken);

        Task<Reference> FirstOrDefaultAsync(ReferenceQuery query, CancellationToken cancellationToken);

        Task<IReadOnlyCollection<Reference>> FindAsync(ReferenceQuery query, CancellationToken cancellationToken);

        Task CreateAsync(Reference reference);

        Task UpdateAsync(Reference reference);

        Task DeleteAsync(Reference reference);
    }
}
