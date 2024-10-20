using VirtualTerminal.Tree.LCRS;

namespace VirtualTerminal.FileSystem
{
    public partial class FileSystem
    {
        public Node<FileDataStruct>? CreateFile(string path, FileDataStruct entry, Node<FileDataStruct> root)
        {
            Node<FileDataStruct>? current = FindFile(path, root);
            Node<FileDataStruct> newFile = new(entry);

            if (current == null)
            {
                return null;
            }

            current.AppendChildNode(newFile);

            return newFile;
        }
    }
}