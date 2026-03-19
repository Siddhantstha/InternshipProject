using Application.Interface;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace User.api.Controllers.Http
{
	[Route("api/[controller]")]
	[ApiController]
	public class HttpUserController : ControllerBase
	{
		private readonly IUserDetails _userDetails;

		public HttpUserController(IUserDetails userDetails)
		{
			_userDetails = userDetails;
		}
		[HttpGet("GetUser")]
		public async Task<IActionResult> GetUserAsync(int userId)
		{
			var result = await _userDetails.GetUserAsync(userId);
			return Ok(result);
		}
	}
}
