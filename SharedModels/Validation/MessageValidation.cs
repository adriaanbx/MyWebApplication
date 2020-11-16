using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Schema;
using System.IO;
using System.Xml.Serialization;

namespace SharedCode.Validation
{
    public class MessageValidation
    {
        public static string OutputType { get; private set; }
        public static string InputType { get; private set; }
        private static string _errorMessage = null;
        private static object result = null;


        public static object? Validate(MemoryStream memoryStream)
        {
            GetInputAndOutputInfo(memoryStream);

            if ((OutputType == "HTML" || OutputType == "XML") && InputType == "Loonbrief")
            {
                result = ValidateXMLPayslip(memoryStream);
            }
            else if ((OutputType == "HTML" || OutputType == "XML") && InputType == "Tijdelijke_werkloosheid")
            {
                result = ValidateXMLTempUnemployment(memoryStream);
            }
            else if (OutputType == "PDF")
            {
                _errorMessage = "You have requested " + OutputType + " output, this output type is not supported yet.";
            }
            else if (InputType != "Loonbrief" && InputType != "Tijdelijke_werkloosheid")
            {
                _errorMessage = "This input type is invalid";
            }
            else _errorMessage = "This output type is invalid";

            if (_errorMessage == null)
            {
                Console.WriteLine("Validation succesfull");
                return result;
            }
            else
            {
                Console.WriteLine(_errorMessage);
                return null;
            }


        }

        private static void GetInputAndOutputInfo(MemoryStream memoryStream)
        {
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.Load(memoryStream);
            XmlNode output_typeNode = xmlDoc.SelectSingleNode("//OutputType");
            OutputType = output_typeNode.InnerText;
            XmlNode input_typeNode = xmlDoc.SelectSingleNode("//InputType");
            InputType = input_typeNode.InnerText;
        }

        private static object? ValidateXMLTempUnemployment(MemoryStream memoryStream)
        {
            try
            {
                XmlReaderSettings settings = new XmlReaderSettings();
                settings.Schemas.Add("", "..\\..\\XML\\tijdelijke_werkloosheid.xsd");
                settings.ValidationType = ValidationType.Schema;
                settings.ValidationFlags |= System.Xml.Schema.XmlSchemaValidationFlags.ReportValidationWarnings;
                settings.ValidationEventHandler += new System.Xml.Schema.ValidationEventHandler(ValidationEventHandle);

                
                Models.TempUnemployment.Envelope result;
                using (var input = new StreamReader(memoryStream))
                {
                    using (XmlReader reader = XmlReader.Create(input, settings))
                    {
                        XmlSerializer ser = new XmlSerializer(typeof(Models.TempUnemployment.Envelope));
                        result = (Models.TempUnemployment.Envelope)ser.Deserialize(reader);
                    }
                }
                return result;

            }
            catch (Exception ex)
            {
                _errorMessage = "Error while validating: " + ex.Message;
                return null;
            }
        }

        private static object? ValidateXMLPayslip(MemoryStream memoryStream)
        {
            //Console.WriteLine("You have requested " + OutputType + " output, this output type is supported. Your input type is " + InputType + ". Loonbrief.");
            XmlReader xmlReader;
            try
            {
                XmlReaderSettings settings = new XmlReaderSettings();
                settings.Schemas.Add("", "..\\..\\XML\\loonbrief.xsd");
                settings.ValidationType = ValidationType.Schema;
                settings.ValidationFlags |= System.Xml.Schema.XmlSchemaValidationFlags.ReportValidationWarnings;
                settings.ValidationEventHandler += new System.Xml.Schema.ValidationEventHandler(ValidationEventHandle);

                
                Models.Payslip.Envelope result;
                memoryStream.Position = 0;
                using (var input = new StreamReader(memoryStream))
                {
                    memoryStream.Position = 0;
                    using (XmlReader reader = XmlReader.Create(input, settings))
                    {
                        //memoryStream.Position = 0;
                        XmlSerializer ser = new XmlSerializer(typeof(Models.Payslip.Envelope));
                        //TODO hexadecimal value 0x00, is an invalid character. Line 42, position 12.
                        //Encoding UTF8 werkt niet.
                        result = (Models.Payslip.Envelope)ser.Deserialize(reader);

                    }
                }
                return result;

            }
            catch (Exception ex)
            {
                _errorMessage = "Error while validating: " + ex.Message;
                return null;
            }
        }

        private static void ValidationEventHandle(object sender, ValidationEventArgs e)
        {
            throw new Exception("The XML you sent us contains invalid data. \r\n" + e.Message);
        }
    }
}
