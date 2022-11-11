using BoardGameStats.Models;

namespace BoardGameStats.Data
{
    public class GameRepository
    {
        private readonly BoardGameStatsContext context;

        public GameRepository(BoardGameStatsContext context)
        {
            this.context = context;
        }

        public Game Add(Game newGame)
        {
            context.Games.Add(newGame);
            context.SaveChanges();
            return newGame;
        }

        public void Delete(Game gameToDelete)
        {
            context.Games.Remove(gameToDelete);
            context.SaveChanges();
        }

        public IEnumerable<Game> GetAllGames()
        {
            return context.Games.ToList();
        }

        public IEnumerable<Game> GetGamesByBoardGame(int id)
        {
            var games = GetAllGames();
            games = games.Where(g => g.BoardGameId == id);
            return games;
        }

        public IEnumerable<Game> GetGamesByPlayer(int id)
        {
            var games = GetAllGames();
            games = games.Where(g => g.PlayerId == id);
            return games;
        }
    }
}
