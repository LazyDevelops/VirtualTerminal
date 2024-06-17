using System;
using System.Collections.Generic;

// ls - O
// cat - O
// get <U, D, R, L> - W
// go <U, D, R, L> - W
// mkdir - O
// rmdir - O
// mv - O
// rm - O
// unlock - W
// login - W
// exit - O
// mapping - W
// man
// help - O

class VirtualTerminal
{
    private struct FileSystemEntry
    {
        public string path { get; }
        public string? content { get; }
        public byte permission { get; }
        public string UID { get; }
        public enum FileType { 
            F, D 
        } public FileType fileType { get; }

        public FileSystemEntry(string path, string UID, byte permission, FileType fileType, string? content = null)
        {
            this.path = path;
            this.UID = UID;
            this.permission = permission;
            this.fileType = fileType;
            this.content = content;
        }
    }

    private List<FileSystemEntry> fileSystem;
    private string currentDirectory;
    private string homeDirectory;
    private string currentUser;

    public VirtualTerminal()
    {
        currentUser = "user";
        currentDirectory = $"/home/{currentUser}";
        homeDirectory = $"/home/{currentUser}";

        fileSystem = new List<FileSystemEntry>
        {
            // 8진수 입력 안됨
            new FileSystemEntry("/", "root", 0b111101, 1),
            new FileSystemEntry("/root", "root", 0b111000, 1),
            new FileSystemEntry("/home", "root", 0b111101, 1),
            new FileSystemEntry("/home/user", currentUser, 0b111101, 1),
            new FileSystemEntry($"/home/user/Hello_{currentUser}.txt", "root", 0b111111, 0, $"Hello, {currentUser}!")
        };
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
        // homeDirectory 일시 ~ 표시 코드 짜기
        // if(!currentDirectory.Equals(homeDirectory))
        //     WriteColoredText("\x1b[1m~\x1b[22m", ConsoleColor.Blue);
        // else
        //     WriteColoredText("\x1b[1m/\x1b[22m", ConsoleColor.Blue);
        WriteColoredText($"\x1b[1m{currentDirectory}\x1b[22m", ConsoleColor.Blue);
        WriteColoredText("$ ", Console.ForegroundColor);
    }

    private void ProcessCommand(string command)
    {
        string[] args = command.Split(' ', 2);

        switch (args[0])
        {
            case "cd":
                ExecuteCd(args);
                break;
            case "cat":
                ExecuteCat(args);
                break;
            case "clear":
                ExecuteClear();
                break;
            case "exit":
                ExecuteExit();
                break;
            case "help":
                ExecuteHelp();
                break;
            case "ls":
                ExecuteLs(args);
                break;
            case "mkdir":
                ExecuteMkdir(args);
                break;
            case "rm":
                ExecuteRm(args);
                break;
            case "rmdir":
                ExecuteRmdir(args);
                break;
            default:
                Console.WriteLine($"Command not found: {command}");
                break;
        }
    }

    private void ExecuteLs(string[] args)
    {
        HashSet<string> seen = new HashSet<string>();
        // 수정 필요
        string options = args.Length > 1 ? args[1] : string.Empty;
        // 수정 필요
        bool detailed = options.Contains("-l");

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
                    if (detailed)
                    {
                        string type = entry.IsDirectory ? "d" : "-";
                        string permissions = ConvertPermissionsToString(entry.Permission);
                        Console.WriteLine($"{type}{permissions} {entry.UID} {relativePath}");
                    }
                    else
                    {
                        Console.WriteLine(relativePath);
                    }
                    seen.Add(relativePath);
                }
            }
        }
    }

    private void ExecuteCd(string[] args)
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

    private void ExecuteCat(string[] args)
    {
        string path = currentDirectory == "/" ? $"/{file}" : $"{currentDirectory}/{file}";
        var entry = fileSystem.Find(entry => entry.Path == path);

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
            fileSystem.Add(new FileSystemEntry(path, currentUser, 0b110100, 0, content));
            Console.WriteLine("File created.");
        }
    }

    private void ExecuteClear()
    {
        Console.Clear();
    }

    private void ExecuteMkdir(string[] args)
    {
        string dirPath = currentDirectory == "/" ? $"/{dir}" : $"{currentDirectory}/{dir}";
        if (fileSystem.Exists(entry => entry.Path == dirPath))
        {
            Console.WriteLine($"Directory already exists: {dir}");
        }
        else
        {
            fileSystem.Add(new FileSystemEntry(dirPath, currentUser, 0b111101, 1));
            Console.WriteLine($"Directory created: {dir}");
        }
    }

    private void ExecuteRmdir(string[] args)
    {
        string path = currentDirectory == "/" ? $"/{dir}" : $"{currentDirectory}/{dir}";
        var entry = fileSystem.Find(entry => entry.Path == path);

        if (entry.Path == null)
        {
            Console.WriteLine($"Directory not found: {dir}");
        }
        else if (!entry.IsDirectory)
        {
            Console.WriteLine($"Not a directory: {dir}");
        }
        else
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
    }

    private void ExecuteRm(string[] args)
    {
        string path = currentDirectory == "/" ? $"/{file}" : $"{currentDirectory}/{file}";
        var entry = fileSystem.Find(entry => entry.Path == path);

        if (entry.Path == null)
        {
            Console.WriteLine($"File not found: {file}");
        }
        else if (entry.IsDirectory)
        {
            Console.WriteLine($"Not a file: {file}");
        }
        else
        {
            fileSystem.RemoveAll(entry => entry.Path == path);
            Console.WriteLine($"File removed: {file}");
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

    // 수정 필요
    private string ConvertPermissionsToString(short permissions)
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

    private bool IsDirectoryEmpty(string[] args)
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
        Console.ResetColor();
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
