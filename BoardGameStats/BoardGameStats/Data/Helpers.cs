using BoardGameStats.Models;

namespace BoardGameStats.Data
{
    public class Helpers
    { 
        private readonly BoardGameRepository _boardGameRepository;
        private readonly GameRepository _gameRepository;
        private readonly PlayerRepository _playerRepository;
        public HelpersHelpers helpersHelpers;

        public Helpers(BoardGameRepository boardGameRepository, GameRepository gameRepository, PlayerRepository playerRepository, HelpersHelpers helpersHelpers)
        {
            _boardGameRepository = boardGameRepository;
            _gameRepository = gameRepository;
            _playerRepository = playerRepository;
            this.helpersHelpers = helpersHelpers;
        }

        public class HighScore
        {
            public decimal Score { get; set; }
            public string? GameName { get; set; }
        }

        public class PlayerScore
        {
            public decimal Score { get; set; }
            public string? PlayerName { get; set; }
        }
        
        public class BestPlayer
        {
            public string? PlayerName { get; set; }
            public decimal WinPercent { get; set; }
        }

        public class BoardGameAndPercentage
        {
            public string? GameName { get; set; }
            public decimal WinPercentage { get; set; }
        }

        public class PlayerDetails
        {
            //The player being viewed - includes total number of plays
            public Player? ThisPlayer { get; set; }
            //Total wins and what percentage of games they play AND win
            public int TotalWins { get; set; }
            public decimal WinPercentage { get; set; }
            //What games they win most frequently
            public BoardGameAndPercentage[] TopThreeGames { get; set; } = new BoardGameAndPercentage[3];
            //High scores the player has
            public List<HighScore> HighScores { get; set; } = new List<HighScore>();

        }

        public class BoardGameDetails
        {
            //The board game being viewed - includes total number of plays
            public BoardGame? ThisBoardGame { get; set; }
            //Who wins the most (by percentage)
            public BestPlayer? FirstBestPlayer { get; set; }
            //Who wins second most (by percentage)
            public BestPlayer? SecondBestPlayer { get; set; }
            //Highest score
            public PlayerScore HighestScore { get; set; }
            //Lowest score
            public PlayerScore LowestScore { get; set; }
        }

        public PlayerDetails GetPlayerDetails(int playerId)
        {
            //********************Get ThisPlayer****************************
            PlayerDetails playerDetails = new PlayerDetails();
            playerDetails.ThisPlayer = _playerRepository.GetPlayer(playerId);

            IEnumerable<Game> allGames = _gameRepository.GetAllGames();
            IEnumerable<Game> playerGames = allGames.Where(g => g.PlayerId == playerId);

            //******************Get TotalWins*******************************
            playerDetails.TotalWins = 0;
            if (playerGames.Any())
            {
                foreach (Game game in playerGames)
                {
                    if (game.Won)
                    {
                        playerDetails.TotalWins++;
                    }
                }
            }

            //***************Get WinPercentage*******************************
            if (playerDetails.ThisPlayer.NumPlays > 0)
            {
                playerDetails.WinPercentage = Decimal.Round((playerDetails.TotalWins / playerDetails.ThisPlayer.NumPlays) * 100, 2);
            }
            else { playerDetails.WinPercentage = 0; }


            //*****************Get TopThreeGames******************************

            //Separate all games played by board game
            List<List<Game>> gamesByBoardGame = helpersHelpers.GetGamesByBoardGame(playerGames);

            //Calculate win percentages for each board game group
            decimal[] winPercentages = helpersHelpers.GetWinPercentages(gamesByBoardGame);

            IEnumerable<BoardGame> allBoardGames = _boardGameRepository.GetAllBoardGames();

            //Get the greatest 3 win percentages
            for (int i = 0; i < 3; i++)
            {
                decimal winPercentage = 0;
                int bestIndex = 0;
                playerDetails.TopThreeGames[i] = new BoardGameAndPercentage();
                for (int j = 0; j < winPercentages.Length; j++)
                {
                    if (winPercentages[j] > winPercentage)
                    {
                        winPercentage = winPercentages[j];
                        bestIndex = j;
                    }
                }

                //Player might not have played three games by this point, hence the if statements...
                if (winPercentages.Length >= 3)
                {
                    //Add to the playerDetails class
                    playerDetails.TopThreeGames[i].WinPercentage = winPercentage;
                    playerDetails.TopThreeGames[i].GameName = allBoardGames.Where(b => b.Id == gamesByBoardGame[bestIndex][0].BoardGameId).ToList()[0].Name;
                }
                else if (winPercentages.Length == 2)
                {
                    if (i < 2)
                    {
                        //Add to the playerDetails class
                        playerDetails.TopThreeGames[i].WinPercentage = winPercentage;
                        playerDetails.TopThreeGames[i].GameName = allBoardGames.Where(b => b.Id == gamesByBoardGame[bestIndex][0].BoardGameId).ToList()[0].Name;
                    }
                    else
                    {
                        playerDetails.TopThreeGames[i].WinPercentage = 0;
                        playerDetails.TopThreeGames[i].GameName = null;
                    }
                }
                else if (winPercentages.Length == 1)
                {
                    if (i < 1)
                    {
                        playerDetails.TopThreeGames[i].WinPercentage = winPercentage;
                        playerDetails.TopThreeGames[i].GameName = allBoardGames.Where(b => b.Id == gamesByBoardGame[bestIndex][0].BoardGameId).ToList()[0].Name;
                    }
                    else
                    {
                        playerDetails.TopThreeGames[i].WinPercentage = 0;
                        playerDetails.TopThreeGames[i].GameName = null;
                    }
                }
                else //winPercentages.Length == 0
                {
                    playerDetails.TopThreeGames[i].WinPercentage = 0;
                    playerDetails.TopThreeGames[i].GameName = null;
                }

                //Set winPercentages at the best index to 0, this way it will not be counted next iteration...
                if (winPercentages.Length > 0)
                {
                    winPercentages[bestIndex] = 0;
                }
            }

            //************************Get HighScores***************************
            //In what games does this player have the high score?
            //Sort ALL game sessions into a 2D list
            gamesByBoardGame = helpersHelpers.GetGamesByBoardGame(allGames);
            Game[] allHighScoringGames = new Game[gamesByBoardGame.Count()];

            for(int i = 0; i < gamesByBoardGame.Count(); i++)
            {
                //Initialize allHighScoringGames to first game in gamesByBoardGame
                allHighScoringGames[i] = gamesByBoardGame[i][0];
                //Then, get the highest scoring game in each "column"
                foreach(Game g in gamesByBoardGame[i])
                {
                    if (g.Score > allHighScoringGames[i].Score)
                    {
                        allHighScoringGames[i] = g;
                    }
                }
            }

            //Now we can see if the player has any high scoring games...
            List<Game> playerHighScoringGames = new List<Game>();
            for(int i = 0; i < allHighScoringGames.Count(); i++)
            {
                if (allHighScoringGames[i].PlayerId == playerId)
                {
                    playerHighScoringGames.Add(allHighScoringGames[i]);
                }
            }

            //Finally, add them to the HighScore list
            foreach(Game g in playerHighScoringGames)
            {
                HighScore hs = new HighScore();
                hs.Score = g.Score;
                hs.GameName = allBoardGames.Where(b => b.Id == g.BoardGameId).ToList()[0].Name;
                playerDetails.HighScores.Add(hs);
            }



            return playerDetails;
        }

