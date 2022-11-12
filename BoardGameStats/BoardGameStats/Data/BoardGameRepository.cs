using BoardGameStats.Models;

namespace BoardGameStats.Data
{
    public class BoardGameRepository
    {
        private readonly BoardGameStatsContext context;

        public BoardGameRepository(BoardGameStatsContext context)
        {
            this.context = context;
        }

        public BoardGame Add(BoardGame newBoardGame)
        {
            context.BoardGames.Add(newBoardGame);
            context.SaveChanges();
            return newBoardGame;
        }

        public void Delete(BoardGame bgToDelete)
        {
            context.BoardGames.Remove(bgToDelete);
            context.SaveChanges();
        }

        public IEnumerable<BoardGame> GetAllBoardGames()
        {
            return context.BoardGames;
        }
    }
}
