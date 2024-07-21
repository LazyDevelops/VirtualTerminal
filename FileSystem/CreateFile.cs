using Tree;

namespace VirtualTerminal.FileSystem
{
    public partial class FileSystem
    {
        public Tree<FileNode>? CreateFile(string path, FileNode entry, Tree<FileNode> root)
        {
            Tree<FileNode>? current = FindFile(path, root);

            if (current == null)
            {
                return null;
            }

            Tree<FileNode> newFile = new(entry);
            current.AppendChildNode(newFile);

            return newFile;
        }
    }
}