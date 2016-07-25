using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml;

namespace DataBackupTool
{
    public class SettingConfig
    {
        public SettingConfig()
        {
        }
        public const string key_SourcePathList = "SourcePathList";
        public const string key_ConnectionString = "ConnectionString";
        public const string key_TargetPathList = "TargetPathList";
        public const string key_DataBaseBackupRate = "DataBaseBackupRate";
        public static string m_configFilePathName = "";

        public string value_SourcePathList
        {
            get { return GetConfigData(key_SourcePathList); }
            set { WriteConfigData(key_SourcePathList, value); }
        }
        public string value_ConnectionString
        {
            get { return GetConfigData(key_ConnectionString); }
            set { WriteConfigData(key_ConnectionString, value); }
        }

        public string value_DataBaseBackupRate
        {
            get { return GetConfigData(key_DataBaseBackupRate); }
            set { WriteConfigData(key_DataBaseBackupRate, value); }
        }
        public string value_TargetPathList
        {
            get { return GetConfigData(key_TargetPathList); }
            set { WriteConfigData(key_TargetPathList, value); }
        }
        private string GetConfigData(string key)
        {
            lock (m_configFilePathName)
            {
                if (File.Exists(m_configFilePathName))
                {
                    XmlDocument configFile = new XmlDocument();
                    configFile.Load(m_configFilePathName);
                    XmlNodeList nodes = configFile.GetElementsByTagName("add");
                    for (int index = 0; index != nodes.Count; index++)
                    {
                        if (nodes[index].Attributes["key"] != null)
                        {
                            if (nodes[index].Attributes["key"].Value == key)
                            {
                                return nodes[index].Attributes["value"].Value;
                            }
                        }

                    }
                }
                return "";
            }
        }
        private void WriteConfigData(string key, string value)
        {
            lock (m_configFilePathName)
            {
                if (File.Exists(m_configFilePathName))
                {
                    XmlDocument configFile = new XmlDocument();
                    configFile.Load(m_configFilePathName);
                    XmlNodeList nodes = configFile.GetElementsByTagName("add");
                    for (int index = 0; index != nodes.Count; index++)
                    {
                        if (nodes[index].Attributes["key"] != null)
                        {
                            if (nodes[index].Attributes["key"].Value == key)
                            {
                                nodes[index].Attributes["value"].Value = value;
                                break;
                            }
                        }
                    }
                    configFile.Save(m_configFilePathName);
                }
            }
        }
    }
}
