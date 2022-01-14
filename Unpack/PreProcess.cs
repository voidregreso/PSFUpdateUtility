using System;
using System.Diagnostics;
using System.IO;

namespace PSFUpdateUtility.Unpack
{
	// Token: 0x02000008 RID: 8
	internal class PreProcess
	{
		// Token: 0x0600000A RID: 10 RVA: 0x0000272C File Offset: 0x0000092C
		public static void Process(string CABFileName, string DirectoryName)
		{
			Process process = new Process();
			process.StartInfo.UseShellExecute = false;
			process.StartInfo.CreateNoWindow = true;
			process.StartInfo.FileName = "expand.exe";
			process.StartInfo.Arguments = string.Concat(new string[]
			{
				"-f:* \"",
				CABFileName,
				"\" \"",
				DirectoryName,
				"\""
			});
			Console.WriteLine("Expanding CAB...");
			process.Start();
			process.WaitForExit();
			bool flag = process.HasExited && process.ExitCode == 0;
			process.Dispose();
			if (!flag)
			{
				throw new IOException();
			}
		}
	}
}
