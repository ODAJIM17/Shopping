using Shopping.Common;
using Shopping.ViewModels;

namespace Shopping.Helpers
{
    public interface IOrdersHelper
    {
        Task<Response> ProcessOrderAsync(ShowCartViewModel model);
    }
}
