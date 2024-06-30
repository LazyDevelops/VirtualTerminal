namespace Tree
{
    public class Tree<T>
    {
        public T? Data;
        public Tree<T>? Parents;
        public Tree<T>? LeftChild;
        public Tree<T>? RightSibling;

        public Tree() { }

        public Tree(T data)
        {
            Parents = null;
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

            child.Parents = this;
        }

        //public Tree<T> CopyNode()
        //{
        //    if (Data == null)
        //    {
        //        return new Tree<T>();
        //    }

        //    var newNode = new Tree<T>(Data);

        //    if (LeftChild != null)
        //    {
        //        newNode.LeftChild = LeftChild.CopyNode();
        //        newNode.LeftChild.Parents = newNode;
        //    }

        //    if (RightSibling != null)
        //    {
        //        newNode.RightSibling = RightSibling.CopyNode();
        //        newNode.RightSibling.Parents = newNode.Parents;
        //    }

        //    return newNode;
        //}

        public List<Tree<T>> GetChildren()
        {
            var children = new List<Tree<T>>();
            var child = LeftChild;

            while (child != null)
            {
                children.Add(child);
                child = child.RightSibling;
            }

            return children;
        }

        public void RemoveChildNode(Tree<T> nodeToRemove)
        {
            if (nodeToRemove == null)
            {
                return; // 삭제할 노드가 null인 경우 종료
            }

            if (LeftChild == nodeToRemove)
            {
                // 삭제할 노드가 첫 번째 자식인 경우
                LeftChild = nodeToRemove.RightSibling;
            }
            else
            {
                // 삭제할 노드가 첫 번째 자식이 아닌 경우
                Tree<T>? currentChild = LeftChild;
                while (currentChild != null)
                {
                    if (currentChild.RightSibling == nodeToRemove)
                    {
                        // 삭제할 노드를 찾은 경우
                        currentChild.RightSibling = nodeToRemove.RightSibling;
                        return;
                    }
                    currentChild = currentChild.RightSibling;
                }
            }
        }

        public void PrintTree(int depth)
        {
            for (int i = 0; i < depth; i++)
            {
                Console.Write(" ");
            }

            Console.WriteLine(Data);

            LeftChild?.PrintTree(depth + 1);
            RightSibling?.PrintTree(depth);
        }

        public void TestPrintTree(int depth)
        {
            for (int i = 0; i < depth - 1; i++)
            {
                Console.Write("    ");
            }

            if (depth != 0)
            {
                Console.Write("|-- ");
            }

            if (LeftChild == null)
            {
                Console.WriteLine(Data);
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.Blue;
                Console.WriteLine(Data);
                Console.ResetColor();
            }

            LeftChild?.TestPrintTree(depth + 1);
            RightSibling?.TestPrintTree(depth);
        }
    }
}