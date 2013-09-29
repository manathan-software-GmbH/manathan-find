namespace manathan.find.console
{
    using System;

    class Program
    {
        static void Main(string[] args)
        {
            SearchEngine.Initialize();
            while (true)
            {
                Console.WriteLine("Insert your query: ");
                var searchEngine = new SearchEngine();
                var results = searchEngine.Search(Console.ReadLine(), "");
                results.ForEach(_ => Console.WriteLine("{0} - {1} - {2}", _.Url, _.Title, _.Content));
            }
        }
    }
}
