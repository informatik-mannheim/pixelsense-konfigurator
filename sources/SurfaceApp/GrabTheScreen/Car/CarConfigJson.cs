using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace GrabTheScreen
{
    /// <summary>
    /// DTO to transfer car data from and to JSON by using the <see cref="JsonSerializer" />.
    /// </summary>
    [DataContract]
    public class CarConfigJson
    {
        [DataMember(Name="product")]
        public Product product;

        public void SetColor(string color) 
        {
            product.attributeGroups.Single(at => at.name == "Exterior").attributes.Single(at => at.name == "Farbe").selectedValues = new List<string> { color };
        }

        public string GetColor()
        {
            return product.attributeGroups.Single(at => at.name == "Exterior").attributes.Single(at => at.name == "Farbe").selectedValues[0];
        }

        public static CarConfigJson Default() 
        {
            CarConfigJson json = new CarConfigJson()
            {
                product = new Product()
                {
                    attributeGroups = new List<AttributeGroups>() 
                    {
                        new AttributeGroups() 
                        {
                            name = "Exterior",
                            attributes = new List<Attributes>() 
                            {
                                new Attributes() 
                                {
                                    name = "Farbe",
                                    selectedValues = new List<string> { "Grün" }
                                },

                                new Attributes() 
                                {
                                    name = "Scheibentönung",
                                    selectedValues = new List<string> { "Ja" }

                                },

                                 new Attributes() 
                                {
                                    name = "Felgen",
                                    selectedValues = new List<string> { "Felge B" }

                                }
                            }
                        },

                        new AttributeGroups() 
                        {
                            name = "Interior",
                            attributes = new List<Attributes>() 
                            {
                                new Attributes() 
                                {
                                    name = "Polster",
                                    selectedValues = new List<string> { "Leder" }
                                },

                                new Attributes() 
                                {
                                    name = "Navigation",
                                    selectedValues = new List<string> { "Ja" }

                                }
                            }
                        },
                    }
                }
            };

            return json;
        }
    }

    [DataContract]
    public class Product
    {
        [DataMember]
        public List<AttributeGroups> attributeGroups;
    }

    [DataContract]
    public class AttributeGroups
    {
        [DataMember]
        public string name;

        [DataMember]
        public List<Attributes> attributes;
    }

    [DataContract]
    public class Attributes
    {
        [DataMember]
        public string name;

        [DataMember]
        public List<string> selectedValues;
    }
}
