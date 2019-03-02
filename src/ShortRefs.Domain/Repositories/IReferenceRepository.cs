namespace ShortRefs.Domain.Repositories
{
    using System.Threading.Tasks;

    using ShortRefs.Domain.Models.References;
    using ShortRefs.Domain.Models.Users;

    public interface IReferenceRepository
    {
        Task<Reference> GetAsync(int id);

        Task<Reference> FirstOrDefaultAsync(ReferenceQuery query);

        Task CreateAsync(Reference reference);

        Task UpdateAsync(Reference reference);

        Task DeleteAsync(Reference reference);
    }
}
