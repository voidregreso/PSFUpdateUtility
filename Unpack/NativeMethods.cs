using System;
using System.Runtime.InteropServices;

namespace PSFUpdateUtility.Unpack
{
	// Token: 0x02000006 RID: 6
	internal static class NativeMethods
	{
		// Token: 0x06000007 RID: 7
		[DllImport("msdelta.dll", SetLastError = true)]
		[return: MarshalAs(UnmanagedType.Bool)]
		internal static extern bool ApplyDeltaW(long ApplyFlags, [MarshalAs(UnmanagedType.LPWStr)] string lpSourceName, [MarshalAs(UnmanagedType.LPWStr)] string lpDeltaName, [MarshalAs(UnmanagedType.LPWStr)] string lpTargetName);
	}
}
