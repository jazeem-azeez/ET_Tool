using System.IO;

namespace ET_Tool.IO
{
    public interface IDiskIOHandler
    {
        bool DirectoryCreateDirectory(string folderPath);
        string[] DirectoryGetFiles(string dirPath, string pattern, SearchOption searchOptions);
        void FileCopy(string srcFileName, string destFileName);
        void FileCopy(string srcFileName, string destFileName, bool isOverWriteEnabled);
        void FileDelete(string fileName);
        bool FileExists(string fileName);
        string FileReadAllText(string fileName);
        FileStream FileReadTextStream(string file);
        void FileWriteAllText(string fileName, string content);
        FileStream FileWriteTextStream(string destFileName);
        int GetSubDirectoriesLength(string path);
    }
}