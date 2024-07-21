namespace VirtualTerminal.Error
{
    public partial class ErrorMessage
    {
        public static string DNotEmpty(string command, string comment)
        {
            return $"{command}:{comment}: 디렉터리가 비어어있지 않습니다";
        }
    }
}