namespace ShortRefs.Domain.Repositories
{
    using System.Threading;
    using System.Threading.Tasks;

    public interface ISequenceCounterRepository
    {
        Task<bool> CreateIfNotExists(string sequenceId);

        Task<long> GetNextIdAsync(string sequenceId, CancellationToken cancellationToken);
    }
}
