namespace VirtualTerminal.Error{
    public class ErrorMessage
    {
        public static string NoSuchForD(string command, string comment)
        {
            return $"{command}:{comment}: 그런 파일이나 디렉터리가 없습니다";
        }

        public static string PermissionDenied(string command, string comment)
        {
            return $"{command}:{comment}: 권한 부족";
        }

        public static string NotD(string command, string comment)
        {
            return $"{command}:{comment}: 디렉터리가 아닙니다";
        }

        public static string NotF(string command, string comment)
        {
            return $"{command}:{comment}: 파일이 아닙니다";
        }

        public static string FExists(string command, string comment)
        {
            return $"{command}:{comment}: 파일이 이미 있습니다";
        }

        public static string DNotEmpty(string command, string comment){
            return $"{command}:{comment}: 디렉터리가 비어어있지 않습니다";
        }

        public static string CmdNotFound(string command, string comment){
            return $"{command}:{comment}: 명렁어를 찾을 수 없습니다";
        }

        public static string CmdNotFound(string comment){
            return $"명렁어를 찾을 수 없습니다: {comment}";
        }

        public static string InvalidMode(string command, string comment){
            return $"{command}:{comment}: 잘못된 모드 입니다";
        }

        public static string MissingOperandAfter(string command, string comment){
            return $"{command}: '{comment}' 뒤에 파일 경로가 빠짐";
        }

        public static string ArgLack(string command){
            return $"{command}: 인수 부족\n"+
                   $"자세한 사용법은 \"man {command}\"를 입력하십시오.";
        }

        public static string DefaultErrorComment(string input){
            return $" {input}";
        }
    }
}