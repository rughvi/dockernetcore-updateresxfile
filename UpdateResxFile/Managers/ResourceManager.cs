using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Resources;
using UpdateResxFile.Models;
using System.Collections;
using System.IO;
using System.Xml;
using System.Xml.Linq;

namespace UpdateResxFile.Managers
{
    public class ResourceManager
    {
        private const string _sourceResourceFile = @"C:\ResxFiles\AppResourcesSource.resx";
        private const string _destinationResourceFile = @"C:\ResxFiles\AppResourcesDestination.resx";

        public List<ResourceModel> GetResources()
        {
            List<ResourceModel> resourceModels = new List<ResourceModel>();
            using (XmlReader reader = XmlReader.Create(_sourceResourceFile))
            {
                reader.MoveToContent();
                while (reader.Read())
                {
                    if (reader.NodeType == XmlNodeType.Element)
                    {
                        if (reader.Name == "data")
                        {
                            XElement el = XNode.ReadFrom(reader) as XElement;
                            if (el != null)
                            {
                                ResourceModel resourceModel = new ResourceModel();
                                resourceModel.Key = el.Attribute("name").Value;
                                resourceModel.SourceStr = el.Element("value").Value;
                                resourceModels.Add(resourceModel);
                            }
                        }
                    }
                }
            }

            using (XmlReader reader = XmlReader.Create(_destinationResourceFile))
            {
                reader.MoveToContent();
                while (reader.Read())
                {
                    if (reader.NodeType == XmlNodeType.Element)
                    {
                        if (reader.Name == "data")
                        {
                            XElement el = XNode.ReadFrom(reader) as XElement;
                            if (el != null)
                            {
                                
                                var key = el.Attribute("name").Value;
                                var value = el.Element("value").Value;

                                ResourceModel existingResourceModel = resourceModels.SingleOrDefault(rm => rm.Key == key);
                                if(existingResourceModel != null)
                                {
                                    existingResourceModel.DestinationStr = value;
                                }
                                else
                                {
                                    ResourceModel resourceModel = new ResourceModel()
                                    {
                                        Key = key,
                                        DestinationStr = value
                                    };
                                    resourceModels.Add(resourceModel);
                                }                                
                            }
                        }
                    }
                }
            }

            return resourceModels;
        }

        public void UpdateDestinationResources(List<ResourceModel> resourceModels)
        {
            XDocument xdoc = XDocument.Load(_destinationResourceFile);
            foreach(ResourceModel resourceModel in resourceModels)
            {
                var el = xdoc.Root.Elements("data").SingleOrDefault(e => e.Attribute("name").Value == resourceModel.Key)?.Element("value");
                el?.SetValue(resourceModel.DestinationStr);
            }
            xdoc.Save(_destinationResourceFile);            
        }
    }
}
