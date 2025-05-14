namespace WebApp.Services
{
	public class AuthService
	{
		private readonly IHttpContextAccessor _httpContextAccessor;

		public AuthService(IHttpContextAccessor httpContextAccessor)
		{
			_httpContextAccessor = httpContextAccessor;
		}

		public void SetUserIdRole(int Id, string Role)
		{
			_httpContextAccessor.HttpContext?.Session.SetInt32("UserId", Id);
			_httpContextAccessor.HttpContext?.Session.SetString("UserRole", Role);
		}

		public (int, string?) GetUserIdRole()
		{
			int? userId = _httpContextAccessor.HttpContext?.Session.GetInt32("UserId");
			string? userRole = _httpContextAccessor.HttpContext?.Session.GetString("UserRole");
			return (userId ?? 0, userRole);
		}

		public void ClearSession()
		{
			_httpContextAccessor.HttpContext?.Session.Clear();
		}
	}
}