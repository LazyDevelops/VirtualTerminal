namespace VirtualTerminal.Commands
{
    public class HelpCommand : VirtualTerminal.ICommand
    {
        public void Execute(string[] args, VirtualTerminal VT)
        {
            Console.WriteLine("man [명령어]를 이용해 더 자세한 내용을 볼 수 있습니다.\n");
            Console.WriteLine("명령어 목록:");

            foreach (VirtualTerminal.ICommand action in VT.commandMap.Values){
                Console.WriteLine(action.Description());
            }
        }

        public string Description()
        {
            return "help - 모든 명령어의 간단한 사용방법 출력";
        }
    }
}