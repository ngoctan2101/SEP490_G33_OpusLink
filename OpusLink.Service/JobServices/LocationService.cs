using OpusLink.Entity.Models;
using OpusLink.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace OpusLink.Service.JobServices
{
    public interface ILocationService
    {
        Task<List<Location>> GetAllLocation();
    }
    public class LocationService : ILocationService
    {
        private readonly OpusLinkDBContext _dbContext;
        public LocationService(OpusLinkDBContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<List<Location>> GetAllLocation()
        {
            return await _dbContext.Locations
                .ToListAsync();
        }
    }
}
