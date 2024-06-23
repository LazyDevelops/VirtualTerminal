namespace VirtualTerminal
{
    public partial class VirtualTerminal
    {
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
    }
}