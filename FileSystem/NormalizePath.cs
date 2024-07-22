namespace VirtualTerminal.FileSystem
{
    public partial class FileSystem
    {
        private static string NormalizePath(string path)
        {
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