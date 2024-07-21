namespace VirtualTerminal.FileSystem
{
    public partial class FileSystem
    {
        public string GetAbsolutePath(string path, string homeDirectory, string currentDirectory)
        {
            if (path.StartsWith('/'))
            {
                // 절대 경로인 경우
                return NormalizePath(path);
            }

            if (path.StartsWith("~/"))
            {
                // 홈 디렉터리(~)로 시작하는 경우
                return NormalizePath(homeDirectory + "/" + path.Remove(0, 2));
            }

            if (path == "~")
            {
                return homeDirectory;
            }

            if (path == ".")
            {
                // 현재 디렉터리를 나타내는 경우
                return currentDirectory;
            }

            if (path == "..")
            {
                // 부모 디렉터리를 나타내는 경우
                int lastSlashIndex = currentDirectory.LastIndexOf('/');
                return lastSlashIndex > 0 ? currentDirectory.Remove(lastSlashIndex) : "/";
            }

            // 파일 이름만 주어진 경우, 현재 디렉터리를 기준으로 절대 경로 생성
            return NormalizePath(currentDirectory + "/" + path);
        }
    }
}