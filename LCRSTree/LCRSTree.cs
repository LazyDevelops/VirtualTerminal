namespace VirtualTerminal.LCRSTree
{
    public class Tree<T>
    {
        public T? Data;
        public Tree<T>? LeftChild;
        public Tree<T>? Parent;
        public Tree<T>? RightSibling;


        public Tree(T data)
        {
            Parent = null;
            LeftChild = null;
            RightSibling = null;
            Data = data;
        }

        public void AppendChildNode(Tree<T> child)
        {
            if (LeftChild == null)
            {
                LeftChild = child;
            }
            else
            {
                Tree<T> temp = LeftChild;

                while (temp.RightSibling != null)
                {
                    temp = temp.RightSibling;
                }

                temp.RightSibling = child;
            }

            child.Parent = this;
        }

        public List<Tree<T>> GetChildren()
        {
            List<Tree<T>> children = [];
            Tree<T>? child = LeftChild;

            while (child != null)
            {
                children.Add(child);
                child = child.RightSibling;
            }

            return children;
        }

        public void RemoveChildNode(Tree<T> nodeToRemove)
        {
            if (LeftChild == nodeToRemove)
            {
                LeftChild = nodeToRemove.RightSibling;
            }
            else
            {
                Tree<T>? currentChild = LeftChild;
                while (currentChild != null)
                {
                    if (currentChild.RightSibling == nodeToRemove)
                    {
                        currentChild.RightSibling = nodeToRemove.RightSibling;
                        return;
                    }

                    currentChild = currentChild.RightSibling;
                }
            }
        }
    }
}