using System.Reflection;

namespace Example.Core.Contracts;
public static class PathConstants
{

	#region Data

	/// <summary>Сборка, которая начала исполнение приложения.</summary>
	private static readonly Assembly CallingAssembly = Assembly.GetEntryAssembly()!;

	/// <summary>
	/// Путь к общей для всех пользователей в системе папке, где хранятся данные приложения.
	/// </summary>
	private static readonly string ProgramDataPath = Environment.GetFolderPath(
		Environment.SpecialFolder.CommonApplicationData);

	#endregion

	#region Properties

	/// <summary>Возвращает путь, по которому будут сохраняться данные приложения.</summary>
	/// <value>Путь, по которому будут сохраняться данные приложения.</value>
	public static string AppProgramDataPath => Path.Combine(
		ProgramDataPath);

	/// <summary>Возвращает путь, по которому будут сохраняться протоколы работы приложения.</summary>
	/// <value>Путь, по которому будут сохраняться протоколы работы приложения.</value>
	public static string LogsRootPath => Path.Combine(
		AppProgramDataPath,
		"Logs");

	/// <summary>Возвращает путь к папке временных файлов проекта.</summary>
	public static string TempDataPath => Path.Combine(
		AppProgramDataPath,
		"Temp");

	#endregion

}
