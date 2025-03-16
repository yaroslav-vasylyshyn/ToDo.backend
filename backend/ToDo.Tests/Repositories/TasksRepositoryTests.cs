using Microsoft.EntityFrameworkCore;
using ToDo.Core.Context;
using ToDo.Core.Models;
using ToDo.Infrastructure.Repositories;

namespace ToDo.Tests.Repositories
{
    public class TasksRepositoryTests
    {
        private AppDbContext GetDbContext()
        {
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString()) 
                .Options;

            var context = new AppDbContext(options);
            return context;
        }

        [Fact]
        public async Task GetAllAsync_ReturnsAllTasks_WhenNoStatusProvided()
        {
            var context = GetDbContext();
            context.Tasks.AddRange(
                new Tasks { Id = 1, Name = "Task 1", Status = "Done", Description = "Desc 1" },
                new Tasks { Id = 2, Name = "Task 2", Status = "Done", Description = "Desc 2" }
            );
            await context.SaveChangesAsync();
            var repository = new TasksRepository(context);

            var result = await repository.GetAllAsync();

            Assert.Equal(2, result.Count());
        }

        [Fact]
        public async Task GetAllAsync_ReturnsFilteredTasks_WhenStatusProvided()
        {
            var context = GetDbContext();
            context.Tasks.AddRange(
                new Tasks { Id = 1, Name = "Task 1", Status = "Done", Description = "Desc 1" },
                new Tasks { Id = 2, Name = "Task 2", Status = "To Do", Description = "Desc 2" }
            );
            await context.SaveChangesAsync();
            var repository = new TasksRepository(context);

            var result = await repository.GetAllAsync("Done");

            Assert.Single(result);
            Assert.Equal("Done", result.First().Status);
        }

        [Fact]
        public async Task GetByIdAsync_ReturnsTask_WhenTaskExists()
        {
            var context = GetDbContext();
            context.Tasks.Add(new Tasks { Id = 1, Name = "Task 1", Status = "Done", Description = "Desc 1" });
            await context.SaveChangesAsync();
            var repository = new TasksRepository(context);

            var task = await repository.GetByIdAsync(1);

            Assert.NotNull(task);
            Assert.Equal("Task 1", task.Name);
        }

        [Fact]
        public async Task GetByIdAsync_ReturnsNull_WhenTaskDoesNotExist()
        {
            var context = GetDbContext();
            var repository = new TasksRepository(context);

            var task = await repository.GetByIdAsync(999);

            Assert.Null(task);
        }

        [Fact]
        public async Task AddAsync_AddsTaskSuccessfully()
        {
            var context = GetDbContext();
            var repository = new TasksRepository(context);
            var newTask = new Tasks { Id = 1, Name = "New Task", Status = "Done", Description = "Test Desc" };

            await repository.AddAsync(newTask);
            var task = await context.Tasks.FindAsync(1);

            Assert.NotNull(task);
            Assert.Equal("New Task", task.Name);
        }

        [Fact]
        public async Task UpdateAsync_UpdatesTask_WhenTaskExists()
        {
            var context = GetDbContext();
            context.Tasks.Add(new Tasks { Id = 1, Name = "Old Task", Status = "Done", Description = "Old Desc" });
            await context.SaveChangesAsync();
            var repository = new TasksRepository(context);
            var updatedTask = new Tasks { Name = "Updated Task", Status = "Done", Description = "Updated Desc" };

            var result = await repository.UpdateAsync(1, updatedTask);

            Assert.NotNull(result);
            Assert.Equal("Updated Task", result.Name);
            Assert.Equal("Done", result.Status);
            Assert.Equal("Updated Desc", result.Description);
        }

        [Fact]
        public async Task UpdateAsync_ReturnsNull_WhenTaskDoesNotExist()
        {
            var context = GetDbContext();
            var repository = new TasksRepository(context);

            var updatedTask = new Tasks { Name = "Updated Task", Status = "Done", Description = "Updated Desc" };

            var result = await repository.UpdateAsync(999, updatedTask);

            Assert.Null(result);
        }

        [Fact]
        public async Task DeleteAsync_DeletesTask_WhenTaskExists()
        {
            var context = GetDbContext();
            context.Tasks.Add(new Tasks { Id = 1, Name = "Task to Delete", Status = "Done", Description = "Desc" });
            await context.SaveChangesAsync();
            var repository = new TasksRepository(context);

            var deletedTask = await repository.DeleteAsync(1);
            var taskInDb = await context.Tasks.FindAsync(1);

            Assert.NotNull(deletedTask);
            Assert.Null(taskInDb);
        }

        [Fact]
        public async Task DeleteAsync_ReturnsNull_WhenTaskDoesNotExist()
        {
            var context = GetDbContext();
            var repository = new TasksRepository(context);

            var result = await repository.DeleteAsync(999);

            Assert.Null(result);
        }
    }
}
