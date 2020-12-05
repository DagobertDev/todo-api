using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using TodoApi.Data;
using TodoApi.Models;

namespace TodoApi.Controllers
{
	[Route("api/tasks")]
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
			return Ok(_repository.GetTasks());
		}

		[HttpGet("{id}")]
		public ActionResult<TodoTask> GetTask(int id)
		{
			return Ok(_repository.GetTask(id));
		}
	}
}
