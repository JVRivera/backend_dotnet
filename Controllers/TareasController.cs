using BackendTareas.Services;
using Microsoft.AspNetCore.Mvc;
using BackendTareas.Models;

namespace BackendTareas.Controllers
{
    [ApiController]
    [Route("api/tareas")]
    public class TareasController : ControllerBase
    {
        private readonly ITareaService _tareaService;

        public TareasController(ITareaService tareaService)
        {
            _tareaService = tareaService;
        }

        [HttpGet("buscar/{estado}")]
        public async Task<IActionResult> ObtenerTareas(string estado)
        {
            var tareas = await _tareaService.ObtenerTareas(estado);
            return Ok(tareas);
        }

        [HttpPost]
        public async Task<IActionResult> CrearTarea([FromBody] Tarea tarea)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);     

            if (tarea.FechaVencimiento < DateTime.Now)
                throw new Exception("La fecha no puede ser menor a la actual");                 

            var nuevaTarea = await _tareaService.CrearTarea(tarea);
            return Ok(nuevaTarea);
        }    

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateTarea(int id, [FromBody] Tarea tarea)
        {
            if (id != tarea.Id)
                return BadRequest("El ID de la URL no coincide con el del body");

            if (!ModelState.IsValid)
                return BadRequest(ModelState);                

            var tareaActualizada = await _tareaService.ActualizarTarea(tarea);
            return Ok(tareaActualizada);
        }    

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTarea(int id)
        {
            var result = await _tareaService.EliminarTarea(id);

            if (!result)
                return NotFound("Tarea no encontrada");

            return Ok(result);
        }                
    }
}