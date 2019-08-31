using System;

namespace ZipperLibrary
{
    public class ZipItem
    {
        public long Size { get; private set; }
        public string Name { get; private set; }
        public DateTime LastModified { get; private set; }

        public ZipItem(string name, long size, DateTime lastModified)
        {
            Name = name;
            Size = size;
            LastModified = lastModified;
        }
    }
}
