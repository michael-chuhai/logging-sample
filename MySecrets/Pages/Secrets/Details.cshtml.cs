using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using MySecrets.Data.Entities;
using MySecrets.Interfaces;

namespace MySecrets.Pages.Secrets
{
    [Authorize]
    public class DetailsModel : PageModel
    {
        private readonly ISecretService _secretService;
        private readonly ILogger<DetailsModel> _logger;


        public Secret Secret { get; set; }


        public DetailsModel(
            ISecretService secretService,
            ILogger<DetailsModel> logger)
        {
            _secretService = secretService;
            _logger = logger;
        }


        public async Task<IActionResult> OnGetAsync(int? id)
        {
            _logger.LogInformation("Details for secret #{secretId} are requested.", id);

            if (id == null)
            {
                _logger.LogInformation("Couldn't find details for empty id");

                return NotFound();
            }

            _logger.LogInformation("Loading the secret details");

            Secret = await _secretService.GetAsync(id.Value);

            if (Secret == null)
            {
                _logger.LogInformation("Couldn't find the secret");

                return NotFound();
            }

            _logger.LogInformation("The secret details are successfully loaded.");

            return Page();
        }
    }
}
