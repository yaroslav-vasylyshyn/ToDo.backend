using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using ToDo.Core.Context;
using ToDo.Core.Models;
using ToDo.Infrastructure.DTO;

namespace ToDo.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TasksController : ControllerBase
    {
        private readonly ITasksRepository _tasksRepository;
        private readonly IMapper _mapper;

        public TasksController(ITasksRepository tasksRepository, IMapper mapper)
        {
            _tasksRepository = tasksRepository;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var tasks = await _tasksRepository.GetAllAsync();
            var tasksDto = _mapper.Map<List<TaskDto>>(tasks);
            return Ok(tasksDto);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var task = await _tasksRepository.GetByIdAsync(id);
            if (task == null)
            {
                return NotFound();
            }
            var taskDto = _mapper.Map<TaskDto>(task);
            return Ok(taskDto);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateTaskDto createTaskDto)
        {
            var task = _mapper.Map<Tasks>(createTaskDto);
            if (task == null)
            {
                return BadRequest("Task is null");
            }
            try
            {
                await _tasksRepository.AddAsync(task);
            }
            catch (DbUpdateException ex)
            {
                if (ex.InnerException is SqlException sqlEx && sqlEx.Message.Contains("CHK_Status"))
                {
                    return BadRequest("Invalid Task Status. Allowed statuses: To do, In Progress, Done.");
                }

                return StatusCode(500, "An error occurred while saving the task. Please try again later.");
            }
            var taskDto = _mapper.Map<TaskDto>(task);
            return CreatedAtAction(nameof(GetById), new { id = task.Id }, taskDto);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] UpdateTaskDto updateTaskDto)
        {
            var task = _mapper.Map<Tasks>(updateTaskDto);
            try
            {
                task = await _tasksRepository.UpdateAsync(id, task);
            }
            catch (DbUpdateException ex)
            {
                if (ex.InnerException is SqlException sqlEx && sqlEx.Message.Contains("CHK_Status"))
                {
                    return BadRequest("Invalid Task Status. Allowed statuses: To do, In Progress, Done.");
                }

                return StatusCode(500, "An error occurred while saving the task. Please try again later.");
            }
            if (task == null)
            {
                return NotFound();
            }

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var task = await _tasksRepository.DeleteAsync(id);
            if (task == null)
            {
                return NotFound();
            }

            return NoContent();
        }
    }
}
