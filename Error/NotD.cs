namespace VirtualTerminal.Error
{
    public static partial class ErrorMessage
    {
        public static string NotD(string command, string comment)
        {
            return $"{command}:{comment}: 디렉터리가 아닙니다\n";
        }
    }
}