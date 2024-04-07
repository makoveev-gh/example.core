using Microsoft.EntityFrameworkCore;
using Microsoft.Net.Http.Headers;
using Microsoft.OpenApi.Models;

using Newtonsoft.Json;

using Example.Core;
using Example.Core.Data;
using Example.Core.Service;
using Example.Core.Contracts;
using Example.Core.DataStorage;
using Example.Core.Service.Api.v1;
using Example.Core.Service.Configuration;
using Example.Core.Service.Api.HealthApi;

public class Program
{
	private static IConfiguration Configuration { get; set; }

	private static Serilog.ILogger Log { get; set; }

	public static void Main(string[] args)
	{
		ProfileLocationStorage.ProfileRootDir = PathConstants.AppProgramDataPath;

		if(args.Length != 0)
		{
			ProfileLocationStorage.ProfileRootDir = args[0];
		}

		Directory.CreateDirectory(ProfileLocationStorage.ConfigDirPath);

		JsonConvert.DefaultSettings = () => new JsonSerializerSettings
		{
			Formatting = Formatting.Indented,
			MissingMemberHandling = MissingMemberHandling.Error
		};

		Configuration = new ConfigurationBuilder()
			.SetBasePath(ProfileLocationStorage.ConfigDirPath)
			.AddJsonFile(ProfileLocationStorage.ConfigFileName, optional: true, reloadOnChange: false)
			.AddCommandLine(args)
			.Build();
		Log = Serilog.Log.ForContext(typeof(Program));

		try
		{
			Log.Information($"Using profile dir: {ProfileLocationStorage.ProfileRootDir:profileDir}");
			Log.Information("Starting web host");
			var builder = WebApplication.CreateBuilder(args);

			AddServices(builder);

			var app = builder.Build();

			Configure(app);

			app.Run();
		}
		catch(Exception exc)
		{
			Log.Fatal(exc, "Host terminated unexpectedly");
		}
	}


	internal static void Configure(WebApplication app)
	{
		app.UseSwagger();
		app.UseSwaggerUI(c =>
		{
			c.SwaggerEndpoint("/swagger/v1/swagger.json", "Example");
		});

		app.UseAuthorization();

		app.UseCors(builder => builder
			.AllowAnyOrigin()
			.AllowAnyMethod()
			.AllowAnyHeader());

		app.UseRouting()
			.UseWebSockets()
			.UseExceptionHandler("/Error");

		app.Use(async (context, next) =>
		{
			context.Response.GetTypedHeaders().CacheControl = new CacheControlHeaderValue
			{
				NoCache = true,
				NoStore = true,
			};

			await next();
		});

		app.MapHealthEndpoints()
			.MapTableEndpoints()
			.MapClientsEndpoints()
			.MapDatesEndpoints();
	}

	internal static void AddServices(WebApplicationBuilder builder)
	{
		builder.Services.AddAuthorization();

		if(File.Exists(ProfileLocationStorage.ConfigPath))
		{
			builder.Configuration.Sources.Clear();
			builder.Configuration.AddConfiguration(Configuration);
		}
		else
		{
			Log.Information($"Configuration file: {ProfileLocationStorage.ConfigPath:configPath} couldn't be found");
		}


		builder.Services.AddSingleton<DatabaseConfiguration>();

		builder.Services
			.AddDbContext<ApplicationContext>(
				(s, o) => o.UseNpgsql(
					s.GetRequiredService<DatabaseConfiguration>().PostgreSql.ToConnectionString()))
			.AddSingleton<IApplicationDbProvider, ApplicationDbProvider>();

		builder.Services.AddOptions();
		builder.Services.AddMemoryCache();
		builder.Services.AddSingleton<IInitializable, DatabaseInitializer>();

		builder.Services.AddSingleton<ITableManager, TableManager>();
		builder.Services.AddSingleton<IClientsManager, ClientsManager>();
		builder.Services.AddSingleton<IDatesManager, DatesManager>();

		builder.Services.AddHostedService<InitializationService>();

		builder.Services
			.AddCors(opt => opt.AddDefaultPolicy(corsBuilder => corsBuilder
				.AllowAnyHeader()
				.AllowAnyMethod()
				.AllowAnyOrigin()));

		builder.Services.AddEndpointsApiExplorer();

		builder.Services.AddSwaggerGen(c =>
		{
			c.SwaggerDoc(
				"v1",
				new OpenApiInfo
				{
					Version = "v1",
					Title = "Example",
					Description = "Example"
				});
		});

	}
}