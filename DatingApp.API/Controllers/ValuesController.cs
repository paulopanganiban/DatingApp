using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DatingApp.API.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DatingApp.API.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class ValuesController : ControllerBase //Controller is for c# View, because angular is our view kaya ControllerBase.
    {
        private readonly DataContext _context;
        public ValuesController(DataContext context)
        {
            _context = context;

        }
        // GET api/values
        //for the meantime
        [AllowAnonymous]
        [HttpGet]
        // public ActionResult<IEnumerable<string>> Get()
        //return http responses to the client. just returning strings, we can return ok
        public async Task<IActionResult> GetValues()
        {
            //put await on call of the context
            var values = await _context.Values.ToListAsync();
            return Ok(values);

           // return new string[] { "value1", "fucky" };
        }

        // GET api/values/5
        [AllowAnonymous]
        [HttpGet("{id}")]
        // public ActionResult<string> Get(int id)
        public async Task<IActionResult> GetValue(int id)
        {
            var value = await _context.Values.FirstOrDefaultAsync(x => x.Id == id);
            return Ok(value);
        }

        // POST api/values
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        // PUT api/values/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
