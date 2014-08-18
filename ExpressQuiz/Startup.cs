using System;
using System.Data.Entity.Migrations;
using System.Data.Entity.Validation;
using System.Diagnostics;
using System.Linq;
using ExpressQuiz.Migrations;
using ExpressQuiz.Models;
using Microsoft.Ajax.Utilities;
using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(ExpressQuiz.Startup))]
namespace ExpressQuiz
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);

        }

    }
}
