using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;

namespace MediaMetadata.cs
{
    class Movie
    {
        private String imdbID;
        private String title;
        private String imdbTitle;
        private int year;
        private int runtime;
        private String summary;
        private List<String> actors;
        private List<String> genres;
        private Director director;
        private String imdbPage;
        private String imageURL;
        private double rating;
        private int numRated;
        private String mpr;
        private String mprCause;
        private bool wasMatched;

        public Movie()
        {
            genres = new List<String>();
            actors = new List<String>();
        }

        public String Title
        {
            get { return this.title; }
            set { this.title = value; }
        }

        public String IMDBTitle
        {
            get { return this.imdbTitle; }
            set { this.imdbTitle = value; }
        }

        public String IMDBID
        {
            get { return this.imdbID; }
        }

        public String Summary
        {
            get { return this.summary; }
            set { this.summary = value; }
        }

        public String IMDBURL
        {
            get { return this.imdbPage; }
            set 
            { 
                this.imdbPage = value;
                Regex ID = new Regex("http://www.imdb.com/title/(?<id>[t0-9]*)/");
                Match m = ID.Match(value);
                imdbID = m.Groups["id"].Value;
            }
        }

        public String ImageURL
        {
            get { return this.imageURL; }
            set { this.imageURL = value; }
        }

        public int Year
        {
            get { return this.year; }
            set { this.year = value; }
        }

        public int Runtime
        {
            get { return this.runtime; }
            set { this.runtime = value; }
        }

        public List<String> Genres
        {
            get { return this.genres; }
            set { this.genres = value; }
        }

        public List<String> Actors
        {
            get { return this.actors; }
            set { this.actors = value; }
        }

        public String MPR
        {
            get { return this.mpr; }
            
            set 
            { 
                if(value.Equals("G") || value.Equals("PG") || value.Equals("PG-13") || value.Equals("R") || value.Equals("NA-17"))
                    this.mpr = value;
            }
        }

        public String MPRCause
        {
            get { return this.mprCause; }
            set { this.mprCause = value; }
        }

        public double Rating
        {
            get { return this.rating; }
            set
            {
                if(value >= 0.0 && value <=10.0)
                this.rating = value; 
            }
        }

        public int NumRated
        {
            get { return this.numRated; }
            set { this.numRated = value; }
        }

        public Director Director
        {
            get { return this.director; }
            set { this.director = value; }
        }

        public override string ToString()
        {
            String result = "";
            if (wasMatched)
            {
                result = String.Format("<?xml version=\"1.0\" encoding=\"UTF-8\"?>\r\n" +
                                              "<movie>\r\n" +
                                              "   <imdbTitle>\"{0}\"</imdbTitle>\r\n"+
                                              "   <title>\"{1}\"</title>\r\n" +
                                              "   <runtime>{2}</runtime>\r\n"+
                                              "   <director>\r\n"+
                                              "      <name>\"{3}\"</name>\r\n"+
                                              "      <page>\"{4}\"</page>\r\n"+
                                              "   </director>\r\n"+
                                              "   <summary>\"{5}\"</summary>\r\n" +
                                              "   <rating>{6}</rating>\r\n" +
                                              "   <numRated>{7}</numRated>\r\n"+
                                              "   <mpr>\"{8}\"</mpr>\r\n" +
                                              "   <mprCause>\"{9}\"</mprCause>\r\n" +
                                              "   <year>{10}</year>\r\n" +
                                              "   <imdbID>{11}</imdbID>\r\n" +
                                              "   <imdbURL>\"{12}\"</imdbURL>\r\n"+
                                              "   <genres>\r\n", 
                                              imdbTitle, title, runtime, director.Name, director.IMDBPage, summary, rating, numRated, mpr, mprCause, year, imdbID, imdbPage);
                                              // {0}-----{1------{2}--------------{3}-----------{4}---------{5}------{6}-------{7}----{8}-----{9}----{10}----{11}----{12}
                for (int i = 0; i < genres.Count(); i++)
                {
                    result += String.Format("      <genre>\r\n" +
                                            "         <name>\"{0}\"</name>\r\n" +
                                            "      </genre>\r\n", genres[i]);
                }

                result += "   </genres>\r\n"+
                          "   <actors>\r\n";
                for (int i = 0; i < actors.Count(); i++)
                {
                    String name = actors[i].Substring(0, actors[i].IndexOf(" as "));
                    String role = actors[i].Substring(actors[i].IndexOf(" as ") + 4);

                    result += String.Format("      <actor>\r\n" +
                                            "         <name>\"{0}\"</name>\r\n" +
                                            "         <role>\"{1}\"</role>\r\n" +
                                            "      </actor>\r\n", name, role);
                }
                result += "   </actors>\r\n</movie>";
            }

            else
                result = "Movie not matched. No data available";
            return result;
        }

        public bool WasMatched
        {
            get { return this.wasMatched; }
            set { this.wasMatched = value; }
        }
    }
}
