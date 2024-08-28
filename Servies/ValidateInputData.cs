namespace TallentexResult.Servies
{
    public class ValidateInputData
    {
        /// <summary>
        /// Validate 10 digit Mobile number
        /// </summary>
        /// <returns>return true if mobile is valid</returns>
        public static bool ValidateMobileNumber(string Value)
        {
            if (Value.Length != 10)
                return false;
            char[] Val = Value.ToCharArray();
            for (int i = 0; i < Val.Length; i++)
            {
                int intAscii = (int)Val[i];
                if (intAscii < 48 || intAscii > 57)
                {
                    return false;
                }
            }

            return true;
        }
    }
}
