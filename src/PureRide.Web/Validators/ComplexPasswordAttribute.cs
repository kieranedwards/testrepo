using System;
using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace PureRide.Web.Validators
{

public class ComplexPasswordAttribute : ValidationAttribute
{

    private readonly Regex _complexPassword = new Regex(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d).+$",RegexOptions.Compiled);

    /// <summary>
    /// Determines whether the specified value of the object is valid. 
    /// </summary>
    /// <returns>
    /// true if the specified value is valid; otherwise, false. 
    /// </returns>
    public override bool IsValid(object value)
    {
        if (value == null) return false;
        if (value.GetType() != typeof(string)) throw new InvalidOperationException("can only be used on string properties.");

        var password = (string)value;

        if (string.IsNullOrWhiteSpace(password))
            return false;

        //longer than 6, alpha numeric, upper and lowercase
        if(password.Length <= 6)
            return false;

        return (_complexPassword.IsMatch(password));
    }

    public ComplexPasswordAttribute()
    {
        ErrorMessage = "Please use  more complex password include numbers, letters and a mix of upper and lowercase longer than 6 charactors";
    }

}
}
