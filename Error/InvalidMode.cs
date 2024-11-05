namespace VirtualTerminal.Error
{
    public static partial class ErrorMessage
    {
        public static string InvalidMode(string command, string comment)
        {
            return $"{command}:{comment}: 잘못된 모드 입니다\n";
        }
    }
}