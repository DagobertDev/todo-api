using System.Collections.Generic;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TodoApi.Data;
using TodoApi.Models;

namespace TodoApi.Controllers
{
	[Authorize]
	[Route("tasks")]
	[ApiController]
	public class TodoTasksController : ControllerBase
	{
		private readonly ITodoTaskRepository _repository;

		public TodoTasksController(ITodoTaskRepository repository)
		{
			_repository = repository;
		}

		[HttpGet]
		public ActionResult<IEnumerable<TodoTask>> GetAllTasks()
		{
			return Ok(_repository.GetAllTasks());
		}

		[HttpGet("{id}")]
		public ActionResult<TodoTask> GetTask(int id)
		{
			var task = _repository.GetTask(id);

			if (task == null)
			{
				return NotFound();
			}

			return Ok(task);
		}

		[HttpPost]
		public ActionResult<TodoTask> CreateTask(TodoTaskWriteDto taskWriteDto)
		{
			var task = taskWriteDto.ToTodoTask();

			if (string.IsNullOrWhiteSpace(task.Description))
			{
				return BadRequest();
			}

			_repository.CreateTodoTask(task);
			_repository.SaveChanges();

			return CreatedAtAction(nameof(GetTask), new { id = task.Id }, task);
		}

		[HttpPut("{id}")]
		public ActionResult<TodoTask> UpdateTask(int id, TodoTask task)
		{
			var existingTask = _repository.GetTask(id);

			if (existingTask == null)
			{
				return NotFound();
			}

			existingTask.Description = task.Description;

			_repository.UpdateTask(task);
			_repository.SaveChanges();

			return NoContent();
		}

		[HttpDelete("{id}")]
		public ActionResult DeleteTask(int id)
		{
			var existingTask = _repository.GetTask(id);

			if (existingTask == null)
			{
				return NotFound();
			}

			_repository.DeleteTask(existingTask);
			_repository.SaveChanges();

			return NoContent();
		}
	}
}
