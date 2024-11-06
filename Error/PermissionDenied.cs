namespace VirtualTerminal.Error
{
    public static partial class ErrorMessage
    {
        public static string PermissionDenied(string command, string comment)
        {
            return $"{command}:{comment}: 권한 부족\n";
        }
    }
}