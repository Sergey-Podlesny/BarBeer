using BarBeer.Models;
using BarBeer.ViewModels;
using BarBeer.ViewModels.ResponseViewModel;
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
        Task<Bar> GetBarByNameAsync(string name);
        Task<int> CreateBarAsync(BarViewModel model);
        Task DeleteBarByIdAsync(int id);
        Task UpdateBarAsync(int id, BarViewModel model);
        Task<IEnumerable<CommentViewModel>> GetCommentsByUserLoginAsync(string login);
        Task<IEnumerable<CommentViewModel>> GetCommentsByBarNameAsync(string name);
        Task<IEnumerable<Bar>> GetBarsByRatingAndNameAsync(double from, double to, string name);
        Task<IEnumerable<object>> GetPersonalBestBarsByUserIdAsync(int id);
        Task<double> LeaveFeedbackAsync(FeedbackViewModel model);
        Task<int> SaveBestBar(int userIdbarId, int userId);
    }
}
