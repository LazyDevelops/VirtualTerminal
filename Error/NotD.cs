namespace VirtualTerminal.Error
{
    public partial class ErrorMessage
    {
        public static string NotD(string command, string comment)
        {
            return $"{command}:{comment}: 디렉터리가 아닙니다";
        }
    }
}