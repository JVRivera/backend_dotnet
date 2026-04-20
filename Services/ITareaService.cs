using BackendTareas.Models;

namespace BackendTareas.Services
{
    public interface ITareaService
    {
        Task<List<Tarea>> ObtenerTareas(string estado);
        Task<Tarea> CrearTarea(Tarea tarea);
        Task<Tarea> ActualizarTarea(Tarea tarea);
        Task<bool> EliminarTarea(int id);
    }
}