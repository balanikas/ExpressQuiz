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
