using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Castle.Windsor;
namespace TaskDemo
{
    public class Startup
    {
        public static void Setup()
        {
            Startup startup = new Startup();
            startup.SetupDI();
        }
        public void SetupDI()
        {
            var container = new WindsorContainer();
        }

    }
}
