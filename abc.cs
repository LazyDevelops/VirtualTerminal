using System;
using System.Collections.Generic;

class VirtualTerminal
{
    private struct FileSystemEntry
    {
        public string Path { get; }
        public string? Content { get; }
        public bool IsDirectory { get; }

        public FileSystemEntry(string path, bool isDirectory, string? content = null)
        {
            Path = path;
            IsDirectory = isDirectory;
            Content = content;
        }
    }
    private List<FileSystemEntry> fileSystem;
    private string currentDirectory;

    public VirtualTerminal()
    {
        fileSystem = new List<FileSystemEntry>
        {
            new FileSystemEntry("/", true),
            new FileSystemEntry("/home", true),
            new FileSystemEntry("/home/user", true),
            new FileSystemEntry("/home/user/file.txt", false, "this is a sample file")
        };

        currentDirectory = "/";
    }

    public void Run()
    {
        while (true)
        {
            DisplayPrompt();
            string? command = Console.ReadLine();
            if (string.IsNullOrWhiteSpace(command))
            {
                continue;
            }
            ProcessCommand(command);
        }
    }

    private void DisplayPrompt()
    {
        WriteColoredText("\x1b[1muser\x1b[22m", ConsoleColor.Green);
        WriteColoredText(":", Console.ForegroundColor);
        WriteColoredText($"\x1b[1m{currentDirectory}\x1b[22m", ConsoleColor.Blue);
        WriteColoredText("$ ", Console.ForegroundColor);
    }

    private void ProcessCommand(string command)
    {
        string[] args = command.Split(' ');

        switch (args[0])
        {
            case "ls":
                ExecuteLs();
                break;
            case "cd":
                if (args.Length > 1)
                {
                    ExecuteCd(args[1]);
                }
                else
                {
                    Console.WriteLine("Usage: cd <directory>");
                }
                break;
            case "cat":
                if (args.Length > 1)
                {
                    ExecuteCat(args[1]);
                }
                else
                {
                    Console.WriteLine("Usage: cat <file>");
                }
                break;
            case "clear":
                ExecuteClear();
                break;
            case "mkdir":
                if (args.Length > 1)
                {
                    ExecuteMkdir(args[1]);
                }
                else
                {
                    Console.WriteLine("Usage: mkdir <directory>");
                }
                break;
            case "rmdir":
                if (args.Length > 1)
                {
                    ExecuteRmdir(args[1]);
                }
                else
                {
                    Console.WriteLine("Usage: rmdir <directory>");
                }
                break;
            case "rm":
                if (args.Length > 1)
                {
                    ExecuteRm(args[1]);
                }
                else
                {
                    Console.WriteLine("Usage: rm <file>");
                }
                break;
            case "exit":
                ExecuteExit();
                break;
            case "help":
                ExecuteHelp();
                break;
            default:
                Console.WriteLine($"Command not found: {command}");
                break;
        }
    }

    private void ExecuteLs()
    {
        HashSet<string> seen = new HashSet<string>();

        foreach (var entry in fileSystem)
        {
            if (entry.Path.StartsWith(currentDirectory))
            {
                string relativePath = entry.Path.Substring(currentDirectory.Length).TrimStart('/');
                if (relativePath.Contains('/'))
                {
                    relativePath = relativePath.Substring(0, relativePath.IndexOf('/'));
                }

                if (!seen.Contains(relativePath) && relativePath.Length > 0)
                {
                    Console.WriteLine(relativePath);
                    seen.Add(relativePath);
                }
            }
        }
    }

    private void ExecuteCd(string dir)
    {
        if (dir == "." || dir == "./")
        {
            return;
        }
        else if (dir == ".." || dir == "../")
        {
            if (currentDirectory != "/")
            {
                int lastSlashIndex = currentDirectory.LastIndexOf('/');
                if (lastSlashIndex == 0)
                {
                    currentDirectory = "/";
                }
                else
                {
                    currentDirectory = currentDirectory.Substring(0, lastSlashIndex);
                }
            }
        }
        else
        {
            string newDir = currentDirectory == "/" ? $"/{dir}" : $"{currentDirectory}/{dir}";
            if (fileSystem.Exists(entry => entry.Path == newDir && entry.IsDirectory))
            {
                currentDirectory = newDir;
            }
            else
            {
                Console.WriteLine($"Directory not found: {dir}");
            }
        }
    }

