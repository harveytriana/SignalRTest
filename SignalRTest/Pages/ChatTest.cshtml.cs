﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;

namespace SignalRTest.Pages
{
    public class ChatTestModel : PageModel
    {
        private readonly ILogger<ChatTestModel> _logger;

        public ChatTestModel(ILogger<ChatTestModel> logger)
        {
            _logger = logger;
        }

        public void OnGet()
        {

        }
    }
}
