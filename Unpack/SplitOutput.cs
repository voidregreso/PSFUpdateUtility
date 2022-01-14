using System;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Windows;

namespace PSFUpdateUtility.Unpack
{
	// Token: 0x02000007 RID: 7
	internal class SplitOutput
	{
		// Token: 0x06000008 RID: 8 RVA: 0x0000233C File Offset: 0x0000053C
		public static void WriteOutput(string PSFFile, string DirectoryName)
		{
			Console.WriteLine("Writing: " + DeltaFileList.List.Count + " Files");
			FileStream fileStream = File.OpenRead(Path.GetFullPath(PSFFile));
			string text = DirectoryName;
			
			Environment.CurrentDirectory = text;
			if (!Directory.Exists("000"))
			{
				Directory.CreateDirectory("000");
			}
			foreach (object obj in DeltaFileList.List)
			{
				DeltaFile deltaFile = (DeltaFile)obj;
				string fileName = deltaFile.FileName;
				if (!File.Exists(fileName))
				{
					string directoryName = Path.GetDirectoryName(fileName);
					string fileName2 = Path.GetFileName(fileName);
					string text2 = text + Path.DirectorySeparatorChar.ToString() + fileName;
					string text3 = text + Path.DirectorySeparatorChar.ToString() + directoryName;
					int num = 0;
					if (text2.Length > 259 || text3.Length > 259)
					{
						num = 1;
					}
					if (num == 0 && directoryName.IndexOf("_", StringComparison.Ordinal) != -1)
					{
						try
						{
							Directory.CreateDirectory(directoryName);
						}
						catch (Exception)
						{
							Console.WriteLine("Writing: " + fileName);
							Console.WriteLine("Error: failed to create directory.\n");
						}
					}
					string text4;
					if (num == 0)
					{
						text4 = fileName;
					}
					else
					{
						text4 = "000\\" + fileName2;
					}
					try
					{
						fileStream.Seek(deltaFile.sourceOffset, SeekOrigin.Begin);
					}
					catch
					{
						Console.WriteLine("Writing: " + fileName);
						Console.WriteLine("Error: PSFFileStream.Seek failed.\n");
					}
					byte[] buffer = new byte[deltaFile.sourceLength];
					try
					{
						fileStream.Read(buffer, 0, deltaFile.sourceLength);
					}
					catch
					{
						Console.WriteLine("Writing: " + fileName);
						Console.WriteLine("Error: PSFFileStream.Read failed.\n");
					}
					FileStream fileStream2 = File.Create(text4);
					try
					{
						fileStream2.Write(buffer, 0, deltaFile.sourceLength);
					}
					catch
					{
						Console.WriteLine("Writing: " + fileName);
						Console.WriteLine("Error: OutputFileStream.Write failed.\n");
					}
					fileStream2.Close();
					if (deltaFile.sourceType.Equals("PA30", StringComparison.OrdinalIgnoreCase))
					{
						Console.WriteLine("Expanding PA30: " + fileName);
						if (!NativeMethods.ApplyDeltaW(0L, null, text4, text4))
						{
							Console.WriteLine("Error: ApplyDeltaW failed.\n");
						}
					}
					File.SetLastWriteTimeUtc(text4, DateTime.FromFileTimeUtc(deltaFile.time));
					if (num == 1)
					{
						using (Process process = new Process())
						{
							process.StartInfo.UseShellExecute = false;
							process.StartInfo.CreateNoWindow = true;
							process.StartInfo.WorkingDirectory = Directory.GetParent(text).FullName;
							process.StartInfo.FileName = "robocopy.exe";
							process.StartInfo.Arguments = string.Concat(new string[]
							{
								"\"",
								DirectoryName,
								"\\000\" \"",
								DirectoryName,
								"\\",
								directoryName,
								"\" ",
								fileName2,
								" /MOV /R:1 /W:1 /NFL /NDL /NP /NJH /NJS"
							});
							process.Start();
							process.WaitForExit();
						}
					}
				}
			}
			if (Directory.Exists("000"))
			{
				Directory.Delete("000", true);
			}
		}
	}
}
