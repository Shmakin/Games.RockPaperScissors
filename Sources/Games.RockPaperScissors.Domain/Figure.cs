namespace Games.RockPaperScissors.Domain
{
    public class Figure
    {
        public Figure(string id, string figureName)
        {
            this.Id = id;
            this.FigureName = figureName;
        }

        public string Id { get; }

        /*
         We should probably use localizations here
         */
        public string FigureName { get; }
    }
}