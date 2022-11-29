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

        public BoardGame Get(int id)
        {
            return context.BoardGames.Find(id);
        }

        public BoardGame Update(BoardGame bgToUpdate)
        {
            var bg = context.BoardGames.Attach(bgToUpdate);
            bg.State = Microsoft.EntityFrameworkCore.EntityState.Modified;
            context.SaveChanges();
            return bgToUpdate;
        }
    }
}
