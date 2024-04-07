namespace Example.Core.Contracts;

public static class ProfileLocationStorage
{
	static ProfileLocationStorage()
	{
		ProfileRootDir = PathConstants.AppProgramDataPath;
	}

	/// <summary>Возвращает или задаёт корневой путь к корню профиля приложения.</summary>
	/// <value>Корневой путь к корню профиля приложения.</value>
	public static string ProfileRootDir { get; set; }

	/// <summary>Возвращает или задаёт путь, куда будут сохранятся конфигурация приложения.</summary>
	/// <value>Путь, куда будет сохранятся конфигурация приложения.</value>
	public static string ConfigDirPath => Path.Combine(ProfileRootDir, "Configuration");

	/// <summary>Возвращает путь, куда будут сохранятся база данных приложения.</summary>
	/// <value>Путь, куда будут сохранятся база данных приложения.</value>
	public static string DatabaseDirPath => Path.Combine(ProfileRootDir, "Database");

	/// <summary>Возвращает имя файла, в котором будет содержаться конфигурация приложения.</summary>
	/// <value>Имя файла, в котором будет содержаться конфигурация приложения.</value>
	public static string ConfigFileName => "configuration.json";

	/// <summary>Возвращает путь, где будут хранится конфигурационные файлы.</summary>
	/// <value>Путь, где будут хранится конфигурационные файлы.</value>
	public static string ConfigPath => Path.Combine(ConfigDirPath, ConfigFileName);
}
