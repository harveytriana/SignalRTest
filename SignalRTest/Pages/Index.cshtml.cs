using Microsoft.AspNetCore.Mvc.RazorPages;
using SignalRTest.Shared;

namespace SignalRTest.Pages
{
    public class IndexModel : PageModel
    {
        //Tracer _tracer;

        public void OnGet(Tracer tracer)
        {
            //tracer.Start("SignalRTest_Index_{Date}");

            //tracer.Log("This is a Tracer test.");
        }
    }
}
