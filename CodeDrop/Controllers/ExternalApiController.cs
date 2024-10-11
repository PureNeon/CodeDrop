using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using static CodeDrop.Messages.DefaultMessages;

namespace CodeDrop.Controllers
{
	[Route("api")]
	[ApiController]
	public class ExternalApiController : ControllerBase
	{
		private readonly IWebHostEnvironment _environment;

		public ExternalApiController(IWebHostEnvironment environment)
		{
			_environment = environment;
		}

		[HttpGet]
		public StringMessage Index()
		{
			return new StringMessage("CodeDrop API is available");
		}

		[HttpGet("{shareCode}")]
		public StringMessage? GetByCode(string shareCode)
		{
			string filePath = Path.Combine(_environment.WebRootPath, $"SavedCode\\{shareCode}.txt");
			if (!System.IO.File.Exists(filePath))
			{
				Response.StatusCode = (int)HttpStatusCode.NotFound;
				return null;
			}
			string content = System.IO.File.ReadAllText(filePath);
			return new StringMessage(content);
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
				Response.StatusCode = (int)HttpStatusCode.NotFound;
				return null;
			}
			System.IO.File.WriteAllText(filePath, sm.Message);
			return new StringMessage(shareCode);
		}
	}
}
