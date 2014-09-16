using Microsoft.Owin;
using Owin;

[assembly: OwinStartup(typeof (ExpressQuiz.Startup))]

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