namespace VirtualTerminal.Error
{
    public static partial class ErrorMessage
    {
        public static string CmdNotFound(string command, string comment)
        {
            return $"{command}:{comment}: 명렁어를 찾을 수 없습니다\n";
        }

        public static string CmdNotFound(string comment)
        {
            return $"명렁어를 찾을 수 없습니다: {comment}\n";
        }
    }
}