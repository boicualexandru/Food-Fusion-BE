using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using DataAccess.Models;
using Microsoft.EntityFrameworkCore;
using Services.Cuisines.Models;

namespace Services.Cuisines
{
    public class CuisinesService : ICuisinesService
    {
        private readonly FoodFusionContext _dbContext;
        private readonly IMapper _mapper;

        public CuisinesService(FoodFusionContext dbContext, IMapper mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }

        public List<CuisineModel> GetCuisines()
        {
            var cuisines = _dbContext.Cuisine
                .AsNoTracking()
                .ToList();

            return _mapper.Map<List<CuisineModel>>(cuisines);
        }
    }
}
