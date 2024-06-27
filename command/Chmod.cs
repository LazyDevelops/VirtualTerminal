using Tree;
using VirtualTerminal.Errors;
using static FileSystem.FileSystem;

namespace VirtualTerminal.Commands
{
    public class ChmodCommand : VirtualTerminal.ICommand
    {
        public void Execute(string[] args, VirtualTerminal VT)
        {
            Tree<FileNode>? file;
            string? absolutePath;
            bool[] permission;
            byte? inputPermission;

            foreach (string arg in args)
            {
                if(arg.Length == 6){
                    if(arg.Contains('1') || arg.Contains('0')){
                        // int count = 0;
                        // foreach(char c in arg){
                        //     inputPermission[count] = Convert.ToBoolean(c);
                        //     count++;
                        //     byte result = 0;
                        // for (int i = 0; i < boolArray.Length; i++)
                        // {
                        //     if (boolArray[i])
                        //     {
                        //         result |= (byte)(1 << i);
                        //     }
                        // }
                        // }

                        inputPermission = Convert.ToByte(arg.PadLeft(8, '0'), 2);
                        Console.WriteLine(inputPermission);
                    }
                }
                else if (arg != args[0] && !arg.Contains('-') && !arg.Contains("--"))
                {
                    absolutePath = VT.fileSystem.GetAbsolutePath(arg, VT.HOME, VT.PWD);
                    file = VT.fileSystem.FindFile(absolutePath, VT.root);

                    if (file == null)
                    {
                        Console.WriteLine(ErrorsMassage.NoSuchForD(args[0], ErrorsMassage.DefaultErrorComment(arg)));
                        return;
                    }

                    permission = VT.fileSystem.CheckFilePermission(VT.USER, file, VT.root);

                    if(file.Data.UID != VT.USER || !permission[0] || !permission[1] || !permission[2]){
                        Console.WriteLine(ErrorsMassage.PermissionDenied(args[0], ErrorsMassage.DefaultErrorComment(arg)));
                        return;
                    }
                }
            }

            // file?.Data.Permission = inputPermission;
        }

        public string Description(bool detail)
        {
            return "chmod - ������ ���� ����";
        }
    }
}