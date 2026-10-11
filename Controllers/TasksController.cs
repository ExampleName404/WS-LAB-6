using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TaskManagementApi.Models;
using TaskManagementApi.Services;

namespace TaskManagementApi.Controllers;

[ApiController]
[Route("api/tasks")]
public class TasksController : ControllerBase
{
    private readonly TaskStore _store;

    public TasksController(TaskStore store)
    {
        _store = store;
    }

    [HttpGet]
    public ActionResult<List<TaskItem>> GetAll()
    {
        return Ok(_store.GetAll());
    }

    [HttpGet("{id:int}")]
    public ActionResult<TaskItem> GetById(int id)
    {
        var task = _store.Get(id);
        if (task == null)
            return NotFound(new { message = $"Задача с id={id} не найдена" });

        return Ok(task);
    }

    [HttpPost]
    [Authorize]
    public IActionResult Create(TaskItem task)
    {
        var userName = GetUserName();
        var created = _store.Add(task);

        return CreatedAtAction(nameof(GetById), new { id = created.Id }, new
        {
            message = "Задача создана",
            createdBy = userName,
            task = created
        });
    }

    [HttpDelete("{id:int}")]
    [Authorize]
    public IActionResult Delete(int id)
    {
        var userName = GetUserName();
        if (!_store.Delete(id))
            return NotFound(new { message = $"Задача с id={id} не найдена" });

        return Ok(new
        {
            message = $"Задача {id} удалена",
            deletedBy = userName
        });
    }

    private string? GetUserName()
    {
        return HttpContext.User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Name)?.Value;
    }
}
