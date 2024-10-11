using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using static CodeDrop.Messages.DefaultMessages;

namespace CodeDrop.Controllers
{
	[Route("dev")]
	[ApiController]
	public class DefaultController : ControllerBase
	{
		[HttpGet]
		public StringMessage Index()
		{
			return new StringMessage(Message: "Server is running!");
		}

		[HttpGet("ping")]
		public StringMessage Ping()
		{
			return new StringMessage(Message: "pong");
		}
	}
}
