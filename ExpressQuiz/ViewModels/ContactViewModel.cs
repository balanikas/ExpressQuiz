using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using ExpressQuiz.Core.Models;

namespace ExpressQuiz.ViewModels
{
    public class ContactViewModel
    {
        public ContactInfo ContactInfo { get; set; }

        public bool WasSent { get; set; }

    }
}