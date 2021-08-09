using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using MySecrets.Data.Entities;
using MySecrets.Interfaces;

namespace MySecrets.Pages.Secrets
{
    [Authorize]
    public class IndexModel : PageModel
    {
        private readonly ISecretService _secretService;
        private readonly ILogger<IndexModel> _logger;


        public IndexModel(
            ISecretService secretService,
            ILogger<IndexModel> logger)
        {
            _secretService = secretService;
            _logger = logger;
        }


        public IReadOnlyCollection<Secret> Secret { get; set; }


        public async Task OnGetAsync()
        {
            _logger.LogInformation("Loading secrets...");

            Secret = await _secretService.GetAsync();

            _logger.LogInformation("Secrets loaded");
        }
    }
}
