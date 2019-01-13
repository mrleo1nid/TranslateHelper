using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;

namespace TranslateHelper.Workers
{
    class DataWorker
    {
        /// <summary>
        /// Создаём xml файл
        /// </summary>
        private static void CreateModListFile()
        {
            XDocument xdoc = new XDocument();
            XElement Mods = new XElement("Mods");
            xdoc.Add(Mods);
            //сохраняем документ
            xdoc.Save("Mods.xml");
        }
        /// <summary>
        /// Загружаем лист модов из файла
        /// </summary>
        /// <returns>Лист модов</returns>
        public static ObservableCollection<Mod> LoadModList()
        {
            ObservableCollection<Mod> mods = new ObservableCollection<Mod>();
            FileInfo file = new FileInfo("Mods.xml");
            if (!file.Exists)
            {
                CreateModListFile();
                return mods;
            }
            mods = ReadModFile();
            return mods;
        }
        /// <summary>
        /// Читаем моды из файла
        /// </summary>
        /// <returns>Список модов</returns>
        private static ObservableCollection<Mod> ReadModFile()
        {
            FileInfo file = new FileInfo("Mods.xml");
            if (!file.Exists) { CreateModListFile(); }
            ObservableCollection<Mod> mods = new ObservableCollection<Mod>();
            XmlDocument xDoc = new XmlDocument();
            xDoc.Load("Mods.xml");
            // получим корневой элемент
            XmlElement xRoot = xDoc.DocumentElement;
            // обход всех узлов в корневом элементе
            foreach (XmlNode xnode in xRoot)
            {
                Mod mod = new Mod();
                XmlNode idAttribute = xnode.Attributes.GetNamedItem("id");
                XmlNode nameAttribute = xnode.Attributes.GetNamedItem("name");
                if (idAttribute!=null) {  mod.ID = Guid.Parse(idAttribute.InnerText);}
                if (nameAttribute!=null) { mod.Name = nameAttribute.InnerText; }
                foreach (XmlNode node in xnode.ChildNodes)
                {
                    switch (node.Name)
                    {
                        case "path":
                            mod.Path = node.InnerText;
                            break;
                        case "imagePath":
                            mod.ImagePath = node.InnerText;
                            break;
                        case "languagesPath":
                            mod.LanguagesPath = node.InnerText;
                            break;
                        case "aboutPath":
                            mod.AboutPath = node.InnerText;
                            break;
                    }
                }
                mod.Info = Worker.LoadModInfo(mod.AboutPath);
                mods.Add(mod);
            }

            return mods;
        }
        /// <summary>
        /// Добавляем мод в файл
        /// </summary>
        /// <param name="mod">Мод</param>
        public static void AddModToFile(Mod mod)
        {
            FileInfo file = new FileInfo("Mods.xml");
            if (!file.Exists) { CreateModListFile(); }
            XmlDocument xDoc = new XmlDocument();
            xDoc.Load("Mods.xml");
            XmlElement xRoot = xDoc.DocumentElement;
            XmlElement modElem = xDoc.CreateElement("mod");
            XmlAttribute nameAttr = xDoc.CreateAttribute("name");
            XmlAttribute idAttr = xDoc.CreateAttribute("id");
            XmlElement pathElem = xDoc.CreateElement("path");
            XmlElement imageElem = xDoc.CreateElement("imagePath");
            XmlElement languagesElem = xDoc.CreateElement("languagesPath");
            XmlElement aboutElem = xDoc.CreateElement("aboutPath");
            // создаем текстовые значения для элементов и атрибута
            XmlText nameText = xDoc.CreateTextNode(mod.Name);
            XmlText imageText = xDoc.CreateTextNode(mod.ImagePath);
            XmlText pathText = xDoc.CreateTextNode(mod.Path);
            XmlText languagesText = xDoc.CreateTextNode(mod.LanguagesPath);
            XmlText aboutText = xDoc.CreateTextNode(mod.AboutPath);
            XmlText idText = xDoc.CreateTextNode(mod.ID.ToString());


            //добавляем узлы
            nameAttr.AppendChild(nameText);
            pathElem.AppendChild(pathText);
            imageElem.AppendChild(imageText);
            languagesElem.AppendChild(languagesText);
            aboutElem.AppendChild(aboutText);
            idAttr.AppendChild(idText);

            modElem.Attributes.Append(nameAttr);
            modElem.Attributes.Append(idAttr);
            modElem.AppendChild(pathElem);
            modElem.AppendChild(imageElem);
            modElem.AppendChild(languagesElem);
            modElem.AppendChild(aboutElem);
            xRoot.AppendChild(modElem);
            xDoc.Save("Mods.xml");
        }
        /// <summary>
        /// Удаляем мод из файла
        /// </summary>
        /// <param name="mod">Мод</param>
        public static void RemoveModToFile(Mod mod)
        {
            FileInfo file = new FileInfo("Mods.xml");
            if (!file.Exists) { CreateModListFile(); }
            XmlDocument xDoc = new XmlDocument();
            xDoc.Load("Mods.xml");
            XmlElement xRoot = xDoc.DocumentElement;
            XmlNode childnode = xRoot.SelectSingleNode($"mod[@id='{mod.ID}']");
            if (childnode != null) xRoot.RemoveChild(childnode);
            xDoc.Save("Mods.xml");
        }
        /// <summary>
        /// Очищаем файл с модами
        /// </summary>
        public static void ClearModFile()
        {
            FileInfo file = new FileInfo("Mods.xml");
            if (!file.Exists)
            {
                CreateModListFile();
                return;
            }
            XmlDocument xDoc = new XmlDocument();
            xDoc.Load("Mods.xml");
            XmlElement xRoot = xDoc.DocumentElement;
            xRoot.RemoveAll();
            xDoc.Save("Mods.xml");
        }
    }
}
