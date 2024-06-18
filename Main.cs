using System;
using System.Collections.Generic;
using Tree;
// using VirtualTerminal;

class Program
{
    static void Main()
    {
        Tree<int> root = new Tree<int>(1);
        Console.WriteLine(root.Data);
    }
}