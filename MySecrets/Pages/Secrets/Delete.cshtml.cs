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
    public class DeleteModel : PageModel
    {
        private readonly ISecretService _secretService;
        private readonly ILogger<DeleteModel> _logger;


        [BindProperty]
        public Secret Secret { get; set; }


        public DeleteModel(
            ISecretService secretService,
            ILogger<DeleteModel> logger)
        {
            _secretService = secretService;
            _logger = logger;
        }


        public async Task<IActionResult> OnGetAsync(int? id)
        {
            _logger.LogInformation("Navigated to deleting page. Loading details for the secret #{id}", id);

            if (id == null)
            {
                _logger.LogInformation("Passed empty id");

                return NotFound();
            }

            _logger.LogInformation("Loading details for secret");

            Secret = await _secretService.GetAsync(id.Value);

            if (Secret == null)
            {
                _logger.LogInformation("Secret isn't found.");

                return NotFound();
            }

            _logger.LogInformation("Secret loaded. Rendering the page.");

            return Page();
        }

        public async Task<IActionResult> OnPostAsync(int? id)
        {
            _logger.LogInformation("Delete secret #{id} request is posted.");

            if (id == null)
            {
                _logger.LogInformation("Passed empty id");

                return NotFound();
            }

            _logger.LogInformation("Deleting secret...");

            await _secretService.DeleteAsync(id.Value);

            _logger.LogInformation("The secret is deleted.");

            return RedirectToPage("./Index");
        }
    }
}
