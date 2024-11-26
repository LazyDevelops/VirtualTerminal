using VirtualTerminal.Error;
using VirtualTerminal.FileSystem;
using VirtualTerminal.Tree.General;

// 수정 필요

namespace VirtualTerminal.Command
{
    public class LsCommand : VirtualTerminal.ICommand
    {
        public string? Execute(int argc, string[] argv, VirtualTerminal VT)
        {
            Node<FileDataStruct>? file;
            List<Node<FileDataStruct>> files = [];
            List<string> inputFilesArg = [];
            string? absolutePath;
            bool[] permission;

            Dictionary<string, bool> options = new() { { "l", false } };

            VirtualTerminal.OptionCheck(ref options, in argv);

            foreach (string arg in argv.Skip(1))
            {
                if (arg.Contains('-') || arg.Contains("--"))
                {
                    continue;
                }

                absolutePath = VT.FileSystem.GetAbsolutePath(arg, VT.HOME, VT.PWD);

                file = VT.FileSystem.FileFind(absolutePath, VT.Root);

                if (file == null)
                {
                    return ErrorMessage.NoSuchForD(argv[0], ErrorMessage.DefaultErrorComment(arg));
                }

                permission = VT.FileSystem.CheckPermission(VT.USER, file, VT.Root);

                if (!permission[0])
                {
                    return ErrorMessage.PermissionDenied(argv[0], ErrorMessage.DefaultErrorComment(arg));
                }

                if (file.Data.FileType != FileType.D)
                {
                    return arg;
                }
                
                files.Add(file);
                inputFilesArg.Add(arg);
            }

            if (files.Count == 0)
            {
                if(VT.PwdNode == null){
                    return null;
                }
                files.Add(VT.PwdNode);
            }

            string? result = null;
            
            for(int i = 0; i < files.Count; i++)
            {
                if(files.Count > 1)
                {
                    result += $"{inputFilesArg[i]}:\n";
                }

                if (options["l"])
                {
                    result += $"total {files[i].Children.Count}\n";
                }

                foreach (Node<FileDataStruct> fileChild in files[i].Children)
                {
                    permission = VT.FileSystem.CheckPermission(VT.USER, fileChild, VT.Root);

                    if (options["l"])
                    {
                        string permissions = VT.FileSystem.PermissionsToString(fileChild.Data.Permission);
                        DateTimeOffset dateTimeOffset = DateTimeOffset.FromUnixTimeSeconds(fileChild.Data.LastTouchTime);

                        int year = dateTimeOffset.Year;
                        int month = dateTimeOffset.Month;
                        int day = dateTimeOffset.Day;
                        int hour = dateTimeOffset.Hour;
                        int minute = dateTimeOffset.Minute;

                        string time = $"{year}년 {month}월 {day} {hour}:{minute}";
                        result += $"{Convert.ToChar(fileChild.Data.FileType)}{permissions} {fileChild.Data.UID} {time} ";
                    }


                    if (fileChild.Data.FileType == FileType.D)
                    {
                        result += $"\u001b[34m{fileChild.Data.Name}\u001b[0m";
                    }
                    else if (permission[2])
                    {
                        result += $"\u001b[32m{fileChild.Data.Name}\u001b[0m";
                    }
                    else
                    {
                        result += fileChild.Data.Name;
                    }

                    result += "\n";
                }

                if(i != files.Count - 1){
                    result += "\n";
                }
            }

            return result;
        }

        public string Description(bool detail)
        {
            if (detail)
            {
                return "ls - 현제 디렉터리에 있는 디렉터리 및 파일 목록 출력\n";
            }

            return "ls - 현제 디렉터리에 있는 디렉터리 및 파일 목록 출력";
        }
    }
}