using VirtualTerminal.LCRSTree;

namespace VirtualTerminal.FileSystem
{
    public partial class FileSystem
    {
        public static bool[] CheckPermission(string username, Tree<FileNode> file, Tree<FileNode> root)
        {
            Tree<FileNode>? curFile = file.Parents;
            bool[] permissions = [false, false, false, false, false, false];
            int min, max;

            if (curFile?.Parents != null)
            {
                curFile = curFile.Parents;
                min = curFile?.Data.UID == username ? 3 : 0;
                max = curFile?.Data.UID == username ? 5 : 2;

                while (min <= max)
                {
                    permissions[max - min + 3] = (curFile?.Data.Permission & (1 << min)) != 0;
                    min++;
                }
            }

            while (curFile != null && curFile != root)
            {
                min = curFile.Data.UID == username ? 3 : 0;
                max = curFile.Data.UID == username ? 5 : 2;

                while (min <= max)
                {
                    permissions[max - min] = (curFile.Data.Permission & (1 << min)) != 0;
                    min++;
                }

                if (!permissions[2] && !permissions[0])
                {
                    return permissions;
                }

                curFile = curFile.Parents;
            }

            min = file.Data.UID == username ? 3 : 0;
            max = file.Data.UID == username ? 5 : 2;

            while (min <= max)
            {
                permissions[max - min] = (file.Data.Permission & (1 << min)) != 0;
                min++;
            }

            return permissions;
        }
    }
}