namespace ToDo.Infrastructure.DTO;

public partial class CreateTaskDto
{
    public string Name { get; set; } = null!;

    public string Description { get; set; } = null!;

    public string Status { get; set; } = null!;

    public DateTime CreatedAt { get; set; } = DateTime.Now;
}
