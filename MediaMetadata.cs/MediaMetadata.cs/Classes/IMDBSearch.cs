using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Net;
using System.Text.RegularExpressions;

namespace MediaMetadata.cs
{
    class IMDBSearch
    {
        private String baseUrl;

        /// <summary>
        /// Fetches the data for the given movie from IMDb's website using a scraper.
        /// </summary>
        /// <param name="movieTitle">The title of the movie to find.</param>
        /// <param name="movieYear">The year the movie was released.</param>
        /// <returns>A Movie object containing the found data.</returns>
        public Movie getData(String movieTitle, int movieYear)
        {
            baseUrl = "http://www.imdb.com/";
            Movie result = getMovieInfo(movieTitle, movieYear);

            return result;
        }

        private Movie getMovieInfo(String movieName, int movieYear)
        {
            String url = baseUrl + "search/title?title_type=tv_movie,feature,documentry&title="+movieName;

            if (movieYear != 0)
                url += String.Format("&release_date={0},{0}", movieYear);

            //read the webpage and parse out the URL for the Movie
            WebClient client = new WebClient();
            String HTMLPage = client.DownloadString(url);


            
            //jump to the "main" section <table class="FindList">
            int newStart = HTMLPage.IndexOf("<table class=\"results\">");
            String narrow = HTMLPage.Substring(newStart+23);
            String Line = narrow.Substring(0,narrow.IndexOf("</table>"));
          
            //split by '</a>'
            Regex split = new Regex(@"\</tr\>");
            String[] elements = split.Split(Line);
            
            Movie m = matching(elements, movieName, movieYear);
            

            return m;
        }

        private Movie matching(String[] elements, String movieName, int year)
        {
            Movie result = new Movie();
            result.Title = movieName;
            result.WasMatched = false;

            //split the element apart
            Regex getMovieData = new Regex("\\<a href=\"/(?<link>.*)\"\\>(?<name>.*)\\</a\\>\\n +\\<span class=\"year_type\"\\>\\((?<year>[0-9]{4})");

            //loop through all the elements
            for (int i = 0; i < elements.Length; i++)
            {
                String element = elements[i];
                Match m = getMovieData.Match(element);

                //if the element doesn't match the regex
                if (!getMovieData.IsMatch(element))
                    continue;   //skip it
 
                String foundTitle = m.Groups["name"].Value.Replace("&#x27;", "\'");
                int foundYear = Int32.Parse(m.Groups["year"].Value);

                //if the elemt's name matches the movie name
                if (isMatch(foundTitle, movieName) && !elements[i].Contains("(TV Episode)") && !elements[i].Contains("TV Series)") && (year == foundYear || year == 0))
                {
                    //record the immidate data
                    result.Title = movieName;
                    result.IMDBTitle = foundTitle;
                    result.IMDBURL = baseUrl + m.Groups["link"].Value;
                    result.Year = foundYear;

                    //get the webepage text
                    WebClient client = new WebClient();
                    String HTMLPage = client.DownloadString(result.IMDBURL);

                    //collect data from IMDb
                    result.Summary = getsummary(HTMLPage);
                    result.Genres = getGeneres(HTMLPage);
                    result.Actors = getActors(HTMLPage);
                    result = getMPR(result, HTMLPage);
                    result = getRating(result, HTMLPage);
                    result.ImageURL = getImageURL(HTMLPage);
                    result.Director = getDirector(HTMLPage);
                    result.Runtime = getRunTime(HTMLPage);

                    //take note of the match and exit loop.
                    result.WasMatched = true;
                    break;
                }
            }
            //return
            return result;
        }

