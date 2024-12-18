﻿using VirtualTerminal.Tree.General;

namespace VirtualTerminal.FileSystem
{
    public partial class FileSystem
    {
        public Node<FileDataStruct>? FileFind(string? path, Node<FileDataStruct> root)
        {
            if (path == null)
            {
                return null;
            }

            Node<FileDataStruct> currentNode = root;
            List<string> files = [];
            string? fileName;

            path = path.TrimStart('/');
            files.AddRange(path.Split('/'));

            fileName = files.Last();

            if (files[0] == "")
            {
                return root;
            }

            foreach (string file in files)
            {
                foreach (Node<FileDataStruct> child in currentNode.Children.Where(child => child.Data.Name == file))
                {
                    currentNode = child;
                    break;
                }
            }

            return currentNode.Data.Name != fileName ? null : currentNode;
        }
    }
}
/*
using VirtualTerminal.Tree.General;

namespace VirtualTerminal.FileSystem
{
    public partial class FileSystem
    {
        public Node<FileDataStruct>? FileFind(string? path, Node<FileDataStruct> root)
        {
            if (string.IsNullOrEmpty(path))
            {
                return null;
            }

            Node<FileDataStruct> currentNode = root;
            List<string> files = new List<string>();

            // 경로 정리
            path = path.TrimStart('/');
            files.AddRange(path.Split('/'));

            if (files.Count == 0 || files[0] == "")
            {
                return root;
            }

            // 파일 트리 탐색
            foreach (string file in files)
            {
                Node<FileDataStruct>? nextNode = currentNode.Children.FirstOrDefault(child => child.Data.Name == file);

                if (nextNode == null)
                {
                    // 경로가 존재하지 않으면 null 반환
                    return null;
                }

                currentNode = nextNode;
            }

            return currentNode;
        }
    }
}
*/