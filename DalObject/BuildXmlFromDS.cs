using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using System.Xml.Serialization;

namespace Dal
{
    public static class BuildXmlFromDS
    {
        #region CTOR:BuildXmlFromDataSource
        public static void BuildXmlFromDataSource()
        {
            string localPath;
            string str = Assembly.GetExecutingAssembly().Location;
            localPath = Path.GetDirectoryName(str); 
            localPath = Path.GetDirectoryName(localPath);
            localPath = Path.GetDirectoryName(localPath);

            localPath += @"\Data\";

            string dronePath = localPath + @"drone.xml";
            string droneChargePath = localPath + @"droneCharge.xml";
            string stationsPath = localPath + @"station.xml";
            string customerPath = localPath + @"customer.xml";
            string parcelPath = localPath + @"parcel.xml";
            string userPath = localPath + @"user.xml";
            string configPath = localPath + @"config.xml";


            SaveListToXMLSerializer(DataSource.StationList, stationsPath);
            SaveListToXMLSerializer(DataSource.ClientList, customerPath);
            SaveListToXMLSerializer(DataSource.ParcelList, parcelPath);
            //SaveListToXMLSerializer(DataSource.Users, userPath);
            creatXmls(DataSource.DroneChargesList, droneChargePath, "droneCharge");
            SaveListToXMLSerializer (DataSource.DroneChargeList, dronePath);
            //SaveListToXMLSerializer(DalObject.Instance.PowerConsumptionByDrone().ToList(), configPath);
        }
        #endregion

        #region CreateElement
        public static XElement CreateElement<T>(T obj)
        {
            var res = new XElement(typeof(T).Name);
            foreach (PropertyInfo prop in obj.GetType().GetProperties())
            {
                res.Add(new XElement(prop.Name, prop.GetValue(obj)));
            }
            return res;
        }

        #endregion

        #region creatXmls
        static void creatXmls<T>(List<T> list, string path, string name)
        {
            XElement root = new XElement(name);
            foreach (var item in list)
            {
                root.Add(CreateElement(item));
            }
            root.Save(path);
        }
        #endregion

        #region LoadListFromXMLElement
        public static XElement LoadListFromXMLElement(string filePath)
        {
            try
            {
                if (File.Exists(filePath))
                {
                    return XElement.Load(filePath);
                }
                else
                {
                    XElement rootElem = new XElement(filePath);
                    rootElem.Save(filePath);
                    return rootElem;
                }
            }
            catch (Exception ex)
            {
                throw new DO.XmlFileLoadException(filePath);
            }
        }
        #endregion

        #region LoadListFromXMLSerializer
        public static List<T> LoadListFromXMLSerializer<T>(string filePath)
        {
            try
            {
                if (File.Exists(filePath))
                {
                    List<T> list;
                    XmlSerializer x = new XmlSerializer(typeof(List<T>));
                    FileStream file = new FileStream(filePath, FileMode.Open);
                    list = (List<T>)x.Deserialize(file);
                    file.Close();
                    return list;
                }
                else
                    return new List<T>();
            }
            catch (Exception ex)
            {
                throw new DO.XmlFileLoadException(filePath);
            }
        }
        #endregion

        #region SaveListToXMLSerializer
        public static void SaveListToXMLSerializer<T>(List<T> list, string filePath)
        {
            try

            {
                FileStream file = new FileStream(filePath, FileMode.Create);
                XmlSerializer x = new XmlSerializer(list.GetType());
                x.Serialize(file, list);
                file.Close();
            }
            catch (Exception ex)
            {
                throw new DO.XmlFileCreateException(filePath);
            }
        }
        #endregion

        #region SaveListToXMLElement
        public static void SaveListToXMLElement(XElement rootElem, string filePath)
        {
            try
            {
                rootElem.Save(filePath);
            }
            catch (Exception ex)
            {
                throw new DO.XmlFileCreateException(filePath);
            }
        }
        #endregion
    }
}