    private void ExecuteCat(string file)
    {
        string path = currentDirectory == "/" ? $"/{file}" : $"{currentDirectory}/{file}";
        var entry = fileSystem.Find(entry => entry.Path == path); //  && !entry.IsDirectory

        if(entry.IsDirectory){
            Console.WriteLine($"Not a file: {file}");
        }
        else if (entry.Path != null)
        {
            Console.WriteLine(entry.Content);
        }
        else
        {
            Console.WriteLine($"File not found: {file}. Creating new file. Enter content (end with a single dot on a line):");
            string content = ReadMultiLineInput();
            fileSystem.Add(new FileSystemEntry(path, false, content));
            Console.WriteLine("File created.");
        }
    }

    private void ExecuteClear()
    {
        Console.Clear();
    }

    private void ExecuteMkdir(string dir)
    {
        string dirPath = currentDirectory == "/" ? $"/{dir}" : $"{currentDirectory}/{dir}";
        if (fileSystem.Exists(entry => entry.Path == dirPath))
        {
            Console.WriteLine($"Directory already exists: {dir}");
        }
        else
        {
            fileSystem.Add(new FileSystemEntry(dirPath, true));
            Console.WriteLine($"Directory created: {dir}");
        }
    }

    private void ExecuteRmdir(string dir)
    {
        string path = currentDirectory == "/" ? $"/{dir}" : $"{currentDirectory}/{dir}";
        var entry = fileSystem.Find(entry => entry.Path == path); // && entry.IsDirectory

        if(!entry.IsDirectory)
        {
            Console.WriteLine($"Not a directory: {dir}");
        }
        else if (entry.Path != null)
        {
            if (IsDirectoryEmpty(path))
            {
                fileSystem.RemoveAll(entry => entry.Path == path);
                Console.WriteLine($"Directory removed: {dir}");
            }
            else
            {
                Console.WriteLine($"Directory not empty: {dir}");
            }
        }
        else
        {
            Console.WriteLine($"Directory not found: {dir}");
        }
    }

    private void ExecuteRm(string file)
    {
        string path = currentDirectory == "/" ? $"/{file}" : $"{currentDirectory}/{file}";
        var entry = fileSystem.Find(entry => entry.Path == path); // && !entry.IsDirectoryr

        if(entry.IsDirectory){
            Console.WriteLine($"Not a file: {file}");
        }
        else if (entry.Path != null)
        {
            fileSystem.RemoveAll(entry => entry.Path == path);
            Console.WriteLine($"File removed: {file}");
        }
        else
        {
            Console.WriteLine($"File not found: {file}");
        }
    }

    private void ExecuteExit()
    {
        Environment.Exit(0);
    }

    private void ExecuteHelp()
    {
        Console.WriteLine("Available commands:");
        Console.WriteLine("ls - List directory contents");
        Console.WriteLine("cd - Change the current directory");
        Console.WriteLine("cat - Display file content");
        Console.WriteLine("clear - Clear the screen");
        Console.WriteLine("mkdir - Create a new directory");
        Console.WriteLine("rmdir - Remove a directory");
        Console.WriteLine("rm - Remove a file");
        Console.WriteLine("exit - Exit the terminal");
    }

    private bool IsDirectoryEmpty(string dirPath)
    {
        foreach (var entry in fileSystem)
        {
            if (entry.Path.StartsWith($"{dirPath}/"))
            {
                return false;
            }
        }
        return true;
    }

    private string ReadMultiLineInput()
    {
        string content = "";
        string? line;
        while ((line = Console.ReadLine()) != ".")
        {
            content += line + Environment.NewLine;
        }
        return content.TrimEnd('\n');
    }

    private static void WriteColoredText(string text, ConsoleColor color)
    {
        Console.ForegroundColor = color;
        Console.Write(text);
        Console.ResetColor(); // 다른 색상으로 바꾸기 전에 색상 리셋
    }
}

class Program
{
    static void Main()
    {
        VirtualTerminal terminal = new VirtualTerminal();
        terminal.Run();
    }
}
