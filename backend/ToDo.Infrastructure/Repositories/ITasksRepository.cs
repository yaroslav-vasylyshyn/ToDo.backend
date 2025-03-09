using ToDo.Core.Models;

public interface ITasksRepository
{
    Task<IEnumerable<Tasks>> GetAllAsync();
    Task<Tasks?> GetByIdAsync(int id);
    Task AddAsync(Tasks task);
    Task<Tasks?> UpdateAsync(int id, Tasks task);
    Task<Tasks?> DeleteAsync(int id);
}
