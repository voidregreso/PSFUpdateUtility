using System;
using System.Xml;

namespace PSFUpdateUtility.Unpack
{
	// Token: 0x02000005 RID: 5
	internal class GenerateFileList
	{
		// Token: 0x06000005 RID: 5 RVA: 0x000021C4 File Offset: 0x000003C4
		public static void Generate(string PSFFileName, string DirectoryName)
		{
			XmlDocument xmlDocument = new XmlDocument();
			xmlDocument.Load(DirectoryName + "\\express.psf.cix.xml");
			XmlNode nextSibling = xmlDocument.FirstChild.NextSibling;
			XmlNode xmlNode = nextSibling.FirstChild;
			while (!xmlNode.LocalName.Equals("Files"))
			{
				xmlNode = xmlNode.NextSibling;
			}
			XmlNodeList childNodes = xmlNode.ChildNodes;
			foreach (object obj in childNodes)
			{
				XmlNode xmlNode2 = (XmlNode)obj;
				XmlElement xmlElement = (XmlElement)xmlNode2;
				string attribute = xmlElement.GetAttribute("name");
				long time = long.Parse(xmlElement.GetAttribute("time"));
				XmlNode xmlNode3 = xmlNode2.FirstChild;
				while (!xmlNode3.LocalName.Equals("Delta"))
				{
					xmlNode3 = xmlNode3.NextSibling;
				}
				XmlNode xmlNode4 = xmlNode3.FirstChild;
				while (!xmlNode4.LocalName.Equals("Source"))
				{
					xmlNode4 = xmlNode4.NextSibling;
				}
				XmlElement xmlElement2 = (XmlElement)xmlNode4;
				string attribute2 = xmlElement2.GetAttribute("type");
				long sourceOffset = long.Parse(xmlElement2.GetAttribute("offset"));
				int sourceLength = int.Parse(xmlElement2.GetAttribute("length"));
				DeltaFileList.List.Add(new DeltaFile(attribute, time, attribute2, sourceOffset, sourceLength));
			}
		}
	}
}
