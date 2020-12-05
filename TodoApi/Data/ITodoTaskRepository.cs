using System.Collections.Generic;
using TodoApi.Models;

namespace TodoApi.Data
{
	public interface ITodoTaskRepository
	{
		public IEnumerable<TodoTask> GetTasks();
		public TodoTask GetTask(int id);
	}
}
