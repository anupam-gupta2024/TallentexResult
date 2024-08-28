using System.Data;

namespace TallentexResult.Servies
{
    public class Global
    {
        #region GetOTP
        /// <summary>
        /// Generate Ramdom number for OTP
        /// </summary>
        /// <returns>4 digit number</returns>
        public string GetOTP()
        {
            var chars = "0123456789";
            var stringChars = new char[4];
            var random = new Random();
            for (int i = 0; i < stringChars.Length; i++)
            {
                stringChars[i] = chars[random.Next(chars.Length)];
            }
            string finalString = new String(stringChars);
            return finalString;
        }
        #endregion

        #region Format
        /// <summary>
        /// Convert numeric to Roman
        /// </summary>
        /// <param name="number">Digit to convert</param>
        /// <returns>Roman number</returns>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        public string ToRoman(int number)
        {
            if ((number < 0) || (number > 3999)) throw new ArgumentOutOfRangeException("insert value betwheen 1 and 3999");
            if (number < 1) return string.Empty;
            if (number >= 1000) return "M" + ToRoman(number - 1000);
            if (number >= 900) return "CM" + ToRoman(number - 900); //EDIT: i've typed 400 instead 900
            if (number >= 500) return "D" + ToRoman(number - 500);
            if (number >= 400) return "CD" + ToRoman(number - 400);
            if (number >= 100) return "C" + ToRoman(number - 100);
            if (number >= 90) return "XC" + ToRoman(number - 90);
            if (number >= 50) return "L" + ToRoman(number - 50);
            if (number >= 40) return "XL" + ToRoman(number - 40);
            if (number >= 10) return "X" + ToRoman(number - 10);
            if (number >= 9) return "IX" + ToRoman(number - 9);
            if (number >= 5) return "V" + ToRoman(number - 5);
            if (number >= 4) return "IV" + ToRoman(number - 4);
            if (number >= 1) return "I" + ToRoman(number - 1);
            throw new ArgumentOutOfRangeException("something bad happened");
        }

        /// <summary>
        /// Format string in propercase
        /// </summary>
        public string Capitalize(string input)
        {
            return System.Globalization.CultureInfo.CurrentUICulture.TextInfo.ToTitleCase(input.Trim().ToLower());
        }
        #endregion

        #region DataTable
        /// <summary>
        /// Sum the rows value in datatable
        /// </summary>
        /// <returns>Integer for rows sum</returns>
        public Int32 Sum(DataTable dt, string columnName)
        {
            Int32 a = 0;

            if (dt != null && dt.Columns.Contains(columnName))
            {
                return dt.AsEnumerable()
                    .Where(r => !r.IsNull(columnName) && Int32.TryParse(r[columnName].ToString(), out a))
                    .Sum(r => a);
            }

            return 0;
        }
        #endregion
    }
}
