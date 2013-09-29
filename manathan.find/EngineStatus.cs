namespace manathan.find
{
    #region

    using System;
    using Lucene.Net.Documents;

    #endregion

    public class EngineStatus
    {
        public static void IndexPage(string value)
        {
            Console.WriteLine("Started indexing page {0} ", value);
        }

        public static void IndexDocument(string value)
        {
            Console.WriteLine("Started indexing document {0} ", value);
        }
    }
}