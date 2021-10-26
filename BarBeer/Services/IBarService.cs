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
        IEnumerable<Bar> GetBars();
        Task<Bar> GetBarById(int id);
        Task<int> CreateBar(BarViewModel model);
        Task DeleteBarById(int id);
        Task UpdateBar(int id, BarViewModel model);

    }
}
