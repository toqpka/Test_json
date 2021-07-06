using System;
using System.Text.Json;
using System.IO;

namespace Test_json
{
    //Structure for Props
    public class Attr
    {
        public string Name { get; set; }
        public string Type { get; set; }
        public string Value { get; set; }
    }
    //Structure for Element
    public class ProductOccurence
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public Attr[] Props { get; set; }
    }
    class Program
    {
        static void Main(string[] args)
        {
            //Опция для отображения json c новыми строками и пробелами
            var options = new JsonSerializerOptions
            {
                WriteIndented = true,
            };
            string Final_out = "";
            int i = 0;
            string XML_file_path_in = @"c:\test\test.xml";
            string ISON_file__path_out = @"c:\test\json.json";
            if (!(File.Exists(XML_file_path_in)))
            {
                Console.WriteLine("File does not exist.");
                return;
            }
            //Load xml file
            System.Xml.XmlDocument xDoc = new System.Xml.XmlDocument();
            xDoc.Load(XML_file_path_in);
            //Check for ProductOccurence in file
            System.Xml.XmlNodeList Product_list = xDoc.GetElementsByTagName("ProductOccurence");
            if (xDoc.HasChildNodes)
            {
                //Loop for all ProductOccurence in xml
                while (i < Product_list.Count)
                {
                    System.Xml.XmlNode ProductOccur = Product_list[i];
                    System.Xml.XmlElement ProductOccur_Element = (System.Xml.XmlElement)ProductOccur;
                    System.Xml.XmlAttribute elem_id_num = ProductOccur_Element.GetAttributeNode("Id");
                    System.Xml.XmlAttribute elem_name = ProductOccur_Element.GetAttributeNode("Name");
                    //Search for Id and Name in ProductOccurence
                    int mass_size = 0; // Array size for structure Props
                    if (ProductOccur.HasChildNodes)
                    {
                        for (int k = 0; k < ProductOccur.ChildNodes.Count; k++)
                        {
                            System.Xml.XmlNode Attrib = ProductOccur.ChildNodes[k];
                            if (Attrib.Name == "Attributes")
                            {
                                System.Xml.XmlElement Attrib_elem = (System.Xml.XmlElement)Attrib;

                                if (Attrib_elem.HasChildNodes)
                                {
                                    mass_size = Attrib_elem.ChildNodes.Count;
                                }
                            }
                        }
                    }

                    //Create new element for JSON
                    var ProdOc = new ProductOccurence
                    {
                        Id = elem_id_num.InnerText,
                        Name = elem_name.InnerText,
                        Props = new Attr[mass_size]
                    };
                    // If Props exist
                    if (ProductOccur.HasChildNodes)
                    {
                        for (int k = 0; k < ProductOccur.ChildNodes.Count; k++)
                        {
                            System.Xml.XmlNode Attrib = ProductOccur.ChildNodes[k];
                            if (Attrib.Name == "Attributes")
                            {
                                System.Xml.XmlElement Attrib_elem = (System.Xml.XmlElement)Attrib;

                                if (Attrib_elem.HasChildNodes)
                                {
                                    for (int x = 0; x < Attrib_elem.ChildNodes.Count; x++)
                                    {
                                        // Add new Prop to array Props
                                        System.Xml.XmlNode Attr = Attrib_elem.ChildNodes[x];
                                        if (Attr.Name == "Attr")
                                        {
                                            System.Xml.XmlElement Attr_name = (System.Xml.XmlElement)Attr;
                                            System.Xml.XmlAttribute Attr_name_check = Attr_name.GetAttributeNode("Name");
                                            System.Xml.XmlAttribute Attr_type_check = Attr_name.GetAttributeNode("Type");
                                            System.Xml.XmlAttribute Attr_value_check = Attr_name.GetAttributeNode("Value");
                                            var Props_attr = new Attr
                                            {
                                                Name = Attr_name_check.InnerText,
                                                Type = Attr_type_check.InnerText,
                                                Value = Attr_value_check.InnerText
                                            };
                                            ProdOc.Props[x] = Props_attr;
                                        }
                                    }
                                }
                            }
                        }
                    }
                    //Add [ and new line at start of Final_out string
                    if (i == 0)
                        Final_out = String.Concat(Final_out, "[\n");
                    string jsonString = JsonSerializer.Serialize(ProdOc, options);
                    if (i > 0)
                        Final_out = String.Concat(Final_out, ",\n"); //Add "," and new line between elements for JSON
                    Final_out = String.Concat(Final_out, jsonString);
                    i++;
                }
            }
            if (i != 0)
            { 
                Final_out = String.Concat(Final_out, "\n]"); //Add new line and ] at end of Final_out string
                File.WriteAllText(ISON_file__path_out, Final_out); // Write final file;
            }
        }
    }
}
