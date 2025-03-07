namespace ToDo.Core.Models;

public partial class Tasks
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public string Description { get; set; } = null!;

    public string Status { get; set; } = null!;

    public DateTime CreatedAt { get; set; } = DateTime.Now;
}
