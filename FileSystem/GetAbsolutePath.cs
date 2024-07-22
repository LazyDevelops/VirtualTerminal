namespace VirtualTerminal.FileSystem
{
    public partial class FileSystem
    {
        public static string GetAbsolutePath(string path, string homeDirectory, string currentDirectory)
        {
            if (path.StartsWith('/'))
            {
                // 절대 경로인 경우
                return NormalizePath(path);
            }

            if (path.StartsWith('~'))
            {
                // 홈 디렉터리(~)로 시작하는 경우
                return NormalizePath(homeDirectory + "/" + path.Remove(0, 1));
            }

            // 파일 이름만 주어진 경우, 현재 디렉터리를 기준으로 절대 경로 생성
            return NormalizePath(currentDirectory + "/" + path);
        }
    }
}