using BarBeer.Context;
using BarBeer.Exceptions;
using BarBeer.Models;
using BarBeer.ViewModels;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BarBeer.Services.Implementations
{
    public class BarService : IBarService
    {
        private BarBeerContext dbContext;
        public BarService(BarBeerContext context)
        {
            dbContext = context;
        }
        public IEnumerable<Bar> GetBars()
        {
            return dbContext.Bars.AsEnumerable();
        }
        public async Task<Bar> GetBarById(int id)
        {
            return await dbContext.Bars.FirstOrDefaultAsync(bar => bar.Id == id);
        }

        public async Task<int> CreateBar(BarViewModel model)
        {
            var bar = new Bar
            {
                BarName = model.BarName,
                BarImage = model.BarImage,
                BarRating = model.BarRating,
                BarLocation = model.BarLocation,
            };

            await dbContext.AddAsync(bar);
            await dbContext.SaveChangesAsync();
            return bar.Id;
        }

        public async Task DeleteBarById(int id)
        {
            var bar = await dbContext.Bars.FirstOrDefaultAsync(user => user.Id == id);
            if (bar == null)
            {
                throw new NotFoundException();
            }
            else
            {
                dbContext.Bars.Remove(bar);
                await dbContext.SaveChangesAsync();
            }
        }



        public async Task UpdateBar(int id, BarViewModel model)
        {
            if (model == null)
            {
                throw new InvalidModelException();
            }
            else
            {
                var bar = await dbContext.Bars.FirstOrDefaultAsync(bar => bar.Id == id);
                if (bar == null)
                {
                    throw new NotFoundException();
                }
                else
                {

                    bar.BarName = model.BarName;
                    bar.BarImage = model.BarImage;
                    bar.BarRating = model.BarRating;
                    bar.BarLocation = model.BarLocation;
                    try
                    {
                        dbContext.Bars.Update(bar);
                        dbContext.SaveChanges();
                    }
                    catch (Exception ex)
                    {
                        throw new InternalServerErrorException(ex.Message);
                    }
                }
            }
        }
    }
}
