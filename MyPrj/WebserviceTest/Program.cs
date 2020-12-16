using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebserviceTest
{
    class Program
    {
        static void Main(string[] args)
        {
            ServiceReference1.WeatherServiceClient wsClient = new ServiceReference1.WeatherServiceClient();
            String wsStatus = wsClient.info("北京");
            Console.WriteLine(wsStatus);
        }
    }
}
