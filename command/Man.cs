namespace VirtualTerminal.Commands
{
    public class ManCommand : VirtualTerminal.ICommand
    {
        public void Execute(string[] args, VirtualTerminal VT)
        {
            //if (VT.commandMap.TryGetValue(args[1], out var action))
            //{
            //    Console.Write(action.Description());
            //}
            //else
            //{
            //    Console.WriteLine($"Command not found: {args[0]}");
            //}
        }
    }
}