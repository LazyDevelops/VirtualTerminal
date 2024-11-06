namespace VirtualTerminal.Error
{
    public static partial class ErrorMessage
    {
        public static string MissingOperandAfter(string command, string comment)
        {
            return $"{command}: '{comment}' 뒤에 파일 경로가 빠짐\n";
        }
    }
}