//using Microsoft.AspNetCore.Authorization;
//using Microsoft.AspNetCore.Mvc;
//using Projektarbeit.DAL;
//using Projektarbeit.Models;
//using Projektarbeit.Services;
//using System.Runtime.InteropServices;
//using xyToolz.Database.Services;

                                                                                                                                                                            // Sample for a generic API Crud controller

//namespace xyToolz.Database.Controllers
//{
//    [Authorize]
//    [Route("api/[controller]")]
//    [ApiController]
//    public class CrudController<T>(CrudService<T> crudService) : ControllerBase where T : class
//    {
//        private readonly CrudService<T> _service = crudService;

//        [HttpGet]
//        public async Task<ActionResult<IEnumerable<T>>> Get()
//        {
//            IEnumerable<T> contents = [];
//            try
//            {
//                contents = await _service.GetAll();
//                return Ok(contents);
//            }
//            catch (Exception ex)
//            {
//                xyLog.ExLog(ex);
//                return StatusCode(500);
//            }
//        }

//        // GET: /api/v1/xxx/"{id}"
//        [HttpGet("{id}")]
//        public async Task<ActionResult<T>> Get(int id)
//        {
//            dynamic? target;
//            string read = $"Entry with ID {id} was found: ";
//            try
//            {
//                target = await _service.GetById(id);
//                xyLog.Log(read);
//                return target is not null ? Ok(target) : StatusCode(404);
//            }
//            catch (Exception ex)
//            {
//                xyLog.ExLog(ex);
//                return StatusCode(500);
//            }
//        }

//        [HttpPost]
//        [Authorize(Roles = "admin")]
//        public async Task<ActionResult> Post([FromBody] T value)
//        {
//            string created = $"Entry created successfully!";
//            try
//            {
//                if (ModelState.IsValid)
//                {
//                    if (await _service.Create(value))
//                    {
//                        return Ok(created);
//                    }
//                }
//            }
//            catch (Exception ex)
//            {
//                xyLog.ExLog(ex);
//            }
//            return StatusCode(500);

//        }

//        [HttpPut("{id}")]
//        [Authorize(Roles = "admin")]
//        public async Task<ActionResult> Put(int id, [FromBody] T value)
//        {
//            string updated = $"Entry updated successfully!";
//            try
//            {
//                if (ModelState.IsValid)
//                {
//                    if (await _service.Update(value))
//                    {
//                        return Ok(updated);
//                    }
//                }
//            }
//            catch (Exception ex)
//            {
//                xyLog.ExLog(ex);
//            }
//            return StatusCode(500);
//        }

//        [HttpDelete("{id}")]
//        [Authorize(Roles = "admin")]
//        public async Task<ActionResult> Delete(int id)
//        {
//            dynamic? target;
//            string removed = $"Entry with ID {id} was removed from database";
//            try
//            {
//                target = await _service.GetById(id);
//                if (target != null)
//                {
//                    if (await _service.Remove(target))
//                    {
//                        return Ok(removed);
//                    }
//                }
//            }
//            catch (Exception ex)
//            {
//                xyLog.ExLog(ex);
//            }
//            return StatusCode(500);
//        }


//    }
//}
