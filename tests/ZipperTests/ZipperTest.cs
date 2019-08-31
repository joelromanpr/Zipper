using System;
using System.IO;
using Xunit;
using ZipperLibrary;

namespace ZipperTests
{
    public class ZipperTest
    {
        private DirectoryInfo CreateRandomPath()
        {
            var random = Path.GetRandomFileName();
            var dir = Directory.CreateDirectory(random);
            return dir;
        }

        private string[] CreateTestFiles(string path)
        {
            var dummyFiles = new string[]
            {
                "dummy.txt", "dummy2.txt", "dummy3.txt", "dummy.pdf"
            };
            for (int i = 0; i < dummyFiles.Length; i++)
            {
                File.WriteAllText(Path.Combine(path, dummyFiles[i]), "dummy");
            }

            return dummyFiles;
        }

        private void DeleteTestDir(DirectoryInfo testPath)
        {
            try
            {
                testPath.Delete(recursive: true);
            }
            catch (Exception ex)
            {
                //We don't really care if we can delete the test dir.
            }
        }

        [Fact]
        public void Zip_files_are_at_destination()
        {
            var randomDir = CreateRandomPath();
            var fileNames = CreateTestFiles(randomDir.FullName);
            var destination = CreateRandomPath();

            var config = new Zipper.ZipConfiguration()
            {
                Source = randomDir,
                Destination = new FileInfo(Path.Combine(destination.FullName, "zipfile.zip")),
                Extension = ".pdf",
            };

            var zipFile = Zipper.Zip(config);
            Assert.True(zipFile.File.Exists);
            Assert.Single(zipFile.ZipItems);

            DeleteTestDir(config.Source);
            DeleteTestDir(config.Destination.Directory);
        }

        [Fact]
        public void Zip_skip_two()
        {
            var randomDir = CreateRandomPath();
            var fileNames = CreateTestFiles(randomDir.FullName);
            var destination = CreateRandomPath();

            var config = new Zipper.ZipConfiguration()
            {
                Source = randomDir,
                Destination = new FileInfo(Path.Combine(destination.FullName, "zipfile.zip")),
                Extension = ".txt",
                SkipAmount = 2
            };

            var zipFile = Zipper.Zip(config);
            Assert.True(zipFile.File.Exists);
            Assert.Single(zipFile.ZipItems);

            DeleteTestDir(config.Source);
            DeleteTestDir(config.Destination.Directory);
        }

        [Fact]
        public void Zip_skip_all_zipfile_should_be_empty()
        {
            var randomDir = CreateRandomPath();
            var fileNames = CreateTestFiles(randomDir.FullName);
            var destination = CreateRandomPath();

            var config = new Zipper.ZipConfiguration()
            {
                Source = randomDir,
                Destination = new FileInfo(Path.Combine(destination.FullName, "zipfile.zip")),
                Extension = ".txt",
                SkipAmount = 3
            };

            var zipFile = Zipper.Zip(config);
            Assert.True(zipFile.File.Exists);
            Assert.Empty(zipFile.ZipItems);

            DeleteTestDir(config.Source);
            DeleteTestDir(config.Destination.Directory);
        }

        [Fact]
        public void Zip_invalid_configuration()
        {
            var config = new Zipper.ZipConfiguration();

            Exception ex = Assert.Throws<ArgumentNullException>(() => Zipper.Zip(config));
            Assert.StartsWith("Extension was not specified", ex.Message);

            config.Extension = ".txt";
            ex = Assert.Throws<ArgumentNullException>(() => Zipper.Zip(config));
            Assert.StartsWith("Source was not specified", ex.Message);

            config.Source = new DirectoryInfo("/");
            ex = Assert.Throws<ArgumentNullException>(() => Zipper.Zip(config));
            Assert.StartsWith("Destination was not specified", ex.Message);

            config.Destination = new FileInfo("/");
            config.SkipAmount = -9000;
            ex = Assert.Throws<ArgumentOutOfRangeException>(() => Zipper.Zip(config));
            Assert.StartsWith("SkipAmount must be 0 or greater", ex.Message);
        }
    }
}