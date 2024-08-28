using System.Data;

namespace TallentexResult.Entities
{
    public class StudentMain
    {
        public string Fno { get; set; }
        public string Class { get; set; }

        public DataSet? ds1 { get; set; }
        public DataSet ds2 { get; set; }
        public DataSet ds3 { get; set; }

        public Verification verify { get; set; }

        public enum Key
        {
            percentages,
            marks,
            score,
            difficulty
        }
    }    
}
