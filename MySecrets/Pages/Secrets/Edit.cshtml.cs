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
    public class EditModel : PageModel
    {
        private readonly ISecretService _secretService;
        private readonly ILogger<EditModel> _logger;


        [BindProperty]
        public Secret Secret { get; set; }


        public EditModel(
            ISecretService secretService,
            ILogger<EditModel> logger)
        {
            _secretService = secretService;
            _logger = logger;
        }


        public async Task<IActionResult> OnGetAsync(int? id)
        {
            _logger.LogInformation("Loading details for secret #{secretId} initiated", id);
            if (id == null)
            {
                _logger.LogInformation("Passed empty id. Cannot return anything but 404");

                return NotFound();
            }

            _logger.LogInformation("Loading details for the secret");

            Secret = await _secretService.GetAsync(id.Value);

            if (Secret == null)
            {
                _logger.LogInformation("Secret #{id} isn't found", id);

                return NotFound();
            }

            _logger.LogInformation("Secret found and loaded.");

            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            _logger.LogInformation("Updating secret {secretId}", Secret.Id);

            await _secretService.UpdateAsync(Secret);

            _logger.LogInformation("Secret updated");

            return RedirectToPage("./Index");
        }
    }
}
