using Microsoft.Owin;
using Owin;
using System.Collections.Generic;
using System.Xml.Linq;
using System.Linq;

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
