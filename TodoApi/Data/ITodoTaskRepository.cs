using System.Collections.Generic;
using TodoApi.Models;

namespace TodoApi.Data
{
	public interface ITodoTaskRepository
	{
		bool SaveChanges();
		IEnumerable<TodoTask> GetAllTasks();
		TodoTask GetTask(int id);
		void CreateTodoTask(TodoTask task);
		void UpdateTask(TodoTask task);
	}
}
