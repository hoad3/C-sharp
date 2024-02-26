using System;
using System.Collections.Generic;

class TrieNode
{
    public Dictionary<char, TrieNode> Children { get; set; }
    public List<string> Output { get; set; }
    public TrieNode Failure { get; set; }

    public TrieNode()
    {
        Children = new Dictionary<char, TrieNode>();
        Output = new List<string>();
        Failure = null;
    }
}

class AhoCorasick
{
    private TrieNode BuildTrie(IEnumerable<string> keywords)
    {
        TrieNode root = new TrieNode();

        foreach (var keyword in keywords)
        {
            TrieNode node = root;

            foreach (char c in keyword)
            {
                if (!node.Children.ContainsKey(c))
                    node.Children[c] = new TrieNode();

                node = node.Children[c];
            }

            node.Output.Add(keyword);
        }

        return root;
    }

    private void BuildFailureLinks(TrieNode root)
    {
        Queue<TrieNode> queue = new Queue<TrieNode>();

        foreach (var node in root.Children.Values)
        {
            node.Failure = root;
            queue.Enqueue(node);
        }

        while (queue.Count > 0)
        {
            TrieNode currentNode = queue.Dequeue();

            foreach (var kvp in currentNode.Children)
            {
                char c = kvp.Key;
                TrieNode child = kvp.Value;

                queue.Enqueue(child);

                TrieNode failureNode = currentNode.Failure;

                while (failureNode != null && !failureNode.Children.ContainsKey(c))
                    failureNode = failureNode.Failure;

                child.Failure = failureNode?.Children[c] ?? root;
                child.Output.AddRange(child.Failure.Output);
            }
        }
    }

    public List<(int, int, string)> SearchText(IEnumerable<string> keywords, string text)
    {
        TrieNode root = BuildTrie(keywords);
        BuildFailureLinks(root);

        List<(int, int, string)> foundKeywords = new List<(int, int, string)>();
        TrieNode currentState = root;

        for (int i = 0; i < text.Length; i++)
        {
            char currentChar = text[i];

            while (currentState != null && !currentState.Children.ContainsKey(currentChar))
                currentState = currentState.Failure;

            if (currentState == null)
            {
                currentState = root;
                continue;
            }

            currentState = currentState.Children[currentChar];

            foreach (var keyword in currentState.Output)
            {
                int start = i - keyword.Length + 1;
                int end = i;
                foundKeywords.Add((start, end, keyword));
            }
        }

        return foundKeywords;
    }
}

/*class Program
{
    static void Main()
    {
        AhoCorasick ahoCorasick = new AhoCorasick();
        List<string> keywords = new List<string> { "he", "she", "his", "hers", "us" };
        string text = "ushers";

        List<(int, int, string)> result = ahoCorasick.SearchText(keywords, text);

        Console.WriteLine("Found keywords:");
        foreach (var (start, end, keyword) in result)
        {
            Console.WriteLine($"Keyword '{keyword}' found from index {start} to {end}");
        }
    }
}*/

    

