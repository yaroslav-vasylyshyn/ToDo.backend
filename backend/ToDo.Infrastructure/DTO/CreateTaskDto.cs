using System.ComponentModel.DataAnnotations;

namespace ToDo.Infrastructure.DTO;

public partial class CreateTaskDto
{
    [MaxLength(255, ErrorMessage = "Task Name has to be a maximum of 255 characters long")]
    [MinLength(2, ErrorMessage = "Task Name has to be a minimum of 2 characters long")]
    public string Name { get; set; } = null!;

    [MaxLength(500, ErrorMessage = "Task Description has to be a maximum of 255 characters long")]
    [MinLength(2, ErrorMessage = "Task Description has to be a minimum of 2 characters long")]
    public string Description { get; set; } = null!;

    public string Status { get; set; } = null!;

    public DateTime CreatedAt { get; set; } = DateTime.Now;
}
