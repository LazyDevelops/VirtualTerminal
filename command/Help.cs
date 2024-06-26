namespace VirtualTerminal.Commands
{
    public class HelpCommand : VirtualTerminal.ICommand
    {
        public void Execute(string[] args, VirtualTerminal VT)
        {
            Console.WriteLine("명령어 목록:");
            
            foreach(VirtualTerminal.ICommand action in VT.commandMap.Values){
                Console.WriteLine(action.Description());
            }
        }

        public string Description()
        {
            return "test";
        }
    }
}