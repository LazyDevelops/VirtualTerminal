using Tree;

namespace VirtualTerminal.FileSystem
{
    public class FileSystem
    {
        public struct FileNode(string name, string UID, byte permission, FileType fileType, string? content = null)
        {
            public string Name { get; set; } = name;
            public byte Permission { get; set; } = permission;
            public string UID { get; set; } = UID;
            public FileType FileType { get; set; } = fileType;
            public string? Content { get; set; } = content;

            public override string ToString()
            {
                return Name;
            }
        }

        public enum FileType
        {
            F = '-', // 일반 파일
            D = 'd', // 디렉터리 파일
            I = 'i' // 아이템 파일
        }

        public Tree<FileNode>? CreateFile(string path, FileNode entry, Tree<FileNode> root)
        {
            Tree<FileNode>? current = FindFile(path, root);

            if (current == null)
            {
                return null;
            }

            Tree<FileNode> newFile = new(entry);
            current.AppendChildNode(newFile);

            return newFile;
        }

        public int RemoveFile(string path, Tree<FileNode> root, char? option)
        {
            Tree<FileNode>? currentNode = FindFile(path, root);
            Tree<FileNode> parents;

            if (currentNode == null)
            {
                return 1;
            }

            if (currentNode.Parents == null)
            {
                return 2;
            }

            parents = currentNode.Parents;

            if (option == 'r' || currentNode.LeftChild == null)
            {
                if(currentNode.LeftChild != null){
                    RemoveAllChildren(currentNode);
                }
                parents.RemoveChildNode(currentNode);
                return 0;
            }

            return 3;
        }

        private void RemoveAllChildren(Tree<FileNode> node)
        {
            foreach (var child in node.GetChildren().ToList())
            {
                RemoveAllChildren(child); // 재귀적으로 하위 노드 삭제
                node.RemoveChildNode(child);
            }
        }

        public Tree<FileNode>? FindFile(string path, Tree<FileNode> root)
        {
            Tree<FileNode> currentNode = root;
            var files = new List<string>();
            string? fileName;

            path = path.TrimStart('/');
            files.AddRange(path.Split('/'));

            fileName = files.Last();

            if (files[0] == "")
            {
                return root;
            }
            else
            {
                foreach (string file in files)
                {
                    foreach (Tree<FileNode> child in currentNode.GetChildren())
                    {
                        if (child.Data.Name == file)
                        {
                            currentNode = child;
                            break;
                        }
                    }
                }

                if (currentNode.Data.Name != fileName)
                {
                    return null;
                }

                return currentNode;
            }
        }

        public string ConvertPermissionsToString(short permissions)
        {
            string result = string.Empty;
            result += (permissions & 040) != 0 ? "r" : "-";
            result += (permissions & 020) != 0 ? "w" : "-";
            result += (permissions & 010) != 0 ? "x" : "-";
            result += (permissions & 004) != 0 ? "r" : "-";
            result += (permissions & 002) != 0 ? "w" : "-";
            result += (permissions & 001) != 0 ? "x" : "-";
            return result;
        }

        public bool[] CheckFilePermission(string username, Tree<FileNode> file, Tree<FileNode> root)
        {
            Tree<FileNode>? curFile = file.Parents;
            var permissions = new bool[6] { false, false, false, false, false, false };
            int min, max;

            if(curFile?.Parents != null)
            {
                curFile = curFile?.Parents;
                min = curFile?.Data.UID == username ? 3 : 0;
                max = curFile?.Data.UID == username ? 5 : 2;

                while (min <= max)
                {
                    permissions[max - min + 3] = (curFile?.Data.Permission & (1 << min)) != 0;
                    min++;
                }
            }

            while(curFile != null && curFile != root)
            {
                min = curFile?.Data.UID == username ? 3 : 0;
                max = curFile?.Data.UID == username ? 5 : 2;

                while(min <= max)
                {
                    permissions[max - min] = (curFile?.Data.Permission & (1 << min)) != 0;
                    min++;
                }

                if(!permissions[2] && !permissions[0])
                {
                    return permissions;
                }

                curFile = curFile?.Parents;
            }

            min = file.Data.UID == username ? 3 : 0;
            max = file.Data.UID == username ? 5 : 2;


            while(min <= max)
            {
                permissions[max - min] = (file.Data.Permission & (1 << min)) != 0;
                min++;
            }

            return permissions;
        }

        // path stack이나 list로 수정 고려
        public string GetAbsolutePath(string path, string HomeDirectory, string CurrentDirectory)
        {
            if (string.IsNullOrEmpty(path))
            {
                throw new ArgumentException("Path cannot be null or empty.", nameof(path));
            }

            if (path.StartsWith('/'))
            {
                // 절대 경로인 경우
                return NormalizePath(path);
            }

            if (path.StartsWith("~/"))
            {
                // 홈 디렉터리(~)로 시작하는 경우
                return NormalizePath(HomeDirectory + "/" + path.Remove(0, 2));
            }

            if (path == "~")
            {
                return HomeDirectory;
            }

            if (path == ".")
            {
                // 현재 디렉터리를 나타내는 경우
                return CurrentDirectory;
            }

            if (path == "..")
            {
                // 부모 디렉터리를 나타내는 경우
                int lastSlashIndex = CurrentDirectory.LastIndexOf('/');

                if (lastSlashIndex > 0)
                {
                    return CurrentDirectory.Remove(lastSlashIndex);
                }
                else
                {
                    // 루트 디렉터리인 경우
                    return "/";
                }
            }

            // 파일 이름만 주어진 경우, 현재 디렉터리를 기준으로 절대 경로 생성
            return NormalizePath(CurrentDirectory + "/" + path);
        }

        private string NormalizePath(string path)
        {
            var parts = path.Split('/', StringSplitOptions.RemoveEmptyEntries);
            var stack = new Stack<string>();

            foreach (var part in parts)
            {
                if (part == ".")
                {
                    // 현재 디렉터리, 무시
                    continue;
                }
                else if (part == "..")
                {
                    // 부모 디렉터리, 스택에서 하나 제거
                    if (stack.Count > 0)
                    {
                        stack.Pop();
                    }
                }
                else
                {
                    // 일반 디렉터리 이름, 스택에 추가
                    stack.Push(part);
                }
            }

            var result = new List<string>(stack);
            result.Reverse();
            return "/" + string.Join("/", result);
        }
    }
}
