namespace VirtualTerminal.Command
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

        public string Description(bool detail)
        {
            return "ehco - 입력한 테스트 출력";
        }
    }
}