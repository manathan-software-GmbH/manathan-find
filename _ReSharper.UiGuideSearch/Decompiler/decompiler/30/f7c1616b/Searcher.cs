// Type: Lucene.Net.Search.Searcher
// Assembly: Lucene.Net, Version=2.0.0.4, Culture=neutral, PublicKeyToken=null
// Assembly location: \\Osqlclr006\store$\mkainer\Documents\Visual Studio 2012\Projects\UiGuideSearch\UiGuideSearch\bin\Lucene.Net.dll

using Lucene.Net.Documents;
using Lucene.Net.Index;

namespace Lucene.Net.Search
{
  public abstract class Searcher : Searchable
  {
    /// <summary>
    /// The Similarity implementation used by this searcher.
    /// </summary>
    private Similarity similarity;

    public Searcher()
    {
      this.InitBlock();
    }

    private void InitBlock()
    {
      this.similarity = Similarity.GetDefault();
    }

    /// <summary>
    /// Returns the documents matching
    /// <code>
    /// query
    /// </code>
    /// .
    /// </summary>
    /// <throws>BooleanQuery.TooManyClauses </throws>
    public Hits Search(Query query)
    {
      return this.Search(query, (Filter) null);
    }

    /// <summary>
    /// Returns the documents matching
    /// <code>
    /// query
    /// </code>
    ///  and
    /// 
    /// <code>
    /// filter
    /// </code>
    /// .
    /// 
    /// </summary>
    /// <throws>BooleanQuery.TooManyClauses </throws>
    public virtual Hits Search(Query query, Filter filter)
    {
      return new Hits(this, query, filter);
    }

    /// <summary>
    /// Returns documents matching
    /// <code>
    /// query
    /// </code>
    ///  sorted by
    /// 
    /// <code>
    /// sort
    /// </code>
    /// .
    /// 
    /// </summary>
    /// <throws>BooleanQuery.TooManyClauses </throws>
    public virtual Hits Search(Query query, Sort sort)
    {
      return new Hits(this, query, (Filter) null, sort);
    }

    /// <summary>
    /// Returns documents matching
    /// <code>
    /// query
    /// </code>
    ///  and
    /// <code>
    /// filter
    /// </code>
    /// ,
    ///             sorted by
    /// <code>
    /// sort
    /// </code>
    /// .
    /// 
    /// </summary>
    /// <throws>BooleanQuery.TooManyClauses </throws>
    public virtual Hits Search(Query query, Filter filter, Sort sort)
    {
      return new Hits(this, query, filter, sort);
    }

    public virtual TopFieldDocs Search(Query query, Filter filter, int n, Sort sort)
    {
      return this.Search(this.CreateWeight(query), filter, n, sort);
    }

    public virtual void Search(Query query, HitCollector results)
    {
      this.Search(query, (Filter) null, results);
    }

    public virtual void Search(Query query, Filter filter, HitCollector results)
    {
      this.Search(this.CreateWeight(query), filter, results);
    }

    public virtual TopDocs Search(Query query, Filter filter, int n)
    {
      return this.Search(this.CreateWeight(query), filter, n);
    }

    public virtual Explanation Explain(Query query, int doc)
    {
      return this.Explain(this.CreateWeight(query), doc);
    }

    /// <summary>
    /// Expert: Set the Similarity implementation used by this Searcher.
    /// 
    /// 
    /// </summary>
    /// <seealso cref="M:Lucene.Net.Search.Similarity.SetDefault(Lucene.Net.Search.Similarity)"/>
    public virtual void SetSimilarity(Similarity similarity)
    {
      this.similarity = similarity;
    }

    public virtual Similarity GetSimilarity()
    {
      return this.similarity;
    }

    /// <summary>
    /// creates a weight for
    /// <code>
    /// query
    /// </code>
    /// 
    /// </summary>
    /// 
    /// <returns>
    /// new weight
    /// 
    /// </returns>
    protected internal virtual Weight CreateWeight(Query query)
    {
      return query.Weight(this);
    }

    public virtual int[] DocFreqs(Term[] terms)
    {
      int[] numArray = new int[terms.Length];
      for (int index = 0; index < terms.Length; ++index)
        numArray[index] = this.DocFreq(terms[index]);
      return numArray;
    }

    public abstract void Search(Weight weight, Filter filter, HitCollector results);

    public abstract void Close();

    public abstract int DocFreq(Term term);

    public abstract int MaxDoc();

    public abstract TopDocs Search(Weight weight, Filter filter, int n);

    public abstract Document Doc(int i);

    public abstract Query Rewrite(Query query);

    public abstract Explanation Explain(Weight weight, int doc);

    public abstract TopFieldDocs Search(Weight weight, Filter filter, int n, Sort sort);
  }
}
