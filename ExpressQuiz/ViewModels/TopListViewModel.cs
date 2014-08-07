using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ExpressQuiz.Models;
using ExpressQuiz.Repos;

namespace ExpressQuiz.ViewModels
{
    public class TopListViewModel
    {
       public IEnumerable<TopListItem> TopList { get; set; } 

       
    }
}