namespace VirtualTerminal.Error
{
    public partial class ErrorMessage
    {
        public static string InvalidMode(string command, string comment)
        {
            return $"{command}:{comment}: 잘못된 모드 입니다";
        }
    }
}