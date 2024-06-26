namespace VirtualTerminal.Commands
{
    public class EchoCommand : VirtualTerminal.ICommand
    {
        public void Execute(string[] args, VirtualTerminal VT)
        {   
            foreach (string arg in args)
            {
                if (arg != args[0])
                {
                    Console.Write(arg + " ");
                }
            }

            Console.WriteLine();
        }

        public string Description()
        {
            return "test";
        }
    }
}