namespace VirtualTerminal.Error
{
    public partial class ErrorMessage
    {
        public static string NoSuchForD(string command, string comment)
        {
            return $"{command}:{comment}: 그런 파일이나 디렉터리가 없습니다";
        }
    }
}