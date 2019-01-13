using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using TranslateHelper.Workers;

namespace TranslateHelper.ViewsModels
{
    public class MainViewModel : INotifyPropertyChanged
    {
     

        public ObservableCollection<Mod> Modes { get; set; }

      
        private Mod selectedMod;
        public Mod SelectedMod
        {
            get { return selectedMod; }
            set
            {
                selectedMod = value;
                OnPropertyChanged("SelectedMod");
            }
        }

        #region Commands
        // команда добавления нового объекта
        private RelayCommand addCommand;
        public RelayCommand AddCommand
        {
            get
            {
                return addCommand ??
                       (addCommand = new RelayCommand(obj =>
                       {
                           Mod mod = Worker.SelectMod();
                           if (mod != null)
                           {
                               Modes.Insert(0, mod);
                               DataWorker.AddModToFile(mod);
                               selectedMod = mod;
                           }
                       }));
            }
        }
        private RelayCommand removeCommand;
        public RelayCommand RemoveCommand
        {
            get
            {
                return removeCommand ??
                       (removeCommand = new RelayCommand(obj =>
                       {
                           Mod mod = selectedMod;
                           if (mod != null)
                           {
                               Modes.Remove(mod);
                               DataWorker.RemoveModToFile(mod);
                               selectedMod = Modes.FirstOrDefault();
                           }
                       }));
            }
        }
        private RelayCommand addFolderCommand;
        public RelayCommand AddFolderCommand
        {
            get
            {
                return addFolderCommand ??
                       (addFolderCommand = new RelayCommand(obj =>
                       {
                         List<Mod> mods = Worker.SelectModInGameFolder();
                           if (mods != null)
                           {
                               foreach (var mod in mods)
                               {
                                   Modes.Add(mod);
                                   DataWorker.AddModToFile(mod);
                               }
                               selectedMod = Modes.FirstOrDefault();
                           }
                       }));
            }
        }
        private RelayCommand clearCommand;
        public RelayCommand ClearCommand
        {
            get
            {
                return clearCommand ??
                       (clearCommand = new RelayCommand(obj =>
                       {
                           Modes.Clear();
                           DataWorker.ClearModFile();
                       }));
            }
        }
        #endregion
        public MainViewModel()
        {
            Modes = DataWorker.LoadModList();
        }

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName]string prop = "")
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
        }
    }
}
