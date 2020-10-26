using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace SignalRTestClient.Pages
{
    [EnableCors("CorsPolicy")]
    public class ClientModel : PageModel
    {
        public void OnGet()
        {
        }
    }
}
