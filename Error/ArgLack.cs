namespace VirtualTerminal.Error
{
    public static partial class ErrorMessage
    {
        public static string ArgLack(string command)
        {
            return $"{command}: 인수 부족\n" +
                   $"자세한 사용법은 \"man {command}\"를 입력하십시오.";
        }
    }
}