namespace VirtualTerminal.FileSystem
{
    public partial class FileSystem
    {
        public static string GetAbsolutePath(string path, string homeDirectory, string currentDirectory)
        {
            if (!path.StartsWith('/'))
            {
                // 파일 이름만 주어진 경우, 현재 디렉터리를 기준으로 절대 경로 생성
                path = currentDirectory + "/" + path;
            }

            if (path.StartsWith('~'))
            {
                // 홈 디렉터리(~)로 시작하는 경우
                path = homeDirectory + path.Remove(0, 1);
            }

            string[] parts = path.Split('/', StringSplitOptions.RemoveEmptyEntries);
            Stack<string> stack = new();

            foreach (string part in parts)
            {
                switch (part)
                {
                    case ".":
                        // 현재 디렉터리, 무시
                        continue;
                    case "..":
                        {
                            // 부모 디렉터리, 스택에서 하나 제거
                            if (stack.Count > 0)
                            {
                                stack.Pop();
                            }

                            break;
                        }
                    default:
                        // 일반 디렉터리 이름, 스택에 추가
                        stack.Push(part);
                        break;
                }
            }

            List<string> result = [..stack];
            result.Reverse();
            return "/" + string.Join("/", result);
        }
    }
}