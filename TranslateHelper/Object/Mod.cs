using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using TranslateHelper.Object;

namespace TranslateHelper.Workers
{
   public class Mod : INotifyPropertyChanged
   {
       private Guid id;
        private string name;
        private string path;
        private string imagePath;
        private string languagesPath;
        private string aboutPath;
        private Info info;

        public string Name
        {
            get { return name; }
            set
            {
                name = value;
                OnPropertyChanged("Name");
            }
        }
       public Guid ID
       {
           get { return id; }
           set
           {
               id = value;
               OnPropertyChanged("ID");
           }
       }
        public string Path
        {
            get { return path; }
            set
            {
                path = value;
                OnPropertyChanged("Path");
            }
        }
        public string ImagePath
        {
            get { return imagePath; }
            set
            {
                imagePath = value;
                OnPropertyChanged("ImagePath");
            }
        }
        public string LanguagesPath
        {
            get { return languagesPath; }
            set
            {
                languagesPath = value;
                OnPropertyChanged("LanguagesPath");
            }
        }
        public string AboutPath
        {
            get { return aboutPath; }
            set
            {
                aboutPath = value;
                OnPropertyChanged("AboutPath");
            }
        }
        public Info Info
        {
            get { return info; }
            set
            {
                info = value;
                OnPropertyChanged("Info");
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName]string prop = "")
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
        }
    
}
}
