using Tree;

namespace VirtualTerminal.FileSystem
{
    public partial class FileSystem
    {
        private static void RemoveAllChildren(Tree<FileNode> node)
        {
            foreach (Tree<FileNode> child in node.GetChildren())
            {
                RemoveAllChildren(child); // 재귀적으로 하위 노드 삭제
                node.RemoveChildNode(child);
            }
        }
    }
}