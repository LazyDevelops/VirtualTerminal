using Tree;
using VirtualTerminal.Errors;
using static FileSystem.FileSystem;

namespace VirtualTerminal.Commands
{
    public class CatCommand : VirtualTerminal.ICommand
    {
        public void Execute(string[] args, VirtualTerminal VT)
        {
            Tree<FileNode>? file;
            Tree<FileNode>? parentFile;

            string[]? splitPath;
            string? absolutePath;
            string? parentPath;

            string? fileName;
            
            bool[] permission;

            foreach (string arg in args)
            {
                if (arg != args[0] && !arg.Contains('-') && !arg.Contains("--"))
                {
                    absolutePath = VT.fileSystem.GetAbsolutePath(arg, VT.HOME, VT.PWD);
                    splitPath = absolutePath.Split('/');
                    fileName = splitPath[^1]; // path.Length - 1
                    parentPath = absolutePath.Replace('/' + fileName, "");

                    file = VT.fileSystem.FindFile(absolutePath, VT.root);
                    parentFile = VT.fileSystem.FindFile(parentPath, VT.root);

                    if(parentFile == null){
                        Console.WriteLine(ErrorsMassage.NoSuchForD(args[0], ErrorsMassage.DefaultErrorComment(arg)));
                        return;
                    }

                    if (file == null)
                    {
                        permission = VT.fileSystem.CheckFilePermission(VT.USER, parentFile, VT.root);

                        if(parentFile.Data.FileType != FileType.D){
                            Console.WriteLine(ErrorsMassage.NotD(args[0], ErrorsMassage.DefaultErrorComment(arg)));
                            return;
                        }

                        if(!permission[0] || !permission[1] || !permission[2])
                        {
                            Console.WriteLine(ErrorsMassage.PermissionDenied(args[0], ErrorsMassage.DefaultErrorComment(arg)));
                            return;
                        }

                        Console.WriteLine($"File not found: {fileName}. Creating new file. Enter content (end with a single dot on a line):");
                        string content = VT.ReadMultiLineInput();
                        VT.fileSystem.CreateFile(parentPath, new FileNode(fileName, VT.USER, 0b110100, FileType.F, content), VT.root);
                        return;
                    }

                    if (file.Data.FileType == FileType.D)
                    {
                        Console.WriteLine(ErrorsMassage.NotF(args[0], ErrorsMassage.DefaultErrorComment(arg)));
                        return;
                    }

                    permission = VT.fileSystem.CheckFilePermission(VT.USER, file, VT.root);

                    if(!permission[0])
                    {
                        Console.WriteLine(ErrorsMassage.PermissionDenied(args[0], ErrorsMassage.DefaultErrorComment(arg)));
                        return;
                    }

                    Console.WriteLine(file.Data.Content);
                }
            }
        }

        public string Description(bool detail)
        {
            if (detail)
            {
                return "\u001b[1m간략한 설명\x1b[22m\n" +
                       "   cat - 파일 내용 출력\n\n" +
                       "\u001b[1m사용법\u001b[22m\n" +
                       "   cat [옵션] 파일명*\n\n" +
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
            else
            {
                return "cat - 파일 내용 출력";
            }
        }
    }
}