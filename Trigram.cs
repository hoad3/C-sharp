using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

public class TrigramIndex
{
    private Dictionary<string, List<string>> index;

    public TrigramIndex(List<string> words)
    {
        index = new Dictionary<string, List<string>>();

        foreach (string word in words)
        {
            AddToIndex(word);
        }
    }

    private void AddToIndex(string word)
    {
        
        for (int i = 0; i <= word.Length - 3; i++)
        {
            string trigram = word.Substring(i, 3);
            if (!index.ContainsKey(trigram))
            {
                index[trigram] = new List<string>();
            }
            if (!index[trigram].Contains(word))
            {
                index[trigram].Add(word);
            }
        }
    }

    public List<string> Search(string keyword)
    {
        List<string> result = new List<string>();

       
        for (int i = 0; i <= keyword.Length - 3; i++)
        {
            string trigram = keyword.Substring(i, 3);
            if (index.ContainsKey(trigram))
            {
                result.AddRange(index[trigram]);
            }
        }

        
        result = result.Distinct().ToList();

        return result;
    }
}

class Program
{
    static void Main(string[] args)
    {
       
        List<string> words = new List<string> { "phần trăm"," Tập đoàn","quốc gia ","máy tính ","phần mềm"," kỹ thuật","dịch vụ","new","hợp nhất","dịch.","phần cứng","pineapple","phần" };

        // Build trigram index
        TrigramIndex trigramIndex = new TrigramIndex(words);

        
        string keyword = "phần";
        List<string> searchResult = trigramIndex.Search(keyword);
        Stopwatch stopwatch = Stopwatch.StartNew();
        Console.WriteLine("Search result for keyword '{0}':", keyword);
        foreach (string word in searchResult)
        {
            Console.WriteLine(word);
        }
        
    }
}