namespace VirtualTerminal.Error
{
    public partial class ErrorMessage
    {
        public static string NotF(string command, string comment)
        {
            return $"{command}:{comment}: 파일이 아닙니다";
        }
    }
}