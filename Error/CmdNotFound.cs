namespace VirtualTerminal.Error
{
    public partial class ErrorMessage
    {
        public static string CmdNotFound(string command, string comment)
        {
            return $"{command}:{comment}: 명렁어를 찾을 수 없습니다";
        }
        
        public static string CmdNotFound(string comment)
        {
            return $"명렁어를 찾을 수 없습니다: {comment}";
        }
    }
}