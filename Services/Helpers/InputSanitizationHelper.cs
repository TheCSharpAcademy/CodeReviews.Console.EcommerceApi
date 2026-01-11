using System.Text.RegularExpressions;

namespace ECommerceApp.RyanW84.Services.Helpers;

/// <summary>
/// Utility helper class for input sanitization and string validation.
/// Provides methods for trimming, validation, and safe string handling.
/// </summary>
public static class InputSanitizationHelper
{
    /// <summary>
    /// Safely trims a string and returns null if result is empty.
    /// </summary>
    /// <param name="input">The input string to trim</param>
    /// <returns>Trimmed string or null if empty after trimming</returns>
    public static string? SafeTrim(string? input)
    {
        if (string.IsNullOrWhiteSpace(input))
            return null;

        var trimmed = input.Trim();
        return string.IsNullOrEmpty(trimmed) ? null : trimmed;
    }

    /// <summary>
    /// Validates that an email address has a basic valid format.
    /// </summary>
    /// <param name="email">The email address to validate</param>
    /// <returns>True if email format is valid</returns>
    public static bool IsValidEmail(string? email)
    {
        if (string.IsNullOrWhiteSpace(email))
            return false;

        try
        {
            var addr = new System.Net.Mail.MailAddress(email);
            return addr.Address == email;
        }
        catch
        {
            return false;
        }
    }

    /// <summary>
    /// Removes potentially harmful characters from input.
    /// Sanitizes common SQL injection and XSS attempt patterns.
    /// </summary>
    /// <param name="input">The input string to sanitize</param>
    /// <returns>Sanitized string</returns>
    public static string SanitizeInput(string? input)
    {
        if (string.IsNullOrWhiteSpace(input))
            return string.Empty;

        // Remove potential SQL injection characters (basic protection)
        // Note: Parameterized queries provide the primary defense
        var sanitized = Regex.Replace(input, @"[;'""--]", "");
        return sanitized.Trim();
    }

    /// <summary>
    /// Validates that a string is not null or contains only whitespace.
    /// </summary>
    /// <param name="input">The input string to check</param>
    /// <param name="fieldName">The name of the field (for error messages)</param>
    /// <returns>Validation error message if invalid, null if valid</returns>
    public static string? ValidateNotEmpty(string? input, string fieldName)
    {
        if (string.IsNullOrWhiteSpace(input))
            return $"{fieldName} is required and cannot be empty.";

        return null;
    }

    /// <summary>
    /// Validates that a string length is within acceptable bounds.
    /// </summary>
    /// <param name="input">The input string to check</param>
    /// <param name="minLength">Minimum acceptable length</param>
    /// <param name="maxLength">Maximum acceptable length</param>
    /// <param name="fieldName">The name of the field (for error messages)</param>
    /// <returns>Validation error message if invalid, null if valid</returns>
    public static string? ValidateLength(string? input, int minLength, int maxLength, string fieldName)
    {
        if (string.IsNullOrWhiteSpace(input))
            return $"{fieldName} is required.";

        if (input.Length < minLength)
            return $"{fieldName} must be at least {minLength} characters long.";

        if (input.Length > maxLength)
            return $"{fieldName} must not exceed {maxLength} characters.";

        return null;
    }
}
