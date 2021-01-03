namespace TodoApi.Models
{
	public class UserReadDto
	{
		public string Username { get; set; }
		public string Email { get; set; }

		public static UserReadDto From(ApplicationUser user) =>
			new UserReadDto
			{
				Username =  user.UserName,
				Email = user.Email
			};
	}
}
