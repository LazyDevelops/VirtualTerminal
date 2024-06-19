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

        current = FindFile(root, path);

        Tree<FileSystemEntry> newFile = new Tree<FileSystemEntry>(entry);
        current.AppendChildNode(newFile);
        return newFile;
    }

    public bool RemoveFile(Tree<FileSystemEntry> root, string path)
    {
        string[] directories = path.Split('/');
        Tree<FileSystemEntry> current = root;
        Tree<FileSystemEntry> parents = root;

        current = FindFile(root, path);
        if(current.Parents != null){
            parents = current.Parents;
        }

        if (current.LeftChild == null)
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
        // Tree<FileSystemEntry> home = new Tree<FileSystemEntry>(new FileSystemEntry("home", "root", 0b111101, FileSystemEntry.FileType.D));
        // Tree<FileSystemEntry> bin = new Tree<FileSystemEntry>(new FileSystemEntry("bin", "root", 0b111101, FileSystemEntry.FileType.D));
        // Tree<FileSystemEntry> user = new Tree<FileSystemEntry>(new FileSystemEntry("user", "user", 0b111000, FileSystemEntry.FileType.D));
        // Tree<FileSystemEntry> item = new Tree<FileSystemEntry>(new FileSystemEntry("item", "user", 0b111101, FileSystemEntry.FileType.D));
        // Tree<FileSystemEntry> file = new Tree<FileSystemEntry>(new FileSystemEntry("Hello_user.txt", "root", 0b111111, FileSystemEntry.FileType.F));

        // root.AppendChildNode(home);
        // root.AppendChildNode(bin);
        // home.AppendChildNode(user);
        // user.AppendChildNode(item);
        // user.AppendChildNode(file);

        Program program = new Program();

        program.CreateFile(root, "/", new FileSystemEntry("home", "root", 0b111101, FileSystemEntry.FileType.D));
        program.CreateFile(root, "/", new FileSystemEntry("bin", "root", 0b111101, FileSystemEntry.FileType.D));
        program.CreateFile(root, "/home", new FileSystemEntry("user", "user", 0b111101, FileSystemEntry.FileType.D));
        program.CreateFile(root, "/home/user", new FileSystemEntry("item", "user", 0b111101, FileSystemEntry.FileType.D));
        program.CreateFile(root, "/home/user", new FileSystemEntry("Hello_user.txt", "root", 0b111111, FileSystemEntry.FileType.F));

        program.CreateFile(root, "/home/user/item", new FileSystemEntry("newItem.item", "user", 0b111101, FileSystemEntry.FileType.F, "It`s is Item."));
        program.CreateFile(root, "/", new FileSystemEntry("check.txt", "user", 0b111101, FileSystemEntry.FileType.F, "check File"));
        
        program.RemoveFile(root, "/bin");
        Console.WriteLine(program.FindFile(root, "/home/user").Data.name);
        
        Console.WriteLine();

        root.PrintTree(0);

        Console.WriteLine();

        Console.WriteLine(program.FindFile(root, "/home/user").Parents.Data.name);

        List<string> StringList = new List<string>();

        // while(true){
        //     string? input = Console.ReadLine();

        //     if (string.IsNullOrWhiteSpace(input))
        //     {
        //         continue;
        //     }

        //     StringList.Append(input);

        //     program.CreateFile(root, "/", new FileSystemEntry(input, "user", 0b111101, FileSystemEntry.FileType.F, "check File"));

        //     Console.WriteLine("---PrintTree---");
        //     root.PrintTree(0);
        //     Console.WriteLine("---PrintTree---");
        // }


    }
}
