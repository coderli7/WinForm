using Fiddler;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyWebProxy
{
    class Program
    {
        static void Main(string[] args)
        {
            int iListenPort = 9999;
            FiddlerApplication.Startup(iListenPort, FiddlerCoreStartupFlags.Default);

            Fiddler.FiddlerApplication.BeforeRequest += FiddlerApplication_BeforeRequest;
            Fiddler.FiddlerApplication.BeforeResponse += FiddlerApplication_BeforeResponse;
            Console.ReadLine();
        }

        private static void FiddlerApplication_BeforeResponse(Session oSession)
        {
            throw new NotImplementedException();
        }

        private static void FiddlerApplication_BeforeRequest(Session oSession)
        {
            throw new NotImplementedException();
        }
    }
}
