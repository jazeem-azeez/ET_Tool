using System.IO;

namespace ET_Tool.IO
{
    public class DiskIOHandler : IDiskIOHandler
    {
        public bool DirectoryCreateDirectory(string folderPath) => Directory.CreateDirectory(folderPath).Exists;

        public string[] DirectoryGetFiles(string dirPath, string pattern, SearchOption searchOptions) => Directory.GetFiles(dirPath, pattern, searchOptions);

        public void FileCopy(string srcFileName, string destFileName) => File.Copy(srcFileName, destFileName);

        public void FileCopy(string srcFileName, string destFileName, bool isOverWriteEnabled) => File.Copy(srcFileName, destFileName, isOverWriteEnabled);

        public void FileDelete(string fileName) => File.Delete(fileName);

        public bool FileExists(string fileName) => File.Exists(fileName);

        public string FileReadAllText(string fileName) => File.ReadAllText(fileName);

        public FileStream FileReadTextStream(string file) => File.Open(file, FileMode.Open, FileAccess.ReadWrite);

        public void FileWriteAllText(string fileName, string content) => File.WriteAllText(fileName, content);

        public FileStream FileWriteTextStream(string destFileName) => File.OpenWrite(destFileName);

        public int GetSubDirectoriesLength(string path) => new DirectoryInfo(path).GetDirectories().Length;
    }
}