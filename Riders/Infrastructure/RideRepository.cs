using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Riders.Models;
using Riders.Data;

namespace Riders.Infrastructure
{
    public class RideRepository : IRideRepository
    {
        private readonly RidersContext RidersContext;
        public RideRepository(RidersContext RidersContext)
        {
            this.RidersContext = RidersContext;
        }
        public async Task<bool> Delete(string Id)
        {
            if (Id.Equals(""))
            {
                return false;
            }
            try
            {
                RidersContext.Ride.Remove(await RidersContext.Ride.Where(x => x.Id == Id).SingleOrDefaultAsync());
                await RidersContext.SaveChangesAsync();
            }
            catch (Exception e)
            {
                return false;
            }
            return true;
        }
        public async Task<ICollection<Ride>> GetAll()
        {
            return await RidersContext.Ride.ToListAsync();
        }
        public async Task<Ride> GetById(string Id)
        {
            if (Id.Equals(""))
            {
                return null;
            }
            try
            {
                return await RidersContext.Ride.Where(x => x.Id == Id).SingleOrDefaultAsync();
            }
            catch (Exception e)
            {
                return null;
            }
        }

        public async Task<bool> Insert(Ride ride)
        {
            if (ride == null)
            {
                return false;
            }
            await RidersContext.Ride.AddAsync(ride);
            await RidersContext.SaveChangesAsync();
            return true;
        }
        public async Task<bool> Update(Ride ride)
        {
            if (ride == null)
            {
                return false;
            }
            if (ride.Id == null)
            {
                return false;
            }
            if (ride.Id.Equals(""))
            {
                return false;
            }
            Models.Ride oldRide = await RidersContext.Ride.Where(x => x.Id == ride.Id).SingleOrDefaultAsync();
            if (ride.From != null)
            {
                if (!ride.From.Equals(""))
                {
                    oldRide.From = ride.From;
                }
            }
            if (ride.To != null)
            {
                if (!ride.To.Equals(""))
                {
                    oldRide.To = ride.To;
                }
            }
            //RidersContext.Update(ride);
            await RidersContext.SaveChangesAsync();
            return true;
        }
    }
}