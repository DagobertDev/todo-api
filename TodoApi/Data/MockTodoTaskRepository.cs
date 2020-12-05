using System.Collections.Generic;
using TodoApi.Models;

namespace TodoApi.Data
{
	public class MockTodoTaskRepository : ITodoTaskRepository
	{
		public IEnumerable<TodoTask> GetTasks()
		{
			return new []
			{
				new TodoTask {Id = 1, Description = "Read"},
				new TodoTask {Id = 2, Description = "Exercise"},
				new TodoTask {Id = 3, Description = "Cook"}
			};
		}

		public TodoTask GetTask(int id) => new TodoTask
		{
			Id = id,
			Description = "Test is working"
		};
	}
}
