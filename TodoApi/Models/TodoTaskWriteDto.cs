using System.ComponentModel.DataAnnotations;

namespace TodoApi.Models
{
	public class TodoTaskWriteDto
	{
		[Required]
		public string Description { get; set; }

		public TodoTask ToTodoTask() => new TodoTask { Description = Description };
	}
}
