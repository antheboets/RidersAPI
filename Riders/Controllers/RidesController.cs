using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Riders.Data;

namespace Riders.Controllers
{
    [Route("[controller]")]
    [Authorize]
    [ApiController]
    public class RidesController : ControllerBase
    {
        private readonly RidersContext _context;
        private readonly Infrastructure.IRideRepository Repo;
        public RidesController(RidersContext context, Infrastructure.IRideRepository Repo)
        {
            _context = context;
            this.Repo = Repo;
        }
        [HttpGet]
        public async Task<ActionResult<ICollection<Models.Ride>>> GetRide()
        {
            ICollection<Models.Ride> list = await Repo.GetAll();
            ICollection<Dto.RideForGet> listDto = new List<Dto.RideForGet>();
            foreach(Models.Ride item in list)
            {
                listDto.Add(new Dto.RideForGet { Id= item.Id, From = item.From, To=item.To});
            }
            return Ok(listDto);
        }
        [HttpGet("{id}")]
        public async Task<ActionResult<Dto.RideForGet>> GetRide(string id)
        {
            if (id.Equals(""))
            {
                return BadRequest();
            }
            Models.Ride ride = await Repo.GetById(id);
            if (ride == null)
            {
                return BadRequest();
            }
            return Ok(new Dto.RideForGet { Id = ride.Id, From = ride.From, To = ride.To });
        }
        // PUT: api/Rides/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see https://aka.ms/RazorPagesCRUD.
        [HttpPut("{id}")]
        public async Task<IActionResult> PutRide([FromBody]Dto.RideForPut ride,[FromRoute]string id)
        {
            if (ride == null)
            {
                return BadRequest();
            }
            Models.Ride RideToUpdate = new Models.Ride { Id = id, From = ride.From, To = ride.To };
            if (await Repo.Update(RideToUpdate))
            {
                return Ok();
            }
            return BadRequest();
        }
        // POST: api/Rides
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see https://aka.ms/RazorPagesCRUD.
        [HttpPost]
        public async Task<ActionResult> PostRide(Dto.RideForPost ride)
        {
            if (ride == null)
            {
                return BadRequest();
            }
            if (ride.From == null)
            {
                return BadRequest();
            }
            if (ride.From.Equals(""))
            {
                return BadRequest();
            }
            if (ride.To == null)
            {
                return BadRequest();
            }
            if (ride.To.Equals(""))
            {
                return BadRequest();
            }
            if (await Repo.Insert(new Models.Ride { From=ride.From, To=ride.To }))
            {
                return Ok();
            }
            return BadRequest();
        }
        // DELETE: api/Rides/5
        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteRide(string id)
        {
            if (id.Equals(""))
            {
                return BadRequest();
            }
            if (await Repo.Delete(id))
            {
                return Ok();
            }
            return BadRequest();
        }
        private bool RideExists(string id)
        {
            return _context.Ride.Any(e => e.Id == id);
        }
    }
}
