using System;
using System.Collections.Generic;
using System.Linq;
using TodoApi.Models;

namespace TodoApi.Data
{
	public class SqlTodoTaskRepository : ITodoTaskRepository
	{
		private readonly TodoContext _context;

		public SqlTodoTaskRepository(TodoContext context)
		{
			_context = context;
		}

		public bool SaveChanges()
		{
			return _context.SaveChanges() >= 0;
		}

		public IEnumerable<TodoTask> GetAllTasks() => _context.TodoTasks;

		public TodoTask GetTask(int id) => _context.TodoTasks.FirstOrDefault(t => t.Id == id);
		public void CreateTodoTask(TodoTask task)
		{
			if (task == null)
			{
				throw new ArgumentNullException(nameof(task));
			}

			_context.TodoTasks.Add(task);
		}

		public void UpdateTask(TodoTask task)
		{
			// Changes to an object get implicitly handled
		}
	}
}