        public BoardGameDetails GetBoardGameDetails(int boardGameId)
        {
            BoardGameDetails details = new BoardGameDetails();
            IEnumerable<Game> allGames = _gameRepository.GetAllGames();
            IEnumerable<Player> allPlayers = _playerRepository.GetAllPlayers();
            //******************Get ThisBoardGame***********************
            details.ThisBoardGame = _boardGameRepository.Get(boardGameId);

            //******************Get FirstBestPlayer**********************
            IEnumerable<Game> boardGameGames = allGames.Where(g => g.BoardGameId == boardGameId);
            //Sort all plays of this game by player
            List<List<Game>> gamesByPlayer = helpersHelpers.GetGamesByPlayer(boardGameGames);
            //Get win percentages for each player
            decimal[] winPercentages = helpersHelpers.GetWinPercentages(gamesByPlayer);
            //Get player name and win percentage for the best player
            string? playerName = "";
            decimal winPercent = 0;
            int bestIndex = 0;

            for(int i = 0; i < winPercentages.Length; i++)
            {
                if (winPercentages[i] > winPercent)
                {
                    winPercent = winPercentages[i];
                    playerName = allPlayers.Where(p => p.Id == gamesByPlayer[i][0].PlayerId).ToList()[0].Name;
                    bestIndex = i;
                }
            }

            details.FirstBestPlayer = new BestPlayer();
            details.FirstBestPlayer.WinPercent = winPercent;
            details.FirstBestPlayer.PlayerName = playerName;

            //**************Get SecondBestPlayer******************************
            //Eliminate the previous winning score - if any games exist
            if (winPercentages.Length > 0)
            {
                winPercentages[bestIndex] = 0;
            }
            playerName = "";
            winPercent = 0;

            for (int i = 0; i < winPercentages.Length; i++)
            {
                if (winPercentages[i] > winPercent)
                {
                    winPercent = winPercentages[i];
                    playerName = allPlayers.Where(p => p.Id == gamesByPlayer[i][0].PlayerId).ToList()[0].Name;
                }
            }

            details.SecondBestPlayer = new BestPlayer();
            details.SecondBestPlayer.PlayerName= playerName;
            details.SecondBestPlayer.WinPercent = winPercent;


            //**************Get HighestScore*********************
            details.HighestScore = new PlayerScore();
            details.HighestScore.Score = 0;
            details.HighestScore.PlayerName = "";
            foreach(List<Game> list in gamesByPlayer)
            {
                foreach(Game g in list)
                {
                    if(g.Score > details.HighestScore.Score)
                    {
                        details.HighestScore.Score = g.Score;
                        details.HighestScore.PlayerName = allPlayers.Where(p => p.Id == g.PlayerId).ToList()[0].Name;
                    }
                }
            }

            //****************Get LowestScore********************
            details.LowestScore = new PlayerScore();
            details.LowestScore.Score = details.HighestScore.Score;
            details.LowestScore.PlayerName = "";

            foreach (List<Game> list in gamesByPlayer)
            {
                foreach (Game g in list)
                {
                    if (g.Score < details.LowestScore.Score)
                    {
                        details.LowestScore.Score = g.Score;
                        details.LowestScore.PlayerName = allPlayers.Where(p => p.Id == g.PlayerId).ToList()[0].Name;
                    }
                }
            }

            return details;
        }
    }
}
