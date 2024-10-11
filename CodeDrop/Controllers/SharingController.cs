using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using static CodeDrop.Messages.DefaultMessages;

namespace CodeDrop.Controllers
{
	[Route("")]
	[ApiController]
	public class SharingController : ControllerBase
	{
		private readonly IWebHostEnvironment _environment;

		public SharingController(IWebHostEnvironment environment)
		{
			_environment = environment;
		}

		[HttpGet]
		public IActionResult Index()
		{
			Response.Headers.Clear();
			Response.Headers.Append("Cache-Control", "no-cache");
			Response.Headers.Append("Pragma", "no-cache");
			Response.Headers.Append("Expires", "0");
			return PhysicalFile(Path.Combine(_environment.WebRootPath, "index.html"), "text/html");
		}

		[HttpGet("{shareCode}")]
		public IActionResult GetByCode(string shareCode)
		{
			Response.Headers.Clear();
			Response.Headers.Append("Cache-Control", "no-cache");
			Response.Headers.Append("Pragma", "no-cache");
			Response.Headers.Append("Expires", "0");
			return PhysicalFile(Path.Combine(_environment.WebRootPath, "index.html"), "text/html");
		}

		[HttpPost]
		public StringMessage? CreateNew([FromBody] StringMessage sm)
		{
			MD5 md5 = MD5.Create();
			byte[] bytes = Encoding.UTF8.GetBytes(sm.Message + DateTime.UtcNow);
			byte[] hashBytes = md5.ComputeHash(bytes);
			string hashKey = Convert.ToHexString(hashBytes);
			string filePath = Path.Combine(_environment.WebRootPath, $"SavedCode\\{hashKey}.txt");
			System.IO.File.WriteAllText(filePath, sm.Message);
			return new StringMessage(hashKey);
		}

		[HttpPost("{shareCode}")]
		public StringMessage? Update(string shareCode, [FromBody] StringMessage sm)
		{
			string filePath = Path.Combine(_environment.WebRootPath, $"SavedCode\\{shareCode}.txt");
			if (!System.IO.File.Exists(filePath))
			{
				return null;
			}
			System.IO.File.WriteAllText(filePath, sm.Message);
			return new StringMessage(shareCode);
		}
	}
}
