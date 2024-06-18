using System;
using Tree;

class Program
{
    public struct FileSystemEntry
    {
        public string name { get; }
        public byte permission { get; }
        public string UID { get; }
        public enum FileType
        {
            F, D
        }
        public FileType fileType { get; }
        public string? content { get; }

        public FileSystemEntry(string name, string UID, byte permission, FileType fileType, string? content = null)
        {
            this.name = name;
            this.UID = UID;
            this.permission = permission;
            this.fileType = fileType;
            this.content = content;
        }

        public override string ToString()
        {
            return name;
        }
    }

    public Tree<FileSystemEntry> CreateFile(Tree<FileSystemEntry> root, string path, FileSystemEntry entry)
    {
        string[] directories = path.Split('/');
        Tree<FileSystemEntry> current = root;

        for (int i = 1; i < directories.Length; i++)
        {
            foreach (Tree<FileSystemEntry> tempNode in current.GetChildren())
            {
                if (tempNode.Data.name == directories[i])
                {
                    current = tempNode;
                    break;
                }
            }
        }

        Tree<FileSystemEntry> newFile = new Tree<FileSystemEntry>(entry);
        current.AppendChildNode(newFile);
        return newFile;
    }

    public bool RemoveFile(Tree<FileSystemEntry> root, string path)
    {
        string[] directories = path.Split('/');
        Tree<FileSystemEntry> current = root;
        Tree<FileSystemEntry>? parents = null;

        for (int i = 1; i < directories.Length; i++)
        {
            foreach (Tree<FileSystemEntry> tempNode in current.GetChildren())
            {
                if (tempNode.Data.name == directories[i])
                {
                    current = tempNode;
                    if (directories.Length - i == 1)
                    {
                        parents = current;
                    }
                    break;
                }
            }
        }

        if (current.LeftChild == null && parents != null)
        {
            parents.RemoveChildNode(current);
            return true;
        }
        return false;
    }

    public Tree<FileSystemEntry> FindFile(Tree<FileSystemEntry> root, string path)
    {
        string[] directories = path.Split('/');
        Tree<FileSystemEntry> current = root;

        for (int i = 1; i < directories.Length; i++)
        {
            foreach (Tree<FileSystemEntry> tempNode in current.GetChildren())
            {
                if (tempNode.Data.name == directories[i])
                {
                    current = tempNode;
                    break;
                }
            }
        }

        return current;
    }

    static void Main()
    {
        Tree<FileSystemEntry> root = new Tree<FileSystemEntry>(new FileSystemEntry("/", "root", 0b111101, FileSystemEntry.FileType.D));
        Tree<FileSystemEntry> home = new Tree<FileSystemEntry>(new FileSystemEntry("home", "root", 0b111101, FileSystemEntry.FileType.D));
        Tree<FileSystemEntry> rootD = new Tree<FileSystemEntry>(new FileSystemEntry("root", "root", 0b111000, FileSystemEntry.FileType.D));
        Tree<FileSystemEntry> user = new Tree<FileSystemEntry>(new FileSystemEntry("user", "user", 0b111000, FileSystemEntry.FileType.D));
        Tree<FileSystemEntry> item = new Tree<FileSystemEntry>(new FileSystemEntry("item", "user", 0b111101, FileSystemEntry.FileType.D));
        Tree<FileSystemEntry> file = new Tree<FileSystemEntry>(new FileSystemEntry("Hello_user.txt", "root", 0b111111, FileSystemEntry.FileType.F));

        root.AppendChildNode(home);
        root.AppendChildNode(rootD);
        home.AppendChildNode(user);
        user.AppendChildNode(item);
        user.AppendChildNode(file);

        Program program = new Program();
        program.CreateFile(root, "/home/user/item", new FileSystemEntry("newItem.item", "user", 0b111101, FileSystemEntry.FileType.F));

        root.PrintTree(0);

        Console.WriteLine();
    }
}
