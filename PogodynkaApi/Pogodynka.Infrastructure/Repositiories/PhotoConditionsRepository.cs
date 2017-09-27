using Pogodynka.Core.Repositories;
using System;
using System.Collections.Generic;
using System.Text;
using Pogodynka.Core.Domain;
using System.Threading.Tasks;
using Pogodynka.Infrastructure.EF;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace Pogodynka.Infrastructure.Repositiories
{
    public class PhotoConditionsRepository : IPhotoConditionsRepository, ISqlRepository
    {
        private readonly ImagesContext _context;

        public PhotoConditionsRepository(ImagesContext context)
        {
            _context = context;
        }

        public async Task AddAsync(Images photoConditions)
        {
            await _context.Images.AddAsync(photoConditions);
            await _context.SaveChangesAsync();
        }         
       
        public async Task<IEnumerable<Images>> GetAll()
             => await _context.Images.ToListAsync();

        public async Task<Images> RecieveAsync(Images conditions)
        {
            var list = await _context.Images.ToListAsync();
            var result = list.Where(p => p.TempMax + p.FeelsLikeMax <= conditions.TempMax + conditions.FeelsLikeMax + 6 && p.TempMax + p.FeelsLikeMax >= conditions.TempMax + conditions.FeelsLikeMax - 6)
                                         .Where(p => p.TempMin + p.FeelsLikeMin <= conditions.TempMin + conditions.FeelsLikeMin + 6 && p.TempMin + p.FeelsLikeMin >= conditions.TempMin + conditions.FeelsLikeMin - 6)
                                         .OrderBy(p => Math.Abs(p.TempMax - conditions.TempMax))
                                         .ThenBy(p => Math.Abs(p.TempMin - conditions.TempMin))
                                         .FirstOrDefault();
            if (result == null)
            {
                result = list.Where(p => p.TempMax + p.FeelsLikeMax <= conditions.TempMax + conditions.FeelsLikeMax + 10 && p.TempMax + p.FeelsLikeMax >= conditions.TempMax + conditions.FeelsLikeMax - 10)
                                      .Where(p => p.TempMin + p.FeelsLikeMin <= conditions.TempMin + conditions.FeelsLikeMin + 10 && p.TempMin + p.FeelsLikeMin >= conditions.TempMin + conditions.FeelsLikeMin - 10)
                                      .FirstOrDefault();
            }

            if (result == null)
            {
                result = new Images() { ImagePath = "under8.png", TempMax = 7, TempMin = 6, FeelsLikeMax = 7, FeelsLikeMin = 6, Rain = 0 };
            }
            return result;
        }
    }
}
