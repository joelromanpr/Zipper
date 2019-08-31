using System.Collections.Generic;
using System.IO;

namespace ZipperLibrary
{
    public class ZipFile
    {
        public FileInfo File { get; set; } = new FileInfo($"{nameof(ZipFile)}.zip");
        public List<ZipItem> ZipItems { get; set; } = new List<ZipItem>();

    }
}
