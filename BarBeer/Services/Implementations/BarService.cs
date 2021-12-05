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
        public IEnumerable<Bar> GetBarsAsync()
        {
            return dbContext.Bars.AsEnumerable();
        }
        public async Task<Bar> GetBarByIdAsync(int id)
        {
            var bar = await dbContext.Bars.FirstOrDefaultAsync(bar => bar.Id == id);
            return bar;
        }

        public async Task<int> CreateBarAsync(BarViewModel model)
        {
            var bar = new Bar
            {
                BarName = model.BarName,
                BarImage = model.BarImage,
                BarRating = 0,
                BarLocation = model.BarLocation,
            };

            await dbContext.Bars.AddAsync(bar);
            await dbContext.SaveChangesAsync();
            return bar.Id;
        }

        public async Task DeleteBarByIdAsync(int id)
        {
            var bar = await dbContext.Bars.FirstOrDefaultAsync(user => user.Id == id);
            if (bar == null)
            {
                throw new InternalServerErrorException();
            }
            else
            {
                dbContext.Bars.Remove(bar);
                await dbContext.SaveChangesAsync();
            }
        }



        public async Task UpdateBarAsync(int id, BarViewModel model)
        {
            var bar = await dbContext.Bars.FirstOrDefaultAsync(bar => bar.Id == id);
            if (bar == null)
            {
                throw new InternalServerErrorException();
            }
            else
            {
                bar.BarName = model.BarName;
                bar.BarImage = model.BarImage;
                bar.BarLocation = model.BarLocation;
                dbContext.Bars.Update(bar);
                dbContext.SaveChanges();
            }
        }

        public async Task<IEnumerable<Comment>> GetCommentsByBarIdAsync(int id)
        {
            var comments = await dbContext.Comments.Where(c => c.BarId == id).ToListAsync();
            return comments.AsEnumerable();
        }

        public async Task<IEnumerable<Comment>> GetCommentsByUserIdAsync(int id)
        {
            var comments = await dbContext.Comments.Where(c => c.UserId == id).ToListAsync();
            return comments.AsEnumerable();
        }

        public async Task<Bar> GetBarByNameAsync(string name)
        {
            var bar = await dbContext.Bars.FirstOrDefaultAsync(bar => bar.BarName.ToLower() == name.ToLower());
            return bar;
        }

        public async Task<IEnumerable<Bar>> GetBarsByRatingAsync(double fromRating, double toRating)
        {
            var bars = await dbContext.Bars.Where(bar => bar.BarRating >= fromRating && bar.BarRating <= toRating).ToListAsync();
            return bars.AsEnumerable();
        }

        public async Task<IEnumerable<object>> GetPersonalBestBarsByUserIdAsync(int id)
        {
            var bestBars = await dbContext.PersonalBestBars
                .Where(pBar => pBar.UserId == id)
                .Join(dbContext.Bars,
                pBar => pBar.BarId,
                bar => bar.Id,
                (pBar, bar) => new
                {
                    BarName = bar.BarName,
                    BarImage = bar.BarImage,
                    BarRating = bar.BarRating,
                    BarLocation = bar.BarLocation
                }).ToListAsync();

            return bestBars.AsEnumerable();
        }

        public async Task<double> LeaveFeedbackAsync(FeedbackViewModel model)
        {
            var bar = await GetBarByIdAsync(model.BarId);
            var comments = await GetCommentsByBarIdAsync(model.BarId);
            if (bar == null || comments == null)
            {
                throw new InternalServerErrorException();
            }

            bar.BarRating = (bar.BarRating * comments.Count() + model.Rating) / (comments.Count() + 1);

            var newComment = new Comment()
            {
                BarId = model.BarId,
                UserId = model.UserId,
                Text = model.Comment
            };

            dbContext.Bars.Update(bar);
            dbContext.Comments.Add(newComment);

            dbContext.SaveChanges();

            return (double)bar.BarRating;


        }
    }
}
