namespace ShortRefs.Domain.Repositories
{
    using System.Threading;
    using System.Threading.Tasks;

    public interface ISequenceCounterRepository
    {
        Task<bool> CreateIfNotExistsAsync(string sequenceId);

        Task<long> GetNextIdAsync(string sequenceId, CancellationToken cancellationToken);
    }
}
