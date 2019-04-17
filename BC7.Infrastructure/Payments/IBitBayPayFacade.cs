using System.Threading.Tasks;

namespace BC7.Infrastructure.Payments
{
    public interface IBitBayPayFacade
    {
        Task<string> CreatePayment(); // TODO: parameters will be given during the development
    }
}
