namespace MyApp.Model
{
    public class ExhibitionMatch(string date, string opponent, string result)
    {
        public string Date { get; set; } = date;
        public string Opponent { get; set; } = opponent;
        public string Result { get; set; } = result;

        public override string ToString()
        {
            return $"Date: {Date}, Opponent: {Opponent}, Result: {Result}";
        }
    }
}
