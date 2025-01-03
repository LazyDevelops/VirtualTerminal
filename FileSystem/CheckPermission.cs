﻿using VirtualTerminal.Tree.General;

namespace VirtualTerminal.FileSystem
{
    public partial class FileSystem
    {
        public bool[] CheckPermission(string username, Node<FileDataStruct> file, Node<FileDataStruct> root)
        {
            Node<FileDataStruct>? curFile = file.Parent;
            bool[] permissions = [false, false, false, false, false, false];
            int min, max;

            if (curFile?.Parent != null)
            {
                curFile = curFile.Parent;
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

                curFile = curFile.Parent;
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