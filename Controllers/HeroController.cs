
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using heroApi.Models;
using heroApi.Repositories;
// using heroApi.Models.HeroSearchPayload;

namespace heroApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HeroController : ControllerBase
    {
        private readonly IRepositoryWrapper _repositoryWrapper;

        public HeroController(IRepositoryWrapper RW)
        {
            _repositoryWrapper = RW;
        }

        // GET: api/Hero
        // [HttpGet]
        // public async Task<ActionResult<IEnumerable<HeroItemDTO>>> GetHeroItems()
        // {
          
        //     return await _repositoryWrapper.HeroItems
        //     .Select(x => ItemToDTO(x))
        //     .ToListAsync();
        // }
        // GET: api/TodoItems
        [HttpGet]
        public async Task<ActionResult<IEnumerable<HeroItemDTO>>> GetHeroItems()
        {
            var heroItems =  await _repositoryWrapper.HeroItem.FindAllAsync();
            return heroItems
                .Select(x => ItemToDTO(x))
                .ToList();
        }

        // GET: api/Hero/5
        [HttpGet("{id}")]
        public async Task<ActionResult<HeroItemDTO>> GetHeroItem(int id)
        {
        
            var heroItem = await _repositoryWrapper.HeroItem.FindByIDAsync(id);

            if (heroItem == null)
            {
                return NotFound();
            }

            return ItemToDTO(heroItem);
        }

        // PUT: api/Hero/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutHeroItem(int id, HeroItemDTO heroItemDTO)
        {
            if (id != heroItemDTO.Id)
            {
                return BadRequest();
            }

            var heroItem = await _repositoryWrapper.HeroItem.FindByIDAsync(id);
            if (heroItem == null)
            {
                return NotFound();
            }

            heroItem.Name = heroItemDTO.Name;
            heroItem.Address = heroItemDTO.Address;

            try
            {
                await _repositoryWrapper.HeroItem.UpdateAsync(heroItem);
            }
            catch (DbUpdateConcurrencyException) when (!HeroItemExists(id))
            {
                return NotFound();
            }

            return NoContent();
        }

        // POST: api/Hero
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<HeroItemDTO>> PostHeroItem(HeroItemDTO heroItemDTO)
        {
          var heroItem = new HeroItem
        {
            Address = heroItemDTO.Address,
            Name = heroItemDTO.Name
        };

        
        await _repositoryWrapper.HeroItem.CreateAsync(heroItem, true);

        return CreatedAtAction(
            nameof(GetHeroItem),
            new { id = heroItem.Id },
            ItemToDTO(heroItem));
        }

        // DELETE: api/Hero/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteHeroItem(int id)
        {
            if (_repositoryWrapper.HeroItem == null)
            {
                return NotFound();
            }
            var heroItem = await _repositoryWrapper.HeroItem.FindByIDAsync(id);
            if (heroItem == null)
            {
                return NotFound();
            }

            // _repositoryWrapper.HeroItem.Delete(heroItem);
            await _repositoryWrapper.HeroItem.DeleteAsync(heroItem, true);

            return NoContent();
        }
        [HttpPost("search/{term}")]
        public async Task<ActionResult<IEnumerable<HeroItemDTO>>>  SearchHero(string term)
        {
            var empList = await _repositoryWrapper.HeroItem.SearchHero(term);
            return Ok(empList);           
        }

        [HttpPost("searchhero")]
        public async Task<ActionResult<IEnumerable<HeroItemDTO>>>  SearchHeroMultiple(HeroSearchPayload SearchObj)
        {
            var empList = await _repositoryWrapper.HeroItem.SearchHeroMultiple(SearchObj);
            return Ok(empList);           
        }


         private bool HeroItemExists(long id)
        {
            return _repositoryWrapper.HeroItem.IsExists(id);
        }

        private static HeroItemDTO ItemToDTO(HeroItem heroItem) =>
            new HeroItemDTO
            {
                Id = heroItem.Id,
                Name = heroItem.Name,
                Address = heroItem.Address
            };
    }
}




// using Microsoft.AspNetCore.Mvc;
// using Microsoft.EntityFrameworkCore;
// using heroApi.Models;

// namespace heroApi.Controllers
// {
//     [Route("api/[controller]")]
//     [ApiController]
//     public class HeroController : ControllerBase
//     {
//         private readonly HeroContext _context;

//         public HeroController(HeroContext context)
//         {
//             _context = context;
//         }

//         // GET: api/TodoItems
//         [HttpGet]
//         public async Task<ActionResult<IEnumerable<HeroItemDTO>>> GetHeroItems()
//         {
//             return await _context.HeroItems
//                 .Select(x => ItemToDTO(x))
//                 .ToListAsync();
//         }

//         // GET: api/TodoItems/5
//         [HttpGet("{id}")]
//         public async Task<ActionResult<HeroItemDTO>> GetHeroItem(long id)
//         {
//             var heroItem = await _context.HeroItems.FindAsync(id);

//             if (heroItem == null)
//             {
//                 return NotFound();
//             }

//             return ItemToDTO(heroItem);
//         }
//         // PUT: api/TodoItems/5
//         // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
//         [HttpPut("{id}")]
//         public async Task<IActionResult> UpdateHeroItem(long id, HeroItemDTO heroItemDTO)
//         {
//             if (id != heroItemDTO.Id)
//             {
//                 return BadRequest();
//             }

