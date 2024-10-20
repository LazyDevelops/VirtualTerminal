using VirtualTerminal.LCRSTree;

namespace VirtualTerminal.FileSystem
{
    public partial class FileSystem
    {
        public Node<FileDataStruct>? FindFile(string? path, Node<FileDataStruct> root)
        {
            if (path == null)
            {
                return null;
            }

            Node<FileDataStruct> currentNode = root;
            List<string> files = [];
            string? fileName;

            path = path.TrimStart('/');
            files.AddRange(path.Split('/'));

            fileName = files.Last();

            if (files[0] == "")
            {
                return root;
            }

            foreach (string file in files)
            {
                foreach (Node<FileDataStruct> child in currentNode.GetChildren().Where(child => child.Data.Name == file))
                {
                    currentNode = child;
                    break;
                }
            }

            return currentNode.Data.Name != fileName ? null : currentNode;
        }
    }
}