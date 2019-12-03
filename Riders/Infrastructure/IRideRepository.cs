using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Riders.Infrastructure
{
    public interface IRideRepository
    {
        Task<Models.Ride> GetById(string Id);
        Task<ICollection<Models.Ride>> GetAll();
        Task<bool> Update(Models.Ride ride);
        Task<bool> Delete(string Id);
        Task<bool> Insert(Models.Ride ride);
    }
}
