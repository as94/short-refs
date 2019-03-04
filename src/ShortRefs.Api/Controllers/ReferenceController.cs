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
        private readonly IReferenceService referenceService;

        public ReferenceController(
            IReferenceRepository referenceRepository,
            IReferenceService referenceService)
        {
            this.referenceRepository = referenceRepository;
            this.referenceService = referenceService;
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
        public async Task<IActionResult> RedirectByShortReferenceAsync([Required]string shortReference, CancellationToken cancellationToken)
        {
            var reference = await this.referenceService.GetOriginalReferenceAsync(shortReference, cancellationToken);
            if (reference == null)
            {
                return this.NotFound(shortReference);
            }

            return this.Redirect(reference);
        }

        [HttpPost]
        public async Task<IActionResult> CreateReferenceAsync([Required][FromBody]ReferenceCreate referenceCreate, CancellationToken cancellationToken)
        {
            var existingReference = await this.referenceRepository.FirstOrDefaultAsync(
                new ReferenceQuery(referenceCreate.Reference),
                cancellationToken);

            if (existingReference != null)
            {
                return this.BadRequest($"The reference '{referenceCreate.Reference}' already exists");
            }

            var reference = await this.referenceService.CreateReferenceAsync(referenceCreate.Reference, cancellationToken);

            var viewModel = new ReferenceCreateResult
            {
                Original = reference.Original,
                Short = reference.Short
            };

            return this.Ok(viewModel);
        }
    }
}
