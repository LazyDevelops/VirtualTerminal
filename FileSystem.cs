using Tree;

namespace FileSystem
{
    public class FileSystem
    {
        public struct FileNode(string name, string UID, byte permission, FileType fileType, string? content = null)
        {
            public string name { get; } = name;
            public byte permission { get; } = permission;
            public string UID { get; } = UID;
            public FileType fileType { get; } = fileType;
            public string? content { get; } = content;

            public override string ToString()
            {
                return name;
            }
        }

        public enum FileType
        {
            F = '-',
            D = 'd'
        }

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

        public int RemoveFile(string path, Tree<FileNode> root)
        {
            string[] directories = path.Split('/');
            Tree<FileNode>? current = FindFile(path, root);
            Tree<FileNode> parents;

            if (current == null)
            {
                return 1;
            }

            if (current.Parents == null)
            {
                return 2;
            }

            parents = current.Parents;

            if (current.LeftChild == null)
            {
                parents.RemoveChildNode(current);
                return 0;
            }

            return 3;
        }

        public Tree<FileNode>? FindFile(string path, Tree<FileNode> root)
        {
            Tree<FileNode> current = root;
            string? fileName;
            var files = new List<string>();

            path = path.Substring(1);
            files.AddRange(path.Split('/'));

            fileName = files.Last();

            if (files[0] == "")
            {
                return root;
            }
            else
            {
                foreach (string temp in files)
                {
                    foreach (Tree<FileNode> tempNode in current.GetChildren())
                    {
                        if (tempNode.Data.name == temp)
                        {
                            current = tempNode;
                            break;
                        }
                    }
                }

                if (current.Data.name != fileName)
                {
                    return null;
                }

                return current;
            }
        }

        public string ConvertPermissionsToString(short permissions)
        {
            string result = string.Empty;
            result += (permissions & 040) != 0 ? "r" : "-";
            result += (permissions & 020) != 0 ? "w" : "-";
            result += (permissions & 010) != 0 ? "x" : "-";
            result += (permissions & 004) != 0 ? "r" : "-";
            result += (permissions & 002) != 0 ? "w" : "-";
            result += (permissions & 001) != 0 ? "x" : "-";
            return result;
        }
    }
}
