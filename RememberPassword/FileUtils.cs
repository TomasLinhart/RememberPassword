using System;
using System.IO;

namespace RememberPassword
{
	public static class FileUtils
	{
		public static void WriteFileContent(string filename, string content)
		{
			using (TextWriter textWriter = new StreamWriter(filename)) {
				textWriter.Write(content);
			}
		}

		public static string ReadFileContent(string filename)
		{
			if (File.Exists(filename)) {
				using (FileStream fileStream = new FileStream(filename, FileMode.Open, FileAccess.Read)) {
					using (TextReader textReader = new StreamReader(fileStream)) {
						return textReader.ReadToEnd();
					}
				}
			}

			return null;
		}
	}
}

