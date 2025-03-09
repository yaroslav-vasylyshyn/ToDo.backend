using Microsoft.EntityFrameworkCore;
using ToDo.Core.Context;
using ToDo.Core.Models;

namespace ToDo.Infrastructure.Repositories;
public class TasksRepository : ITasksRepository
{
    private readonly AppDbContext _context;

    public TasksRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Tasks>> GetAllAsync(){
        var tasks = await _context.Tasks.ToListAsync();
        return tasks;
    }

    public async Task<Tasks?> GetByIdAsync(int id){
        var task = await _context.Tasks.FindAsync(id);
        return task;
    }


    public async Task AddAsync(Tasks task)
    {
        _context.Tasks.Add(task);
        await _context.SaveChangesAsync();
    }

    public async Task<Tasks?> UpdateAsync(int id, Tasks updatedTask)
    {
        var existingTask = await _context.Tasks.FindAsync(id);
        if (existingTask == null)
            return null;

        existingTask.Name = updatedTask.Name;
        existingTask.Status = updatedTask.Status;
        existingTask.Description = updatedTask.Description;

        _context.Tasks.Update(existingTask);
        await _context.SaveChangesAsync();
        return existingTask;
    }

    public async Task<Tasks?> DeleteAsync(int id)
    {
        var task = await _context.Tasks.FindAsync(id);
        if (task == null)
            return null;

        _context.Tasks.Remove(task);
        await _context.SaveChangesAsync();
        return task;
    }
}
