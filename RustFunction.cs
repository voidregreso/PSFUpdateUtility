using System;
using System.Runtime.InteropServices;
using System.Text;

namespace PSFUpdateUtility
{
    class RustFunction
    {
        [DllImport("libcabinet.dll")]
        public static extern bool PackCab(byte[] folderp, byte[] dcabp, short comprate);

        public static bool PackCab(string folderp, string dcabp, short comprate)
        {
            byte[] p1 = new UTF8Encoding().GetBytes(folderp);
            byte[] p2 = new UTF8Encoding().GetBytes(dcabp);
            return PackCab(p1, p2, comprate);
        }
    }
}
