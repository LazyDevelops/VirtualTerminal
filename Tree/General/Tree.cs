// 트리 코드 삽입 예정
namespace VirtualTerminal.Tree.General
{
    public class Node<T>
    {
        public T? Data;
        public Node<T>? Parent;
        public List<Node<T>> Children;

        public Node(T? data)
        {
            Data = data;
            Parent = null;
        }
    }

    public class Tree<T>
    {
        public Node<T> Root;

        public Tree(Node<T> root)
        {
            Root = root;
        }
    }
}