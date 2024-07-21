using Tree;

namespace VirtualTerminal.FileSystem
{
    public partial class FileSystem
    {
        private void RemoveAllChildren(Tree<FileNode> node)
        {
            foreach (Tree<FileNode> child in node.GetChildren().ToList())
            {
                RemoveAllChildren(child); // 재귀적으로 하위 노드 삭제
                node.RemoveChildNode(child);
            }
        }
    }
}