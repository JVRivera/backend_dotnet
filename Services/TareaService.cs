using BackendTareas.Data;
using BackendTareas.Models;
using Microsoft.EntityFrameworkCore;

namespace BackendTareas.Services
{
    public class TareaService : ITareaService
    {
        private readonly AppDbContext _context;

        public TareaService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<List<Tarea>> ObtenerTareas(string estado)
        {
            if (estado == "todas")
            {
                return await _context.Tareas.ToListAsync();
            }
            else
            {
                return await _context.Tareas
                    .Where(t => t.Estado == estado)
                    .ToListAsync();
            }
        }

        public async Task<Tarea> CrearTarea(Tarea tarea)
        {
            tarea.UsuarioId=1;
            tarea.FechaCreacion = DateTime.UtcNow;
            tarea.FechaActualizacion = DateTime.UtcNow;

            if (tarea.FechaVencimiento.HasValue)
            {
                tarea.FechaVencimiento = DateTime.SpecifyKind(
                    tarea.FechaVencimiento.Value,
                    DateTimeKind.Utc
                );
            }

            tarea.Estado = "pendiente";

            _context.Tareas.Add(tarea);
            await _context.SaveChangesAsync();

            return tarea;
        }        

        public async Task<Tarea> ActualizarTarea(Tarea tarea)
        {
            var tareaExistente = await _context.Tareas.FindAsync(tarea.Id);

            if (tareaExistente == null)
            {
                throw new Exception("Tarea no encontrada");
            }

            tareaExistente.Titulo = tarea.Titulo;
            tareaExistente.Descripcion = tarea.Descripcion;
            tareaExistente.FechaVencimiento = tarea.FechaVencimiento;
            tareaExistente.Estado = tarea.Estado;
            tareaExistente.UsuarioId = 1;

            tareaExistente.FechaActualizacion = DateTime.UtcNow;

            if (tareaExistente.FechaVencimiento.HasValue)
            {
                tareaExistente.FechaVencimiento = DateTime.SpecifyKind(
                    tareaExistente.FechaVencimiento.Value,
                    DateTimeKind.Utc
                );
            }

            await _context.SaveChangesAsync();

            return tareaExistente;
        }

        public async Task<bool> EliminarTarea(int id)
        {
            var tarea = await _context.Tareas.FindAsync(id);

            if (tarea == null)
                return false;

            _context.Tareas.Remove(tarea);
            await _context.SaveChangesAsync();

            return true;
        }             
    }
}