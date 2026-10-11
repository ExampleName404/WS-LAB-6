using TaskManagementApi.Models;

namespace TaskManagementApi.Services;

public class TaskStore
{
    private readonly object _lock = new();
    private int _nextId = 4;

    private readonly List<TaskItem> _tasks = new()
    {
        new TaskItem { Id = 1, Title = "Настроить JWT", Description = "Подключить JwtBearer в Program.cs", IsCompleted = true },
        new TaskItem { Id = 2, Title = "Сделать AuthController", Description = "POST /api/auth/login", IsCompleted = false },
        new TaskItem { Id = 3, Title = "Защитить методы", Description = "Добавить [Authorize] на POST и DELETE", IsCompleted = false }
    };

    public List<TaskItem> GetAll()
    {
        lock (_lock)
        {
            return _tasks.ToList();
        }
    }

    public TaskItem? Get(int id)
    {
        lock (_lock)
        {
            return _tasks.FirstOrDefault(t => t.Id == id);
        }
    }

    public TaskItem Add(TaskItem task)
    {
        lock (_lock)
        {
            task.Id = _nextId++;
            _tasks.Add(task);
            return task;
        }
    }

    public bool Delete(int id)
    {
        lock (_lock)
        {
            return _tasks.RemoveAll(t => t.Id == id) > 0;
        }
    }
}
