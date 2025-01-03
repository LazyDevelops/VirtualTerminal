﻿using VirtualTerminal.Tree.General;

namespace VirtualTerminal.FileSystem
{
    public partial class FileSystem
    {
        public int FileRemove(string path, Node<FileDataStruct> root, char? option)
        {
            Node<FileDataStruct>? currentNode = FileFind(path, root);
            Node<FileDataStruct> parents;

            if (currentNode == null)
            {
                return 1;
            }

            if (currentNode.Parent == null)
            {
                return 2;
            }

            parents = currentNode.Parent;

            if (option != 'r' && currentNode.Children.Count != 0)
            {
                return 3;
            }

            if (currentNode.Children.Count != 0)
            {
                RemoveAllChildren(currentNode);
            }

            parents.RemoveChildWithNode(currentNode);
            return 0;
        }
    }
}