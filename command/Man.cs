namespace VirtualTerminal.Commands
{
    public class ManCommand : VirtualTerminal.ICommand
    {
        public void Execute(string[] args, VirtualTerminal VT)
        {
            if (VT.commandMap.TryGetValue(args[1], out var action))
            {
                Console.WriteLine(action.Description());
            }
            else
            {
                Console.WriteLine($"{args[0]}: {args[1]}: 명렁어를 찾을 수 없습니다.");
            }
        }

        public string Description()
        {
            return "test";
        }
    }
}