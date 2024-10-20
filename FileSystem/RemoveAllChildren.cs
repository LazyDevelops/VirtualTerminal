using VirtualTerminal.LCRSTree;

namespace VirtualTerminal.FileSystem
{
    public partial class FileSystem
    {
        private static void RemoveAllChildren(Node<FileDataStruct> node)
        {
            foreach (Node<FileDataStruct> child in node.GetChildren())
            {
                RemoveAllChildren(child); // 재귀적으로 하위 노드 삭제
                node.RemoveChildNode(child);
            }
        }
    }
}