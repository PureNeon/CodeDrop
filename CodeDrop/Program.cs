using System.Timers;
using System.Threading.Tasks;

namespace CodeDrop
{
	public class Program
	{
		private readonly IWebHostEnvironment _environment;

		public static void Main(string[] args)
		{
			System.Timers.Timer _timer = new System.Timers.Timer(900000);
			_timer.Elapsed += CheckLifeTimeEvent;
			_timer.AutoReset = true;
			_timer.Enabled = true;

			var builder = WebApplication.CreateBuilder(args);
			builder.Services.AddControllers();

			var app = builder.Build();
			app.MapControllers();
			app.UseStaticFiles();
			app.Run();
		}

		private static void CheckLifeTimeEvent(Object source, ElapsedEventArgs e)
		{
			string[] files = Directory.GetFiles(Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "SavedCode"));
			Parallel.ForEach(files, file => {
				DateTime lastWriteTime = File.GetLastWriteTime(file);
				lastWriteTime = lastWriteTime.AddDays(1);
				if (lastWriteTime <= DateTime.Now)
				{
					File.Delete(file);
				}
			});
		}
	}
}
