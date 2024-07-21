using Tree;

namespace VirtualTerminal.FileSystem
{
    public partial class FileSystem
    {
        public Tree<FileNode>? FindFile(string? path, Tree<FileNode> root)
        {
            if (path == null)
            {
                return null;
            }

            Tree<FileNode> currentNode = root;
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
                foreach (Tree<FileNode> child in currentNode.GetChildren().Where(child => child.Data.Name == file))
                {
                    currentNode = child;
                    break;
                }
            }

            return currentNode.Data.Name != fileName ? null : currentNode;
        }
    }
}