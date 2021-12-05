using BarBeer.Models;
using BarBeer.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BarBeer.Services
{
    public interface IBarService
    {
        IEnumerable<Bar> GetBarsAsync();
        Task<Bar> GetBarByIdAsync(int id);
        Task<int> CreateBarAsync(BarViewModel model);
        Task DeleteBarByIdAsync(int id);
        Task UpdateBarAsync(int id, BarViewModel model);
        Task<IEnumerable<Comment>> GetCommentsByBarIdAsync(int id);
        Task<IEnumerable<Comment>> GetCommentsByUserIdAsync(int id);
        Task<Bar> GetBarByNameAsync(string name);
        Task<IEnumerable<Bar>> GetBarsByRatingAsync(double from, double to);
        Task<IEnumerable<object>> GetPersonalBestBarsByUserIdAsync(int id);
        Task<double> LeaveFeedbackAsync(FeedbackViewModel model);
    }
}
