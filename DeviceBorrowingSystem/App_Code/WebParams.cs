using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Xml;

/// <summary>
/// WebParams 的摘要说明
/// </summary>
public class WebParams
{
	public WebParams()
	{
		//
		// TODO: 在此处添加构造函数逻辑
		//
	}
    public string xmlDocPathName { get; set; }

    public string DeviceWarnDays { get; set; }

    public string GetNodeValue(string parentNode, string nodeName)
    {
        XmlDocument xmlApp = new XmlDocument();
        try
        {
            xmlApp.Load(xmlDocPathName);
            XmlNode rootNode = xmlApp.FirstChild.NextSibling;
            XmlNodeList nodeList = rootNode.ChildNodes;
            foreach (XmlNode node in nodeList)
            {
                if (node.Name.CompareTo(parentNode) == 0)
                {
                    foreach (XmlNode o in node.ChildNodes)
                    {
                        if(o.Name.CompareTo(nodeName) == 0)
                            return o.InnerText;
                    }
                }
            }

            return String.Empty;
        }
        catch
        {
            return String.Empty;
        }
    }
    public XmlNodeList GetNodeValueList(string parentNode, string nodeName)
    {
        XmlDocument xmlApp = new XmlDocument();
        try
        {
            xmlApp.Load(xmlDocPathName);
            XmlNode rootNode = xmlApp.FirstChild.NextSibling;
            XmlNodeList nodeList = rootNode.ChildNodes;
            foreach (XmlNode node in nodeList)
            {
                if (node.Name.CompareTo(parentNode) == 0)
                {
                    return node.ChildNodes;
                }
            }
            return null;
        }
        catch
        {
            return null;
        }
    }
    public string SetNodeValue(string parentNode, string nodeName, string value)
    {
        XmlDocument xmlApp = new XmlDocument();
        try
        {
            xmlApp.Load(xmlDocPathName);
            XmlNode rootNode = xmlApp.FirstChild.NextSibling;
            XmlNodeList nodeList = rootNode.ChildNodes;
            foreach (XmlNode node in nodeList)
            {
                if (node.Name.CompareTo(parentNode) == 0)
                {
                    foreach (XmlNode o in node.ChildNodes)
                    {
                        if (o.Name.CompareTo(nodeName) == 0)
                        {
                            o.InnerText = value;
                            xmlApp.Save(xmlDocPathName);
                            break;
                        }
                    }
                }
            }
            return String.Empty;
        }
        catch
        {
            return String.Empty;
        }
    }
    public bool DeleteNode(string parentNode, string nodeName, string value)
    {
        XmlDocument xmlApp = new XmlDocument();
        try
        {
            xmlApp.Load(xmlDocPathName);
            XmlNode rootNode = xmlApp.FirstChild.NextSibling;
            XmlNodeList nodeList = rootNode.ChildNodes;
            foreach (XmlNode node in nodeList)
            {
                if (node.Name.CompareTo(parentNode) == 0)
                {
                    foreach (XmlNode o in node.ChildNodes)
                    {
                        if (o.Name.CompareTo(nodeName) == 0 && o.InnerText.CompareTo(value) == 0)
                        {
                            o.ParentNode.RemoveChild(o);
                            xmlApp.Save(xmlDocPathName);
                        }
                    }
                }
            }
            return true;
        }
        catch
        {
            return false;
        }
    }
    public bool InsertNode(string parentNode, string nodeName, string value)
    {
        XmlDocument xmlApp = new XmlDocument();
        try
        {
            xmlApp.Load(xmlDocPathName);
            XmlNode rootNode = xmlApp.FirstChild.NextSibling;
            XmlNodeList nodeList = rootNode.ChildNodes;
            foreach (XmlNode node in nodeList)
            {
                if (node.Name.CompareTo(parentNode) == 0)
                {
                    XmlElement newChildNode = xmlApp.CreateElement(nodeName);
                    newChildNode.InnerText = value;
                    node.AppendChild(newChildNode);
                    xmlApp.Save(xmlDocPathName);
                    break;
                }
            }
            return true;
        }
        catch
        {
            return false;
        }
    }


    public XmlNodeList GetCategoryList()
    {
        return this.GetNodeValueList("Category", "Cat_Name");
    }
}