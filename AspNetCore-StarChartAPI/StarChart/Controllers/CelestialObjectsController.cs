using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using StarChart.Data;
using StarChart.Models;

namespace StarChart.Controllers
{
    [Route("api/[CelestialObjects]")]
    [ApiController]
    public class CelestialObjectsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public CelestialObjectsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: CelestialObjects
        public async Task<IActionResult> Index()
        {
            return View(await _context.CelestialObjects.ToListAsync());
        }

        // GET: CelestialObjects/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var celestialObject = await _context.CelestialObjects
                .FirstOrDefaultAsync(m => m.Id == id);
            if (celestialObject == null)
            {
                return NotFound();
            }

            return View(celestialObject);
        }

        // GET: CelestialObjects/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: CelestialObjects/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,OrbitedObjectId,OrbitalPeriod")] CelestialObject celestialObject)
        {
            if (ModelState.IsValid)
            {
                _context.Add(celestialObject);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(celestialObject);
        }

        // GET: CelestialObjects/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var celestialObject = await _context.CelestialObjects.FindAsync(id);
            if (celestialObject == null)
            {
                return NotFound();
            }
            return View(celestialObject);
        }

        // POST: CelestialObjects/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,OrbitedObjectId,OrbitalPeriod")] CelestialObject celestialObject)
        {
            if (id != celestialObject.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(celestialObject);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CelestialObjectExists(celestialObject.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(celestialObject);
        }

        // GET: CelestialObjects/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var celestialObject = await _context.CelestialObjects
                .FirstOrDefaultAsync(m => m.Id == id);
            if (celestialObject == null)
            {
                return NotFound();
            }

            return View(celestialObject);
        }

        // POST: CelestialObjects/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var celestialObject = await _context.CelestialObjects.FindAsync(id);
            _context.CelestialObjects.Remove(celestialObject);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        [HttpPut("{id}")]
        public async Task<IActionResult> PutCelestialObject(int id, CelestialObject celestialObject)
        {
            if (id != celestialObject.Id)
            {
                return BadRequest();
            }

            _context.Entry(celestialObject).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CelestialObjectExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCelestialObject(int id)
        {
            var celestialObject = await _context.CelestialObjects.FindAsync(id);
            if (celestialObject == null)
            {
                return NotFound();
            }

            _context.CelestialObjects.Remove(celestialObject);
            await _context.SaveChangesAsync();

            return NoContent();
        }
        private static CelestialObjectDTO CelestialObjectToDTO(CelestialObject celestialObject) =>
        new CelestialObjectDTO
        {
            Id = celestialObject.Id,
            Name = celestialObject.Name
        };
        [HttpGet]
        public async Task<ActionResult<IEnumerable<CelestialObjectDTO>>> GetCelestialObjects()
        {
            return await _context.CelestialObjects
                .Select(x => CelestialObjectToDTO(x))
                .ToListAsync();
        }
        [HttpGet("{id}")]
        public async Task<ActionResult<CelestialObjectDTO>> GetCelestialObject(int id)
        {
            var celestialObject = await _context.CelestialObjects.FindAsync(id);

            if (celestialObject == null)
            {
                return NotFound();
            }

            return CelestialObjectToDTO(celestialObject);
        }
        [HttpGet("{name}")]
        public async Task<ActionResult<CelestialObjectDTO>> GetCelestialObjectByName(String name)
        {
            var celestialObject = await _context.CelestialObjects.FindAsync(name);
          
            if (celestialObject == null)
            {
                return NotFound();
            }

            return CelestialObjectToDTO(celestialObject);
        }
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateCelestialObject(int id, CelestialObjectDTO celestialObjectDTO)
        {
            if (id != celestialObjectDTO.Id)
            {
                return BadRequest();
            }

            var celestialObject = await _context.CelestialObjects.FindAsync(id);
            if (celestialObject == null)
            {
                return NotFound();
            }

            celestialObject.Name = celestialObjectDTO.Name;
            celestialObject.OrbitedObjectId = celestialObjectDTO.OrbitedObjectId;
            celestialObject.Satellites = celestialObjectDTO.Satellites;
            celestialObject.OrbitalPeriod = celestialObjectDTO.OrbitalPeriod;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException) when (!CelestialObjectExists(id))
            {
                return NotFound();
            }

            return NoContent();
        }
        [HttpPatch("{id}/{name}")]
        public async Task<IActionResult> RenameCelestialObject(int id, CelestialObjectDTO celestialObjectDTO, String name)
        {
            if (id != celestialObjectDTO.Id)
            {
                return BadRequest();
            }

            var celestialObject = await _context.CelestialObjects.FindAsync(id);
            if (celestialObject == null)
            {
                return NotFound();
            }

            celestialObject.Name = name;
           

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException) when (!CelestialObjectExists(id))
            {
                return NotFound();
            }

            return NoContent();
        }

            [HttpPost]
        public async Task<ActionResult<CelestialObjectDTO>> CreateTodoItem(CelestialObjectDTO celestialObjectDTO)
        {
            var celestialObject = new CelestialObject
            {
                Name = celestialObjectDTO.Name,
            OrbitedObjectId = celestialObjectDTO.OrbitedObjectId,
            Satellites = celestialObjectDTO.Satellites,
            OrbitalPeriod = celestialObjectDTO.OrbitalPeriod
        };

            _context.CelestialObjects.Add(celestialObject);
            await _context.SaveChangesAsync();

            return CreatedAtAction(
                nameof(GetCelestialObject),
                new { id = celestialObject.Id },
                CelestialObjectToDTO(celestialObject));
        }

        private bool CelestialObjectExists(int id)
        {
            return _context.CelestialObjects.Any(e => e.Id == id);
        }
    }
}
