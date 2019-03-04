namespace ShortRefs.Domain.Services
{
    using System;
    using System.Collections.Generic;
    using System.Threading;
    using System.Threading.Tasks;

    using ShortRefs.Domain.Models.References;
    using ShortRefs.Domain.Repositories;

    public sealed class ReferenceService : IReferenceService
    {
        private readonly ISequenceCounterRepository sequenceCounterRepository;
        private readonly IReferenceRepository referenceRepository;
        private readonly IReferenceEncoder referenceEncoder;

        public ReferenceService(
            ISequenceCounterRepository sequenceCounterRepository,
            IReferenceRepository referenceRepository,
            IReferenceEncoder referenceEncoder)
        {
            this.sequenceCounterRepository = sequenceCounterRepository ?? throw new ArgumentNullException(nameof(sequenceCounterRepository));
            this.referenceRepository = referenceRepository ?? throw new ArgumentNullException(nameof(referenceRepository));
            this.referenceEncoder = referenceEncoder ?? throw new ArgumentNullException(nameof(referenceEncoder));
        }

        public async Task<IReadOnlyCollection<Reference>> FindAsync(ReferenceQuery query, CancellationToken cancellationToken)
        {
            return await this.referenceRepository.FindAsync(query, cancellationToken);
        }

        public async Task<string> GetOriginalReferenceAsync(string shortReference, CancellationToken cancellationToken)
        {
            if (shortReference == null)
            {
                throw new ArgumentNullException(nameof(shortReference));
            }

            var id = this.referenceEncoder.Decode(shortReference);
            var reference = await this.referenceRepository.GetAsync(id, cancellationToken);

            if (reference == null)
            {
                return null;
            }

            reference.IncrementRedirects();
            await this.referenceRepository.UpdateAsync(reference, cancellationToken);

            return reference.Original;
        }

        public async Task<Reference> CreateReferenceAsync(string originalReference, CancellationToken cancellationToken)
        {
            if (originalReference == null)
            {
                throw new ArgumentNullException(nameof(originalReference));
            }

            var nextId = await this.sequenceCounterRepository.GetNextIdAsync("referenceId", cancellationToken);

            var reference =
                Reference.CreateNew(
                    nextId,
                    originalReference,
                    x => this.referenceEncoder.Encode(x));

            await this.referenceRepository.CreateAsync(reference, cancellationToken);

            return reference;
        }
    }
}
