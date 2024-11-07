using VirtualTerminal.Tree.General;

namespace VirtualTerminal.FileSystem
{
    public partial class FileSystem
    {
        public Node<FileDataStruct>? FileCreate(string path, FileDataStruct entry, Node<FileDataStruct> root)
        {
            Node<FileDataStruct>? current = FindFile(path, root);

            if (current == null)
            {
                return null;
            }

            Node<FileDataStruct> newFile = new(entry);
            current.AppendChildNode(newFile);

            return newFile;
        }
    }
}