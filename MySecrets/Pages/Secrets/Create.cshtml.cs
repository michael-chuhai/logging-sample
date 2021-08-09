using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using MySecrets.Data.Entities;
using MySecrets.Interfaces;

namespace MySecrets.Pages.Secrets
{
    [Authorize]
    public class CreateModel : PageModel
    {
        private readonly ISecretService _secretService;
        private readonly ILogger<CreateModel> _logger;


        [BindProperty]
        public Secret Secret { get; set; }


        public CreateModel(
            ISecretService secretService,
            ILogger<CreateModel> logger)
        {
            _secretService = secretService;
            _logger = logger;
        }


        public IActionResult OnGet()
        {
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            _logger.LogInformation("Creating secret is initiated");

            if (!ModelState.IsValid)
            {
                var errors = ModelState
                    .Where(kvp => kvp.Value.ValidationState == ModelValidationState.Invalid)
                    .Select(kvp => new {FieldName = kvp.Key, Errors = kvp.Value.Errors.Select(e => e.ErrorMessage)})
                    .ToList();

                _logger.LogWarning("Posted secret wasn't valid. Errors: {@validationErrors}", errors);

                return Page();
            }

            _logger.LogInformation("Creating secret...");

            var createdSecret = await _secretService.CreateAsync(Secret);

            _logger.LogInformation("The secret #{secretId} is successfully created.", createdSecret.Id);

            return RedirectToPage("./Index");
        }
    }
}
