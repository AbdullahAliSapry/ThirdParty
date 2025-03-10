using Newtonsoft.Json.Linq;
using System.Dynamic;
using System.IO;
using System.Reflection;
using System.Text;
using System.Xml;
using System.Xml.Serialization;

namespace ThirdParty.Utilities
{
    public class XmlHelper
    {
        public static string ConvertToXml(object value)
        {
            if (value == null) return string.Empty;

            // إنشاء الـ XmlSerializer
            var xmlSerializer = new XmlSerializer(value.GetType());

            var xmlNamespaces = new XmlSerializerNamespaces();
            xmlNamespaces.Add("", "");

            // إعدادات الكتابة لـ XML
            var xmlSettings = new XmlWriterSettings
            {
                Indent = true,
                OmitXmlDeclaration = true,
                Encoding = Encoding.UTF8
            };

            // استخدام StringWriter لكتابة الـ XML
            using (var stringWriter = new StringWriterWithEncoding(new StringBuilder(), Encoding.UTF8))
            {
                using (XmlWriter xmlWriter = XmlWriter.Create(stringWriter, xmlSettings))
                {
                    // إنشاء كائن جديد فقط بالقيم غير الفارغة أو التي تساوي القيم الافتراضية
                    var properties = value.GetType().GetProperties();
                    var validProperties = new List<PropertyInfo>();

                    foreach (var property in properties)
                    {
                        var propertyValue = property.GetValue(value);

                        // تحقق من القيم الفارغة أو التي تساوي صفر
                        if (propertyValue != null &&
                            !(propertyValue is string str && string.IsNullOrEmpty(str)) &&
                            !(propertyValue is decimal decVal && decVal == 0) &&
                            !(propertyValue is int intVal && intVal == 0) &&
                            !(propertyValue is bool boolVal && boolVal == false))  // استبعاد القيم غير الصالحة
                        {
                            validProperties.Add(property);
                        }
                    }

                    // إنشاء كائن جديد من النوع المطلوب وإضافة القيم المعتمدة
                    var filteredObject = Activator.CreateInstance(value.GetType());

                    foreach (var property in validProperties)
                    {
                        var propertyValue = property.GetValue(value);
                        property.SetValue(filteredObject, propertyValue);
                    }

                    // تسلسل الكائن بعد تصفيته
                    xmlSerializer.Serialize(xmlWriter, filteredObject, xmlNamespaces);
                }

                return stringWriter.ToString();
            }
        }

    }

    public class StringWriterWithEncoding : StringWriter
    {
        private readonly Encoding _encoding;

        public StringWriterWithEncoding(StringBuilder builder, Encoding encoding) : base(builder)
        {
            _encoding = encoding;
        }
        public override Encoding Encoding => _encoding;
    }
}

