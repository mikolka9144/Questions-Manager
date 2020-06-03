using System;
using System.Collections.Generic;
using System.IO;
using System.Xml.Serialization;

namespace Points_submit
{
    public class QuestionNamer
    {
        private XmlSerializer xml;
        public List<(string guid, string val)> list;
        private readonly string Path = AppDomain.CurrentDomain.BaseDirectory + "_Data\\questionsNames.dat";
        public QuestionNamer()
        {
            xml = new XmlSerializer(typeof(List<(string guid,string val)>));
            if (File.Exists(Path))
            {
                list = (List<(string guid, string val)>)xml.Deserialize(File.OpenRead(Path));
            }
            else
            {
                list = new List<(string guid, string val)>();
            }
        }

        public void Save()
        {
            File.Delete(Path);
            xml.Serialize(File.OpenWrite(Path), list);
        }
    }
}