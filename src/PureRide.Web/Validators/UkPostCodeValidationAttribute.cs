using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace PureRide.Web.Validators
{
    public class UkPostCodeAttribute : ValidationAttribute
    {

        private readonly Regex _ukPostCodeRegEx = new Regex(@"(GIR 0AA)|((([A-Z-[QVX]][0-9][0-9]?)|(([A-Z-[QVX]][A-Z-[IJZ]][0-9][0-9]?)|(([A-Z-[QVX‌​]][0-9][A-HJKSTUW])|([A-Z-[QVX]][A-Z-[IJZ]][0-9][ABEHMNPRVWXY]))))\s?[0-9][A-Z-[C‌​IKMOV]]{2})",RegexOptions.Compiled);

        public override bool IsValid(object value)
        {
            return _ukPostCodeRegEx.IsMatch(value.ToString().ToUpper());
        }
    }
}