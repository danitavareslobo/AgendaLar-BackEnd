using System.ComponentModel;

namespace AgendaLarAPI.Extensions
{
    public static class EnumExtensions
    {
        public static string GetEnumDescription(this Enum value)
        {
            var fieldInfo = value.GetType().GetField(value.ToString());

            if (fieldInfo?.GetCustomAttributes(
                    typeof(DescriptionAttribute), false) is DescriptionAttribute[] descriptionAttributes && descriptionAttributes.Any())
            {
                return descriptionAttributes.First().Description;
            }

            return value.ToString();
        }
    }
}
