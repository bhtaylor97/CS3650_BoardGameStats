using BoardGameStats.Models;

namespace BoardGameStats.Data
{
    public class PlayerRepository
    {
        private readonly BoardGameStatsContext context;
        private readonly GameRepository gameRepository;

        public PlayerRepository(BoardGameStatsContext context, GameRepository gameRepository)
        {
            this.context = context;
            this.gameRepository = gameRepository;
        }

        public Player Add(Player newPlayer)
        {
            context.Players.Add(newPlayer);
            context.SaveChanges();
            return newPlayer;
        }

        public void Delete(Player playerToDelete)
        {
            context.Players.Remove(playerToDelete);
            context.SaveChanges();
        }

        public IEnumerable<Player> GetAllPlayers()
        {
            return context.Players;
        }

        public Player GetPlayer(int id)
        {
            return context.Players.Find(id);
        }


    }
}
