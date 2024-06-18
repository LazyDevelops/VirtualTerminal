using System;

namespace Tree {
    class Tree<T> {
        public T? Data;
        private Tree<T>? LeftChild;
        private Tree<T>? RightSibling;

        public Tree() { }

        public Tree(T data) {
            LeftChild = null;
            RightSibling = null;
            Data = data;
        }

        public void AppendChildNode(Tree<T> child) {
            if (LeftChild == null) {
                LeftChild = child;
            } else {
                Tree<T> temp = LeftChild;

                while (temp.RightSibling != null) {
                    temp = temp.RightSibling;
                }

                temp.RightSibling = child;
            }
        }

        public void PrintTree(int depth) {
            for (int i = 0; i < depth; i++) {
                Console.Write(" ");
            }

            Console.WriteLine(Data);

            if (LeftChild != null) {
                LeftChild.PrintTree(depth + 1);
            }

            if (RightSibling != null) {
                RightSibling.PrintTree(depth);
            }
        }
    }
}