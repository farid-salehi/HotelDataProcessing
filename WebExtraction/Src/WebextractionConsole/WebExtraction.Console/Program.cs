using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using WebExtraction.Application.Interfaces;

namespace WebExtraction.Console
{
    class Program
    {
        static async Task Main()
        {
            var services = new ServiceCollection();
            ServiceConfiguration.ConfigureServices(services);
            var serviceProvider = services.BuildServiceProvider();
            var hotelInformationJson = await serviceProvider.GetService<IHotelService>().GetHotelDetail();
            System.Console.WriteLine(hotelInformationJson);
            System.Console.ReadLine();

        }

    }
}