        private bool isMatch(String IMDBName, String movieName)
        {
            IMDBName = removeDupSpaces(removePunc(IMDBName)).ToLower();
            movieName = removeDupSpaces(removePunc(movieName)).ToLower();

            if (IMDBName.Contains(" three"))
                IMDBName = IMDBName.Replace(" three ", " 3");

            if (movieName.Contains(" three "))
                movieName = movieName.Replace(" three", " 3" );

            if(IMDBName.Contains("&#x27;"))
                IMDBName = IMDBName.Replace("&#x27;", "\'");

            if (IMDBName.Contains("&#x26;"))
                IMDBName = IMDBName.Replace("&#x26;", "&");

            if (IMDBName.Contains(" and "))
                IMDBName = IMDBName.Replace(" and ", " & ");

            if (movieName.Contains(" and "))
                movieName = movieName.Replace(" and ", " & ");

            if (IMDBName.Contains(" 3d "))
                IMDBName = IMDBName.Replace(" 3d ", " ");

            if (movieName.Contains(" 3d "))
                movieName = movieName.Replace(" 3d ", " ");

            return IMDBName.Equals(movieName);
        }

        private String removePunc(String movieName)
        {
            Char[] chars = movieName.ToArray();
            String result = "";

            foreach (char c in chars)
            {
                if (c == 'Â' || c == '-' || c == '/')
                    result += ' ';

                else if (c == 'é')
                    result += 'e';

                else if (c != '\'' && c != ',' && c != '\"' && c!= '.' && c !=':')
                    result += c;
            }
            return result;
        }

        private String removeDupSpaces(String str)
        {
            if (String.IsNullOrEmpty(str))
                return str;

            String result = "";
            for (int i = 1; i < str.Count(); i++)
            {
                char prev = str.ElementAt(i - 1);
                char curr = str.ElementAt(i);

                if (prev == ' ' && curr == ' ')
                {
                    result += " ";
                    i += 2;
                }
                else
                    result += prev;
            }
            result += str.Last();

            return result;
        }

        private String[] getMovieElements(String[] elements)
        {
            List<String> result = new List<String>();

            for (int i = 0; i < elements.Length; i++)
            {
                String element = elements[i];

                if (!string.IsNullOrEmpty(element))
                {
                    Regex elementSplit = new Regex(@"\</td\>");
                    result.Add(elementSplit.Split(element)[1]);
                }
            }
            return result.ToArray();
        }

        private String processMovieName(string movieName)
        {
            String result = "";

            Char[] chars = movieName.ToLower().ToArray();
            foreach(char c in chars)
            {
                if (c == ' ')
                    result += '+';

                else if (!Char.IsPunctuation(c))
                    result += c;
            }

            return result;
        }

        private String getsummary(String HTMLPage)
        {

            int start = HTMLPage.IndexOf("<h2>Storyline</h2>");
            String part = HTMLPage.Substring(start);
            start = part.IndexOf("<p>");

            if (start == -1)
                return "summary couldn't be found";
            
            part = part.Substring(start);

            Regex storyline = new Regex("\\<p\\>\n(?<storyline>.*)\\<");
            Match m = storyline.Match(part);
            String result = m.Groups["storyline"].Value.Trim();

            return result;
        }

        private List<String> getGeneres(String HTMLPage)
        {
            int start = HTMLPage.IndexOf("<h4 class=\"inline\">Genres:</h4>");
            List<String> result = new List<String>();
            if (start == -1)
            {
                result.Add("Unknown");
                return result;
            }

            String part = HTMLPage.Substring(start);
            int end = part.IndexOf("</div>");
            part = part.Substring(0,end);
            Regex storyline = new Regex("\\> (?<genre>.*)\\</a>");

            String[] genreLines = part.Split('\n');

            for (int i = 1; i < genreLines.Length; i++ )
            {
                String str = genreLines[i];
                Match m = storyline.Match(str);

                String genre = m.Groups["genre"].Value.Trim();

                if(!String.IsNullOrEmpty(genre))
                    result.Add(genre);
            }

            return result;
        }

