using System;

namespace PSFUpdateUtility.Unpack
{
	// Token: 0x02000003 RID: 3
	internal class DeltaFile
	{
		// Token: 0x06000003 RID: 3 RVA: 0x00002194 File Offset: 0x00000394
		public DeltaFile(string FileName, long time, string sourceType, long sourceOffset, int sourceLength)
		{
			this.FileName = FileName;
			this.time = time;
			this.sourceType = sourceType;
			this.sourceOffset = sourceOffset;
			this.sourceLength = sourceLength;
		}

		// Token: 0x04000001 RID: 1
		public string FileName;

		// Token: 0x04000002 RID: 2
		public long time;

		// Token: 0x04000003 RID: 3
		public string sourceType;

		// Token: 0x04000004 RID: 4
		public long sourceOffset;

		// Token: 0x04000005 RID: 5
		public int sourceLength;
	}
}
