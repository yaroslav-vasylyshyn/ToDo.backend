using Moq;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using ToDo.Api.Controllers;
using ToDo.Core.Models;
using ToDo.Infrastructure.DTO;
using ToDo.Infrastructure.Mappings;

namespace ToDo.Tests.Api
{
    public class TasksControllerTests
    {
        private readonly Mock<ITasksRepository> _mockRepo;
        private readonly IMapper _mapper;
        private readonly TasksController _controller;

        public TasksControllerTests()
        {
            _mockRepo = new Mock<ITasksRepository>();

            var mapperConfig = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile(new MappingProfile()); 
            });
            _mapper = mapperConfig.CreateMapper();

            _controller = new TasksController(_mockRepo.Object, _mapper);
        }

        [Fact]
        public async Task GetAll_ShouldReturnOkResult_WithTasks()
        {
            var tasks = new List<Tasks>
            {
                new Tasks { Id = 1, Name = "Task 1", Description = "Some description", Status = "To Do" },
                new Tasks { Id = 2, Name = "Task 2", Description = "Some description", Status = "Done" }
            };
            _mockRepo.Setup(repo => repo.GetAllAsync(null)).ReturnsAsync(tasks);

            var result = await _controller.GetAll();

            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnTasks = Assert.IsType<List<TaskDto>>(okResult.Value);
            Assert.Equal(2, returnTasks.Count);
        }

        [Fact]
        public async Task GetById_ShouldReturnOkResult_WhenTaskExists()
        {
            var task = new Tasks { Id = 1, Name = "Task 1", Description = "Some description", Status = "To Do" };
            _mockRepo.Setup(repo => repo.GetByIdAsync(1)).ReturnsAsync(task);

            var result = await _controller.GetById(1);

            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnTask = Assert.IsType<TaskDto>(okResult.Value);
            Assert.Equal(task.Id, returnTask.Id);
        }

        [Fact]
        public async Task GetById_ShouldReturnNotFound_WhenTaskDoesNotExist()
        {
            _mockRepo.Setup(repo => repo.GetByIdAsync(1)).ReturnsAsync((Tasks?)null);

            var result = await _controller.GetById(1);

            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public async Task Create_ShouldReturnCreated_WhenTaskIsValid()
        {
            var createDto = new CreateTaskDto{ Name = "Task 1", Description = "Test Description", Status = "To Do"};
            _mockRepo.Setup(repo => repo.AddAsync(It.IsAny<Tasks>())).Returns(Task.CompletedTask);

            var result = await _controller.Create(createDto);

            var createdResult = Assert.IsType<CreatedAtActionResult>(result);
            Assert.Equal("GetById", createdResult.ActionName);
        }

        [Fact]
        public async Task Update_ShouldReturnNoContent_WhenTaskIsUpdated()
        {
            var updateDto = new UpdateTaskDto{ Name = "Task 1", Description = "Test Description", Status = "In progress"};
            var task = _mapper.Map<Tasks>(updateDto);
            _mockRepo.Setup(repo => repo.UpdateAsync(1, It.IsAny<Tasks>())).ReturnsAsync(task);

            var result = await _controller.Update(1, updateDto);

            Assert.IsType<NoContentResult>(result);
        }

        [Fact]
        public async Task Update_ShouldReturnNotFound_WhenTaskDoesNotExist()
        {
            var updateDto = new UpdateTaskDto{ Name = "Non-Existent Task", Description = "Test Description", Status = "Done"};
            _mockRepo.Setup(repo => repo.UpdateAsync(1, It.IsAny<Tasks>())).ReturnsAsync((Tasks?)null);

            var result = await _controller.Update(1, updateDto);

            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public async Task Delete_ShouldReturnNoContent_WhenTaskIsDeleted()
        {
            var task = new Tasks { Id = 1, Name = "Task 1", Description = "Some description", Status = "To Do" };
            _mockRepo.Setup(repo => repo.DeleteAsync(1)).ReturnsAsync(task);

            var result = await _controller.Delete(1);

            Assert.IsType<NoContentResult>(result);
        }

        [Fact]
        public async Task Delete_ShouldReturnNotFound_WhenTaskDoesNotExist()
        {
            _mockRepo.Setup(repo => repo.DeleteAsync(1)).ReturnsAsync((Tasks?)null);

            var result = await _controller.Delete(1);

            Assert.IsType<NotFoundResult>(result);
        }
    }
}
