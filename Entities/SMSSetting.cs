namespace TallentexResult.Entities
{
    /// <summary>
    /// getter & setter class for SMS variable
    /// </summary>
    public class SMSSetting
    {
        public string url { get; set; }
        public string userid { get; set; }
        public string password { get; set; }
        public string mask { get; set; }

        /// <summary>
        /// Generate SMS URL by passing all parameter
        /// </summary>
        /// <param name="message">SMS Language</param>
        /// <param name="to">Mobile No.</param>
        /// <returns>URL string</returns>
        public string generateUrl(string message, string to)
        {
            return $"{this.url}?msg={message}&v=1.1&userid={this.userid}&password={this.password}&send_to={to}&msg_type=text&method=sendMessage&mask=TALENT&format=text";
        }
    }


}
