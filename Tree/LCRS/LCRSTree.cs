namespace VirtualTerminal.Tree.LCRS
{
    public class Node<T>
    {
        public T? Data { get; set; }
        public Node<T>? Parent { get; set; }
        public Node<T>? LeftChild { get; set; }
        public Node<T>? RightSibling { get; set; }

        public Node(T? data)
        {
            Data = data;
            Parent = null;
            LeftChild = null;
            RightSibling = null;
        }

        public void AppendChildNode(Node<T> node)
        {
            if (LeftChild == null)
            {
                LeftChild = node;
            }
            else
            {
                Node<T> temp = LeftChild;

                while (temp.RightSibling != null)
                {
                    temp = temp.RightSibling;
                }

                temp.RightSibling = node;
            }

            node.Parent = this;
        }

        public List<Node<T>> GetChildren()
        {
            List<Node<T>> children = [];
            Node<T>? child = LeftChild;

            while (child != null)
            {
                children.Add(child);
                child = child.RightSibling;
            }

            return children;
        }

        public void RemoveChildNode(Node<T> node)
        {
            if (LeftChild == node)
            {
                // 삭제할 노드가 첫 번째 자식인 경우
                LeftChild = node.RightSibling;
            }
            else
            {
                // 삭제할 노드가 첫 번째 자식이 아닌 경우
                Node<T>? currentChild = LeftChild;
                while (currentChild != null)
                {
                    if (currentChild.RightSibling == node)
                    {
                        // 삭제할 노드를 찾은 경우
                        currentChild.RightSibling = node.RightSibling;
                        return;
                    }

                    currentChild = currentChild.RightSibling;
                }
            }
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