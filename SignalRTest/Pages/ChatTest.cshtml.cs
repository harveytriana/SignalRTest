using System;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace SignalRTest.Pages
{
    public class ChatTestModel : PageModel
    {
        public string UserName { get; set; }

        public void OnGet()
        {
            UserName = $"Page Client {new Random().Next(0, 100):000}";
        }
    }
}
