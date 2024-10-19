using VirtualTerminal.LCRSTree;

namespace VirtualTerminal.FileSystem
{
    public partial class FileSystem
    {
        public Tree<FileNode>? CreateFile(string path, FileNode entry, Tree<FileNode> root)
        {
            Tree<FileNode>? current = FindFile(path, root);
            Tree<FileNode> newFile = new(entry);

            if (current == null)
            {
                return null;
            }

            current.AppendChildNode(newFile);

            return newFile;
        }
    }
}