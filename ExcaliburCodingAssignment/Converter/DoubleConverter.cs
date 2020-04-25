
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Text.Json;
using System.Text.Json.Serialization;
using Microsoft.VisualBasic.CompilerServices;

namespace ExcaliburCodingAssignment.Converter
{
    internal class DoubleConverter : JsonConverter<double>
    {

        public DoubleConverter()
        {

        }

        public override bool CanConvert(Type typeToConvert)
        {
            return typeToConvert == typeof(double);
        }

        public override double Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            throw new NotImplementedException();
        }

        public override void Write(Utf8JsonWriter writer, double value, JsonSerializerOptions options)
        {
            writer.WriteStringValue(((decimal)value).ToString("$0.00"));
        }
    }
}