//             var heroItem = await _context.HeroItems.FindAsync(id);
//             if (heroItem == null)
//             {
//                 return NotFound();
//             }

//             heroItem.Name = heroItemDTO.Name;
//             heroItem.Address = heroItemDTO.Address;

//             try
//             {
//                 await _context.SaveChangesAsync();
//             }
//             catch (DbUpdateConcurrencyException) when (!HeroItemExists(id))
//             {
//                 return NotFound();
//             }

//             return NoContent();
//         }
//         // POST: api/TodoItems
//         // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
//         [HttpPost]
//         public async Task<ActionResult<HeroItemDTO>> CreateHeroItem(HeroItemDTO heroItemDTO)
//         {
//             var heroItem = new HeroItem
//             {
//                  Address= heroItemDTO.Address,
//                 Name = heroItemDTO.Name
//             };

//             _context.HeroItems.Add(heroItem);
//             await _context.SaveChangesAsync();

//             return CreatedAtAction(
//                 nameof(GetHeroItem),
//                 new { id = heroItem.Id },
//                 ItemToDTO(heroItem));
//         }

//         // DELETE: api/TodoItems/5
//         [HttpDelete("{id}")]
//         public async Task<IActionResult> DeleteHeroItem(long id)
//         {
//             var heroItem = await _context.HeroItems.FindAsync(id);

//             if (heroItem == null)
//             {
//                 return NotFound();
//             }

//             _context.HeroItems.Remove(heroItem);
//             await _context.SaveChangesAsync();

//             return NoContent();
//         }

//         private bool HeroItemExists(long id)
//         {
//             return _context.TodoItems.Any(e => e.Id == id);
//         }

//         private static HeroItemDTO ItemToDTO(HeroItem heroItem) =>
//             new HeroItemDTO
//             {
//                 Id = heroItem.Id,
//                 Name = heroItem.Name,
//                 Address = heroItem.Address
//             };
//     }
// }

// // using System;
// // using System.Collections.Generic;
// // using System.Linq;
// // using System.Threading.Tasks;
// // using Microsoft.AspNetCore.Http;
// // using Microsoft.AspNetCore.Mvc;
// // using Microsoft.EntityFrameworkCore;
// // using heroApi.Models;

// // namespace heroApi.Controllers
// // {
// //     [Route("api/[controller]")]
// //     [ApiController]
// //     public class HeroController : ControllerBase
// //     {
// //         private readonly HeroContext _context;

// //         public HeroController(HeroContext context)
// //         {
// //             _context = context;
// //         }

// //         // GET: api/Hero
// //         [HttpGet]
// //         public async Task<ActionResult<IEnumerable<HeroItem>>> GetHeroItems()
// //         {
// //           if (_context.HeroItems == null)
// //           {
// //               return NotFound();
// //           }
// //             return await _context.HeroItems.ToListAsync();
// //         }

// //         // GET: api/Hero/5
// //         [HttpGet("{id}")]
// //         public async Task<ActionResult<HeroItem>> GetHeroItem(int id)
// //         {
// //           if (_context.HeroItems == null)
// //           {
// //               return NotFound();
// //           }
// //             var heroItem = await _context.HeroItems.FindAsync(id);

// //             if (heroItem == null)
// //             {
// //                 return NotFound();
// //             }

// //             return heroItem;
// //         }

// //         // PUT: api/Hero/5
// //         // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
// //         [HttpPut("{id}")]
// //         public async Task<IActionResult> PutHeroItem(int id, HeroItem heroItem)
// //         {
// //             if (id != heroItem.Id)
// //             {
// //                 return BadRequest();
// //             }

// //             _context.Entry(heroItem).State = EntityState.Modified;

// //             try
// //             {
// //                 await _context.SaveChangesAsync();
// //             }
// //             catch (DbUpdateConcurrencyException)
// //             {
// //                 if (!HeroItemExists(id))
// //                 {
// //                     return NotFound();
// //                 }
// //                 else
// //                 {
// //                     throw;
// //                 }
// //             }

// //             return NoContent();
// //         }

// //         // POST: api/Hero
// //         // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
// //         [HttpPost]
// //         public async Task<ActionResult<HeroItem>> PostHeroItem(HeroItem heroItem)
// //         {
// //           if (_context.HeroItems == null)
// //           {
// //               return Problem("Entity set 'HeroContext.HeroItems'  is null.");
// //           }
// //             _context.HeroItems.Add(heroItem);
// //             await _context.SaveChangesAsync();

// //             return CreatedAtAction("GetHeroItem", new { id = heroItem.Id }, heroItem);
// //         }

// //         // DELETE: api/Hero/5
// //         [HttpDelete("{id}")]
// //         public async Task<IActionResult> DeleteHeroItem(int id)
// //         {
// //             if (_context.HeroItems == null)
// //             {
// //                 return NotFound();
// //             }
// //             var heroItem = await _context.HeroItems.FindAsync(id);
// //             if (heroItem == null)
// //             {
// //                 return NotFound();
// //             }

// //             _context.HeroItems.Remove(heroItem);
// //             await _context.SaveChangesAsync();

// //             return NoContent();
// //         }

// //         private bool HeroItemExists(int id)
// //         {
// //             return (_context.HeroItems?.Any(e => e.Id == id)).GetValueOrDefault();
// //         }
// //     }
// // }
