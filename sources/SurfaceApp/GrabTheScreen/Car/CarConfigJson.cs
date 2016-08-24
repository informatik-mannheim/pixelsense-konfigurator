using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System;

namespace GrabTheScreen.Car
{
    /// <summary>
    /// DTO to transfer car data from and to JSON by using the <see>
    ///         <cref>JsonSerializer</cref>
    ///     </see>.
    /// </summary>
    [DataContract]
    public class CarConfigJson
    {
        [DataMember(Name="product")]
        public Product Product;

        public void SetColor(string color) 
        {
            Product.AttributeGroups.Single(at => at.Name == "Exterior").Attributes.Single(at => at.Name == "Farbe").SelectedValues = new List<string> { color };
        }

        public string GetColor()
        {
            try
            {
                return Product.AttributeGroups.Single(at => at.Name == "Exterior").Attributes.Single(at => at.Name == "Farbe").SelectedValues[0];
            }
            catch(Exception)
            {
                return "";
            }
        }

        public static CarConfigJson Default() 
        {
            CarConfigJson json = new CarConfigJson()
            {
                Product = new Product()
                {
                    AttributeGroups = new List<AttributeGroups>() 
                    {
                        new AttributeGroups() 
                        {
                            Name = "Exterior",
                            Attributes = new List<Attributes>() 
                            {
                                new Attributes() 
                                {
                                    Name = "Farbe",
                                    SelectedValues = new List<string> { "Grün" }
                                },

                                new Attributes() 
                                {
                                    Name = "Scheibentönung",
                                    SelectedValues = new List<string> { "Ja" }

                                },

                                 new Attributes() 
                                {
                                    Name = "Felgen",
                                    SelectedValues = new List<string> { "Felge B" }

                                }
                            }
                        },

                        new AttributeGroups() 
                        {
                            Name = "Interior",
                            Attributes = new List<Attributes>() 
                            {
                                new Attributes() 
                                {
                                    Name = "Polster",
                                    SelectedValues = new List<string> { "Leder" }
                                },

                                new Attributes() 
                                {
                                    Name = "Navigation",
                                    SelectedValues = new List<string> { "Ja" }

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
        [DataMember(Name = "attributeGroups")]
        public List<AttributeGroups> AttributeGroups;
    }

    [DataContract]
    public class AttributeGroups
    {
        [DataMember(Name="name")]
        public string Name;

        [DataMember(Name="attributes")]
        public List<Attributes> Attributes;
    }

    [DataContract]
    public class Attributes
    {
        [DataMember(Name = "name")]
        public string Name;

        [DataMember(Name = "selectedValues")]
        public List<string> SelectedValues;
    }
}