        private List<String> getActors(String HTMLPage)
        {
            int start = HTMLPage.IndexOf("<h2>Cast</h2>");
            List<String> result = new List<String>();

            if (start == -1)
                return result;
            
            String part = HTMLPage.Substring(start);
            int end = part.IndexOf("</table>");
            part = part.Substring(0, end);

            Regex actor = new Regex("<span class=\"itemprop\" itemprop=\"name\"\\>(?<actor>.*)\\</span\\>");
            Regex role = new Regex("\\<a href=\"/character/.*\" \\>(?<role>.*)\\</a\\>");
            Regex roleBackup = new Regex("\\<div\\>\n(?<role>.*)\n");

            Regex split = new Regex("\\</tr\\>\n");
            String[] genreLines = split.Split(part);

            for (int i = 1; i < genreLines.Length; i++)
            {
                String str = genreLines[i];
                Match m1 = actor.Match(str);
                Match m2 = role.Match(str);
                
                if(!role.IsMatch(str))
                    m2 = roleBackup.Match(str);

                String act = String.Format("{0} as {1}", m1.Groups["actor"], m2.Groups["role"].Value.Trim());

                if (!String.IsNullOrEmpty(act) && !act.Equals(" as "))
                    result.Add(act);
            }

            return result;
        }

        private Movie getMPR(Movie movie, String HTMLPage)
        {
            int start = HTMLPage.IndexOf("<span itemprop=\"contentRating\">");
            
            if (start == -1)
            {
                movie.MPR = "PG-13";
                movie.MPRCause = "Couldn't load MPR or cause. Defaulted to PG-13";
                return movie;
            }

            String part = HTMLPage.Substring(start);
            int end = part.IndexOf("</span>");
            part = part.Substring(0,end+8);

            Regex mpr = new Regex("\\>Rated (?<rating>[PG137R\\-]+) (?<ratingCause>.*)</span>");
            Match m = mpr.Match(part);

            movie.MPR = m.Groups["rating"].Value.Trim();
            movie.MPRCause = m.Groups["ratingCause"].Value.Trim().Replace("for ","");
            return movie;
        }

        private Movie getRating(Movie movie, String HTMLPage)
        {
            int start = HTMLPage.IndexOf("<div class=\"titlePageSprite star-box-giga-star\">");

            if (start == -1)
            {
                movie.Rating = 6.0;
                movie.NumRated = 0;
                return movie;
            }
            String part = HTMLPage.Substring(start+48);

            start = part.IndexOf("title=\"Users rated this ")+24;
            part = part.Substring(start);

            int end = part.IndexOf(" votes) - click stars to rate\" >");

            if (end == -1)
            {
                movie.Rating = 6.0;
                movie.NumRated = 0;
                return movie;
            }

            part = part.Substring(0, end);
            part = part + "";

            Regex ratings = new Regex("(?<rating>[0-9\\.]+)/[0-9]{2} \\((?<numRated>[0-9\\,]+)");
            Match m = ratings.Match(part);

            movie.Rating = Double.Parse(m.Groups["rating"].Value);
            movie.NumRated = Int32.Parse(m.Groups["numRated"].Value.Replace(",",""));
            return movie;
        }

        private String getImageURL(String HTMLPage)
        {
            int start = HTMLPage.IndexOf("<link rel=\'image_src\' href=\"");
            String part = HTMLPage.Substring(start +28); 
            int end = part.IndexOf("\">");
            part = part.Substring(0, end).Trim();

            return part;
        }

        private Director getDirector(String HTMLPage)
        {
            int start = HTMLPage.IndexOf("<h4 class=\"inline\">Director:</h4>");

            if (start == -1)
                return new Director("N/A","N/A");

            String part = HTMLPage.Substring(start);
            int end = part.IndexOf("</div>");
            part = part.Substring(0, end);

            Regex storyline = new Regex("\\<a href=\"(?<link>.*)\" itemprop=\'url\'\\>\\<span class=\"itemprop\" itemprop=\"name\"\\>(?<name>.*)\\</span\\>");
            Match m = storyline.Match(part);

            String name = m.Groups["name"].Value;
            String link = baseUrl + m.Groups["link"].Value;

            return new Director(name, link);
        }

        private int getRunTime(String HTMLPage)
        {
            int start = HTMLPage.IndexOf("<time itemprop=\"duration\"");

            if (start == -1)
                return 0;

            String part = HTMLPage.Substring(start);
            int end = part.IndexOf("</time>");
            part = part.Substring(0, end);

            Regex time = new Regex("\\<time itemprop=\"duration\" datetime=\".*\" \\>\\s*(?<time>[0-9]+) min");
            Match m = time.Match(part);

            return Int32.Parse(m.Groups["time"].Value);
        }
    }
}
