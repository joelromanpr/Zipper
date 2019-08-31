using System.IO;
using System.Linq;

namespace ZipperLibrary
{
    public class Zipper
    {
        private Zipper()
        {
        }

        public static ZipFile Zip(ZipConfiguration config)
        {
            config.Validate();

            var zipFile = new ZipFile();
            zipFile.File = config.Destination;

            using (var zip = new Ionic.Zip.ZipFile(zipFile.File.FullName))
            {
                PrepareZipFile(zipFile, config);

                var fileNames = from f in zipFile.ZipItems select f.Name;

                zip.AddFiles(fileNames.ToList(), directoryPathInArchive: "");
                zip.ParallelDeflateThreshold = -1;
                zip.Save();
            }

            return zipFile;
        }

        /// <summary>
        /// Prepares a Zipper.ZipFile object based on the ZipperConfiguration provided.
        /// </summary>
        /// <param name="zipFile"></param>
        /// <param name="config"></param>
        private static void PrepareZipFile(ZipFile zipFile, ZipConfiguration config)
        {
            var foundFiles = from file in config.Source.EnumerateFiles()
                             where file.Extension == config.Extension
                             orderby file.LastWriteTime ascending
                             select new ZipItem(file.FullName, file.Length, file.LastWriteTime);

            zipFile.ZipItems.AddRange(foundFiles.ToList());

            if (zipFile.ZipItems.Count == 0)
                return;

            if (zipFile.ZipItems.Count == config.SkipAmount)
            {
                zipFile.ZipItems.Clear();
            }
            else
            {
                int start = 0;
                if (config.Strategy == ZipConfiguration.SkipStrategy.START)
                    zipFile.ZipItems.RemoveRange(start, config.SkipAmount);
                else
                {
                    start = zipFile.ZipItems.Count - config.SkipAmount - 1;
                    zipFile.ZipItems.RemoveRange(start, config.SkipAmount);
                }
            }
        }

        /// <summary>
        /// Configuration class for Zipping items.
        /// </summary>
        public class ZipConfiguration
        {
            /// <summary>
            /// The extension of the files that should be zipped.
            /// </summary>
            public string Extension { get; set; }

            /// <summary>
            /// The source directory with the files to zip.
            /// </summary>
            public DirectoryInfo Source { get; set; }

            /// <summary>
            /// The destination of the zipped files.
            /// </summary>
            public FileInfo Destination { get; set; }

            /// <summary>
            /// Specifies the amount of files to skip during the zip process.
            /// </summary>
            public int SkipAmount { get; set; } = 0;

            /// <summary>
            /// Specifies where to start skipping if a skip amount was specified.
            /// <para>Default: ExcludeStrategy.END</para> 
            /// </summary>
            public SkipStrategy Strategy { get; set; } = SkipStrategy.END;

            /// <summary>
            /// Strategies when skipping items in the zipping queue.
            /// </summary>
            public enum SkipStrategy
            {
                START,
                END
            }

            public void Validate()
            {
                if (string.IsNullOrEmpty(Extension))
                    throw new System.ArgumentNullException(nameof(Extension), "Extension was not specified");

                if (Source == null)
                    throw new System.ArgumentNullException(nameof(Source), "Source was not specified");

                if (Destination == null)
                    throw new System.ArgumentNullException(nameof(Destination), "Destination was not specified");

                if (SkipAmount < 0)
                    throw new System.ArgumentOutOfRangeException(nameof(SkipAmount), "SkipAmount must be 0 or greater");
            }
        }
    }
}
