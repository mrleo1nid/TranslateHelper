using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Xml;
using TranslateHelper.Object;
using TranslateHelper.ViewsModels;
using Forms = System.Windows.Forms;

namespace TranslateHelper.Workers
{
   public class Worker
    {
        /// <summary>
        /// Выбор мода
        /// </summary>
        /// <returns>Мод</returns>
        public static Mod SelectMod()
        {
            Mod mod = new Mod();
            Forms.FolderBrowserDialog dial = new Forms.FolderBrowserDialog();
            if (dial.ShowDialog() == Forms.DialogResult.OK)
            {
                var path = dial.SelectedPath;
                DirectoryInfo info = new DirectoryInfo(path);
                mod.Name = info.Name;
                mod.ID = Guid.NewGuid();
                mod.Path = info.FullName;
                mod.ImagePath = mod.Path + @"\About\Preview.png";
                mod.AboutPath = mod.Path + @"\About\About.xml";
                mod.LanguagesPath = mod.Path + @"\Languages";
                mod.Info = LoadModInfo(mod.AboutPath);
                return mod;
            }
            else
            {
                return null;
            }
        }
        /// <summary>
        /// Получаем все моды из указанной дирректории
        /// </summary>
        /// <returns>Список модов</returns>
        public static List<Mod> SelectModInGameFolder()
        {
            List<Mod> list = new List<Mod>();
            Forms.FolderBrowserDialog dial = new Forms.FolderBrowserDialog();
            if (dial.ShowDialog() == Forms.DialogResult.OK)
            {
                var path = dial.SelectedPath;
                DirectoryInfo info = new DirectoryInfo(path);
                if (IsGameDirrectory(info))
                {
                    list.AddRange(CreateModList(info.FullName));
                    return list;
                }
                else
                {
                    MessageBox.Show("Выберите дирректорию RimWorld");
                    SelectModInGameFolder();
                    return null;
                }      
            }
            else
            {
                return null;
            }
        }
        /// <summary>
        /// Получаем инфу по моду
        /// </summary>
        /// <param name="path">Путь к файлу About.xml</param>
        /// <returns></returns>
        public static Info LoadModInfo(string path)
        {
            Info info = new Info();
            FileInfo fileinfo = new FileInfo(path);
            if (fileinfo.Exists)
            {
                XmlDocument xDoc = new XmlDocument();
                xDoc.Load(path);
                // получим корневой элемент
                XmlElement xRoot = xDoc.DocumentElement;
                // обход всех узлов в корневом элементе
                foreach (XmlNode xnode in xRoot)
                {
                    switch (xnode.Name)
                    {
                        case "name":
                            info.Name = xnode.InnerText;
                            break;
                        case "author":
                            info.Author = xnode.InnerText;
                            break;
                        case "targetVersion":
                            info.TargetVersion = xnode.InnerText;
                            break;
                        case "url":
                            info.Url = xnode.InnerText;
                            break;
                        case "description":
                            info.Description = xnode.InnerText;
                            break;
                    }
                }
            }

            return info;
        }
        /// <summary>
        /// Является ли дирректория игровой
        /// </summary>
        /// <param name="info"></param>
        /// <returns></returns>
        public static bool IsGameDirrectory(DirectoryInfo info)
        {
            string exe1 = info.FullName + @"\RimWorldWin64.exe";
            FileInfo Fileexe1 = new FileInfo(exe1);
            string exe2 = info.FullName + @"\RimWorldWin32.exe";
            FileInfo Fileexe2 = new FileInfo(exe2);
            string mods = info.FullName + @"\mods";
            DirectoryInfo dirmods = new DirectoryInfo(mods);
            string Mods = info.FullName + @"\Mods";
            DirectoryInfo dirMods = new DirectoryInfo(Mods);
            if (Fileexe1.Exists || Fileexe2.Exists)
            {
                if (dirmods.Exists || dirMods.Exists)
                {
                    return true;
                }
            }
            return false;
        }
        /// <summary>
        /// Формирует список модов
        /// </summary>
        /// <param name="GamePath">Путь к папки игры</param>
        /// <returns></returns>
        public static List<Mod> CreateModList(string GamePath)
        {
            List<Mod> listmod = new List<Mod>();
            string mods = GamePath + @"\mods";
            DirectoryInfo dirmods = new DirectoryInfo(mods);
            string Mods = GamePath + @"\Mods";
            DirectoryInfo dirMods = new DirectoryInfo(Mods);
            DirectoryInfo Dir = new DirectoryInfo(GamePath);
            if (dirmods.Exists)
            {
                Dir = dirmods;
            }
            else if (dirMods.Exists)
            {
                Dir = dirMods;
            }
            var nameList = Dir.GetDirectories();
            foreach (var name in nameList)
            {
                Mod mod = new Mod();
                mod.Name = name.Name;
                mod.ID = Guid.NewGuid();
                mod.Path = name.FullName;
                mod.ImagePath = mod.Path + @"\About\Preview.png";
                mod.AboutPath = mod.Path + @"\About\About.xml";
                mod.LanguagesPath = mod.Path + @"\Languages";
                mod.Info = LoadModInfo(mod.AboutPath);
                listmod.Add(mod);
            }
            return listmod;
        }
    }
}
