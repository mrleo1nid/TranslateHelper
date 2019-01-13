using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TranslateHelper.Object
{
    public class Info 
    {
        private string name;
        private string author;
        private string targetVersion;
        private string url;
        private string description;

        public string Name
        {
            get { return name; }
            set
            {
                name = value;
            }
        }
        public string Author
        {
            get { return author; }
            set
            {
                author = value;
            }
        }
        public string TargetVersion
        {
            get { return targetVersion; }
            set
            {
                targetVersion = value;
            }
        }
        public string Url
        {
            get { return url; }
            set
            {
                url = value;
            }
        }
        public string Description
        {
            get { return description; }
            set
            {
                description = value;
            }
        }
    }
}
