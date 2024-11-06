namespace VirtualTerminal.Error
{
    public static partial class ErrorMessage
    {
        public static string FExists(string command, string comment)
        {
            return $"{command}:{comment}: 파일이 이미 있습니다\n";
        }
    }
}