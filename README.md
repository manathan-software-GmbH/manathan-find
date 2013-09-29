manathan-find
=============

A simple, extendable search engine based on lucene.net

** usage

first run manathan.indexer on the location you want to index. Specify this in the config file like so: 

      <pages>
        <page url="c:\logs" crawler="FileCrawler">
            <rules>
              <item type="manathan.file.crawler.Rules.SeperatedLineRule, manathan.file.crawler">
                  <settings>
                      <item key="Seperator" value="	" />
                      <item key="Headers" value="Protocol,unkn1,unkn2,timestamp,ip,message" />
                      <item key="HasHeader" value="false" />
                      <item key="FieldMapping" value="Title=Protocol;Content=message" />
                  </settings>
              </item>
          </rules>
        </page>
      </pages>

This statement indexes all files in 'c:\logs' and expect them to be tab-seperated.
      
manathan.find comes with two crawlers - FileCrawler searches the file system, the WebCrawler crawls websites.
      
The SeperatedLineRule is a basic rule that comes with manathan.find which searches text-files like csv files.

Once the indexer is done, startup either manathanFind (a web ui) or manathan.find.console and insert any query.

Done :)

** create your own crawler
A crawler has to inherit ICrawler, and must be registered in the indexer. You should use the configuration section to get the pages you require, and specify a name for the crawler as well

A simple crawler may then look like this:

	namespace manathan.crawler
	{
	    using System;
	    using System.IO;
	    using System.Linq;
	    using System.Threading.Tasks;
	    using Rules;
	    using find;
	    using find.Configuration;
	    using find.Crawler;

	    public class MyCrawler : ICrawler
	    {
		static IndexedPages _searchConfig;

		public MyCrawler()
		{
		    _searchConfig = IndexedPages.GetConfigSettings();
		}

		public virtual string CrawlerName
		{
		    get { return "MyCrawler"; }
		}

		public void Crawl()
		{
		    _searchConfig.GetPagesForCrawler(CrawlerName).ForEach(CrawlDirectory);
		}

		void CrawlDirectory(Page page)
		{
		    // let the magic happen
		}
	    }
	}

The crawler has to create a document, a class that inherits from BaseDocument and contains a single method - string ToSearchable()
This should never return null or an empty string. 

If you want to make your search results better, your crawler can set the Title and Meta-Information for the Document as well
	
Once you added some magic, register the crawler in your indexer and create a page that should be crawled:

	  <search>
	    <index workerstorage="C:\manathan-index\worker"
	           releasestorage="C:\manathan-index\release">
	      <crawler>
	        <crawlers>
	          <item name="MyCrawler" type="manathan.crawler.MyCrawler, manathan.crawler"/>
	        </crawlers>
	      </crawler>
	      <pages>
	        <page url="someurl" crawler="MyCrawler"></page>
	      </pages>
	    </index>
	  </search>

** create your own rules

While it's nice having a specialized crawler for every page, you might want to be generic. This can be done with rules.
A rule does not have to inherit anything, and each crawler is responsible by itself to load them. If you add a rule to a page your crawler does not know of it will ignore the rule.

There are no guidelines to writing a rule, though it is considered good practice to make sure that the document does not has to know anything about the rules it depended on.
