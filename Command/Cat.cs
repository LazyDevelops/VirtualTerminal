using Tree;
using VirtualTerminal.Error;
using VirtualTerminal.FileSystem;

namespace VirtualTerminal.Command
{
    public class CatCommand : VirtualTerminal.ICommand
    {
        public void Execute(int argc, string[] argv, VirtualTerminal VT)
        {
            Tree<FileNode>? file;
            Tree<FileNode>? parentFile;
            string? absolutePath;
            string? parentPath;
            string? fileName;
            bool[] permission;

            foreach (string arg in argv)
            {
                if (arg == argv[0] || arg.Contains('-') || arg.Contains("--"))
                {
                    continue;
                }

                absolutePath = VT.fileSystem.GetAbsolutePath(arg, VT.HOME, VT.PWD);
                fileName = absolutePath.Split('/')[^1];

                file = VT.fileSystem.FindFile(absolutePath, VT.root);

                if (file == null)
                {
                    parentPath = absolutePath.Replace('/' + fileName, "");
                    parentFile = VT.fileSystem.FindFile(parentPath, VT.root);

                    if (parentFile == null)
                    {
                        Console.WriteLine(ErrorMessage.NoSuchForD(argv[0], ErrorMessage.DefaultErrorComment(arg)));
                        return;
                    }

                    permission = VT.fileSystem.CheckFilePermission(VT.USER, parentFile, VT.root);

                    if (!permission[0] || !permission[1] || !permission[2])
                    {
                        Console.WriteLine(ErrorMessage.PermissionDenied(argv[0],
                            ErrorMessage.DefaultErrorComment(arg)));
                        return;
                    }

                    if (parentFile.Data.FileType != FileType.D)
                    {
                        Console.WriteLine(ErrorMessage.NotD(argv[0], ErrorMessage.DefaultErrorComment(arg)));
                        return;
                    }

                    Console.WriteLine($"파일 찾기 실패: {fileName}. 새로운 파일 만들기. 내용을 입력해주십시오. (점(.)만 찍고 엔터 치면 입력 종료):");
                    string content = VT.ReadMultiLineInput();
                    VT.fileSystem.CreateFile(parentPath, new FileNode(fileName, VT.USER, 0b110100, FileType.F, content), VT.root);
                    return;
                }

                permission = VT.fileSystem.CheckFilePermission(VT.USER, file, VT.root);

                if (!permission[0])
                {
                    Console.WriteLine(ErrorMessage.PermissionDenied(argv[0], ErrorMessage.DefaultErrorComment(arg)));
                    return;
                }

                if (file.Data.FileType == FileType.D)
                {
                    Console.WriteLine(ErrorMessage.NotF(argv[0], ErrorMessage.DefaultErrorComment(arg)));
                    return;
                }

                Console.WriteLine(file.Data.Content);
                return;
            }

            string? line;
            while ((line = Console.ReadLine()) != ".")
            {
                Console.WriteLine(line);
            }
        }

        public string Description(bool detail)
        {
            // 설명 약간의 수정 필요
            if (detail)
            {
                return "\u001b[1m간략한 설명\x1b[22m\n" +
                       "   cat - 파일 내용 출력\n\n" +
                       "\u001b[1m사용법\u001b[22m\n" +
                       "   cat [옵션] 파일명\n\n" +
                       "\u001b[1m설명\u001b[22m\n" +
                       "   위에 사용법을 이용하여 파일 내용을 출력 할 수 있으며\n" +
                       "   입출력 제지정자를 이용해 파일 생성 및 내용 작성 및 수정이 가능합니다.\n" +
                       "   (사용법은 예시 참조)\n\n" +
                       "\u001b[1m옵션\u001b[22m\n" +
                       "   -f\n" +
                       "       강제로 실행합니다.\n" +
                       "       (입출력 제지정자 일 때 사용 가능)\n\n" +
                       "\u001b[1m예시\u001b[22m\n" +
                       "   파일 내용 출력 예시\n" +
                       "       cat file.txt\n" +
                       "   입출력 지정자 예시\n" +
                       "       cat > file.txt\n" +
                       "       cat -f > file.txt\n" +
                       "       (-f를 이용해 파일 덮어쓰기 가능)\n";
            }

            return "cat - 파일 내용 출력";
        }
    }
}