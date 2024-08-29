namespace ConsoleApp.Model
{
    public class ExhibitionMatch
    {
        public string Date { get; set; }  // Ova svojstva moraju biti public
        public string Opponent { get; set; }
        public string Result { get; set; }

        public ExhibitionMatch(string date, string opponent, string result)
        {
            Date = date;
            Opponent = opponent;
            Result = result;
        }

        public override string ToString()
        {
            return $"Date: {Date}, Opponent: {Opponent}, Result: {Result}";
        }
    }
}
