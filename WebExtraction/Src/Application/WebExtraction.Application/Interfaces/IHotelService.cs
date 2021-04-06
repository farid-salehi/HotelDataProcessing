using System.Threading.Tasks;

namespace WebExtraction.Application.Interfaces
{
    public interface IHotelService
    {
        Task<string> GetHotelDetail();
    }
}
