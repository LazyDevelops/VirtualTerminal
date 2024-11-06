// 트리 코드 삽입 예정

namespace VirtualTerminal.Tree.General
{
    public class Node<T>
    {
        public List<Node<T>> Children;
        public T? Data;
        public Node<T>? Parent;

        public Node(T? data)
        {
            Data = data;
            Parent = null;
            Children = new List<Node<T>>();
        }

        public void AppendChildNode(Node<T> node)
        {
            node.Parent = this;
            Children.Add(node);
        }

        public void RemoveChildWithNode(Node<T> node)
        {
            Children.Remove(node);
        }

        public void RemoveChildWithIndex(int index)
        {
            if (index < 0 || index > Children.Count - 1)
            {
                return;
            }

            Children.Remove(Children[index]);
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