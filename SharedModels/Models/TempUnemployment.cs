using System;
using System.Xml.Serialization;
using System.Collections.Generic;


namespace SharedCode.Models.TempUnemployment
{
    public class TempUnemployment
    {
        public Envelope Envelope { get; set; }
    }

    [XmlRoot(ElementName = "Sender")]
    public class Sender
    {
        [XmlElement(ElementName = "Application")]
        public string Application { get; set; }
        [XmlElement(ElementName = "Name")]
        public string Name { get; set; }
    }

    [XmlRoot(ElementName = "Adres")]
    public class Adres
    {
        [XmlElement(ElementName = "Straat")]
        public string Straat { get; set; }
        [XmlElement(ElementName = "Huisnummer")]
        public string Huisnummer { get; set; }
        [XmlElement(ElementName = "Bus")]
        public string Bus { get; set; }
        [XmlElement(ElementName = "Postcode")]
        public string Postcode { get; set; }
        [XmlElement(ElementName = "Plaats")]
        public string Plaats { get; set; }
    }

    [XmlRoot(ElementName = "Personen_ten_laste")]
    public class Personen_ten_laste
    {
        [XmlElement(ElementName = "Echtgenoot")]
        public string Echtgenoot { get; set; }
        [XmlElement(ElementName = "Gehandicapt_echtgenoot")]
        public string Gehandicapt_echtgenoot { get; set; }
        [XmlElement(ElementName = "Kinderen")]
        public string Kinderen { get; set; }
        [XmlElement(ElementName = "Gehandicapt_kinderen")]
        public string Gehandicapt_kinderen { get; set; }
    }

    [XmlRoot(ElementName = "Werknemer_gegevens")]
    public class Werknemer_gegevens
    {
        [XmlElement(ElementName = "Naam")]
        public string Naam { get; set; }
        [XmlElement(ElementName = "Voornaam")]
        public string Voornaam { get; set; }
        [XmlElement(ElementName = "Rijksregisternummer")]
        public string Rijksregisternummer { get; set; }
        [XmlElement(ElementName = "Adres")]
        public Adres Adres { get; set; }
        [XmlElement(ElementName = "Burgelijke_staat")]
        public string Burgelijke_staat { get; set; }
        [XmlElement(ElementName = "Personen_ten_laste")]
        public Personen_ten_laste Personen_ten_laste { get; set; }
        [XmlElement(ElementName = "Indienst_datum")]
        public string Indienst_datum { get; set; }
        [XmlElement(ElementName = "Uitdienst_datum")]
        public string Uitdienst_datum { get; set; }
    }

    [XmlRoot(ElementName = "Werkgever_gegevens")]
    public class Werkgever_gegevens
    {
        [XmlElement(ElementName = "Naam")]
        public string Naam { get; set; }
        [XmlElement(ElementName = "RSZ_nummer")]
        public string RSZ_nummer { get; set; }
        [XmlElement(ElementName = "Kinderbijslag")]
        public string Kinderbijslag { get; set; }
        [XmlElement(ElementName = "Verzekering")]
        public string Verzekering { get; set; }
    }

    [XmlRoot(ElementName = "Parameters")]
    public class Parameters
    {
        [XmlElement(ElementName = "InputType")]
        public string InputType { get; set; }
        [XmlElement(ElementName = "Werknemer_gegevens")]
        public Werknemer_gegevens Werknemer_gegevens { get; set; }
        [XmlElement(ElementName = "Werkgever_gegevens")]
        public Werkgever_gegevens Werkgever_gegevens { get; set; }
    }

    [XmlRoot(ElementName = "GenerateDocument")]
    public class GenerateDocument
    {
        [XmlElement(ElementName = "OutputType")]
        public string OutputType { get; set; }
        [XmlElement(ElementName = "Parameters")]
        public Parameters Parameters { get; set; }
    }

    [XmlRoot(ElementName = "Report")]
    public class Payload
    {
        [XmlElement(ElementName = "GenerateDocument")]
        public GenerateDocument GenerateDocument { get; set; }
    }

    [XmlRoot(ElementName = "Envelope")]
    public class Envelope
    {
        [XmlElement(ElementName = "Sender")]
        public Sender Sender { get; set; }
        [XmlElement(ElementName = "Payload")]
        public Payload Payload { get; set; }
        [XmlAttribute(AttributeName = "xsi", Namespace = "http://www.w3.org/2000/xmlns/")]
        public string Xsi { get; set; }
        [XmlAttribute(AttributeName = "noNamespaceSchemaLocation", Namespace = "http://www.w3.org/2001/XMLSchema-instance")]
        public string NoNamespaceSchemaLocation { get; set; }
    }

}


