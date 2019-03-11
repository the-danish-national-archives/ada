namespace Ra.Common.Wpf.Utils
{
    #region Namespace Using

    using System;
    using System.Globalization;
    using System.Windows.Controls;

    #endregion

    public class IntValidation : ValidationRule
    {
        #region Properties

        public int Max { get; set; } = int.MaxValue;
        public int Min { get; set; } = int.MinValue;

        #endregion

        #region

        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            var valueParsed = 0;

            try
            {
                var valueString = value as string ?? "";

                if (valueString.Length > 0)
                    valueParsed = int.Parse(valueString, cultureInfo.NumberFormat);
            }
            catch (Exception e)
            {
                return new ValidationResult(false, "Illegal characters or " + e.Message);
            }

            if (valueParsed < Min || valueParsed > Max)
                return new ValidationResult(false,
                    "Please enter an integer in the range: " + Min + " - " + Max + ".");

            return new ValidationResult(true, null);
        }

        #endregion
    }
}