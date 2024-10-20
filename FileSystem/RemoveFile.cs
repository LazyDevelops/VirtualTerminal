using VirtualTerminal.Tree.LCRS;

namespace VirtualTerminal.FileSystem
{
    public partial class FileSystem
    {
        public int RemoveFile(string path, Node<FileDataStruct> root, char? option)
        {
            Node<FileDataStruct>? currentNode = FindFile(path, root);
            Node<FileDataStruct> parents;

            if (currentNode == null)
            {
                return 1;
            }

            if (currentNode.Parent == null)
            {
                return 2;
            }

            parents = currentNode.Parent;

            if (option != 'r' && currentNode.LeftChild != null)
            {
                return 3;
            }

            if (currentNode.LeftChild != null)
            {
                RemoveAllChildren(currentNode);
            }

            parents.RemoveChildNode(currentNode);
            return 0;
        }
    }
}