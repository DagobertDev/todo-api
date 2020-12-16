using System.ComponentModel.DataAnnotations;

namespace TodoApi.Models
{
	public class TodoTask
	{
		[Key]
		public int Id { get; set; }

		[Required]
		public string Description { get; set; }
	}
}
