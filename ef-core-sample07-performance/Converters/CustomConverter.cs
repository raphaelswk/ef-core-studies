using System;
using System.Linq;
using MasteringEFCore.Domain;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace MasteringEFCore.Converters
{
    public class CustomConverter : ValueConverter<Status, string>
    {
        public CustomConverter() : base(
            p => ConvertToDB(p),
            value => ConvertToApplication(value),
            new ConverterMappingHints(1))
        {            
        }

        static string ConvertToDB(Status status)
        {
            return status.ToString()[0..1];
        }

        static Status ConvertToApplication(string value)
        {
            return Enum.GetValues<Status>()
                       .FirstOrDefault(p => p.ToString()[0..1] == value);
        }
    }
}