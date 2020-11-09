using System;
using System.Xml.Serialization;
using System.Collections.Generic;

namespace MyWebApplication.Models
{
	[XmlRoot(ElementName = "Sender")]
	public class Sender
	{
		[XmlElement(ElementName = "Application")]
		public string Application { get; set; }
		[XmlElement(ElementName = "Name")]
		public string Name { get; set; }
	}

	[XmlRoot(ElementName = "Address")]
	public class Address
	{
		[XmlElement(ElementName = "Street")]
		public string Street { get; set; }
		[XmlElement(ElementName = "Number")]
		public string Number { get; set; }
		[XmlElement(ElementName = "City")]
		public string City { get; set; }
		[XmlElement(ElementName = "PostalCode")]
		public string PostalCode { get; set; }
	}

	[XmlRoot(ElementName = "Parameters")]
	public class Parameters
	{
		[XmlElement(ElementName = "Name")]
		public string Name { get; set; }
		[XmlElement(ElementName = "Surname")]
		public string Surname { get; set; }
		[XmlElement(ElementName = "Address")]
		public Address Address { get; set; }
	}

	[XmlRoot(ElementName = "GenerateDocument")]
	public class GenerateDocument
	{
		[XmlElement(ElementName = "OutputType")]
		public string OutputType { get; set; }
		[XmlElement(ElementName = "Parameters")]
		public Parameters Parameters { get; set; }
	}

	[XmlRoot(ElementName = "Payload")]
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
	}

}