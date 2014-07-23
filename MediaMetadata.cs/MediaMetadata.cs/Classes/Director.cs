using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MediaMetadata.cs
{
    class Director
    {
        private String name;
        private String imdbPage;

        public Director(String Name, String imdbLink)
        {
            this.name = Name;
            this.imdbPage = imdbLink;
        }

        public String Name
        {
            get { return this.name; }
            set { this.name = value; }
        }

        public String IMDBPage
        {
            get { return this.imdbPage; }
            set { this.imdbPage = value; }
        }
    }
}
