using Services.Cuisines.Models;
using System.Collections.Generic;

namespace Services.Cuisines
{
    public interface ICuisinesService
    {
        List<CuisineModel> GetCuisines();
    }
}
