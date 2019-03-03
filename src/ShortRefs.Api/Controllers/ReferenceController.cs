namespace ShortRefs.Api.Controllers
{
    using System.ComponentModel.DataAnnotations;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;

    using ShortRefs.Api.ClientModels;
    using ShortRefs.Domain.Models.References;
    using ShortRefs.Domain.Repositories;
    using ShortRefs.Domain.Services;

    [Authorize]
    [Route("")]
    [Route("references")]
    public class ReferenceController : Controller
    {
        private readonly IReferenceRepository referenceRepository;
        private readonly IReferenceEncoder referenceEncoder;

        private long lastReferenceId;

        public ReferenceController(IReferenceRepository referenceRepository, IReferenceEncoder referenceEncoder)
        {
            this.referenceRepository = referenceRepository;
            this.referenceEncoder = referenceEncoder;

            var count = this.referenceRepository.CountAsync(CancellationToken.None).Result;
            Interlocked.Add(ref this.lastReferenceId, count);
        }

        [HttpGet]
        public async Task<IActionResult> GetReferenceStatAsync(CancellationToken cancellationToken)
        {
            var references = await this.referenceRepository.FindAsync(new ReferenceQuery(), cancellationToken);

            var viewModels = new ReferenceStatList
            {
                Items = references.Select(
                    r => new ReferenceStatItem
                    {
                        Original = r.Original,
                        Short = r.Short,
                        RedirectsCount = r.RedirectsCount
                    })
                .ToArray()
            };
        
            return this.Ok(viewModels);
        }

        [HttpGet]
        [Route("{shortReference}")]
        public async Task<IActionResult> RedirectByOriginalReferenceAsync([Required]string shortReference, CancellationToken cancellationToken)
        {
            var id = this.referenceEncoder.Decode(shortReference);
            var reference = await this.referenceRepository.GetAsync(id, cancellationToken);

            if (reference == null)
            {
                return this.NotFound(shortReference);
            }

            reference.IncrementRedirects();
            await this.referenceRepository.UpdateAsync(reference, cancellationToken);

            return this.Redirect(reference.Original);
        }

        [HttpPost]
        public async Task<IActionResult> CreateReferenceAsync([Required][FromBody]ReferenceCreate referenceCreate, CancellationToken cancellationToken)
        {
            // TODO: lock

            var existingReference = await this.referenceRepository.FirstOrDefaultAsync(
                new ReferenceQuery(referenceCreate.Reference),
                cancellationToken);

            if (existingReference != null)
            {
                return this.BadRequest($"The reference '{referenceCreate.Reference}' already exists");
            }

            Interlocked.Increment(ref this.lastReferenceId);

            var reference =
                Reference.CreateNew(
                    this.lastReferenceId,
                    referenceCreate.Reference,
                    x => this.referenceEncoder.Encode(x));

            await this.referenceRepository.CreateAsync(reference, cancellationToken);

            var viewModel = new ReferenceCreateResult
            {
                Original = reference.Original,
                Short = reference.Short
            };

            return this.Ok(viewModel);
        }
    }
}
