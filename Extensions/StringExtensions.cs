using System.ComponentModel.DataAnnotations;

namespace AgendaLarAPI.Extensions
{
    public static class StringExtensions
    {
        public static bool IsValidEmail(this string? email)
        {
            return new EmailAddressAttribute().IsValid(email);
        }

        public static bool IsValidPassword(this string? password)
        {
            var attribute = new StringLengthAttribute(100) { MinimumLength = 6 };
            return !string.IsNullOrEmpty(password) && attribute.IsValid(password);
        }
    }
}
