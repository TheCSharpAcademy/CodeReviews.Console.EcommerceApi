namespace App.Util;

public class UserFacingException : Exception
{
    public UserFacingException(string message) : base(message)
    { }
}