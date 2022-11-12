using BoardGameStats.Models;

namespace BoardGameStats.Data
{
    public class Helpers
    { 
        private readonly BoardGameRepository _boardGameRepository;
        private readonly GameRepository _gameRepository;
        private readonly PlayerRepository _playerRepository;
        public HelpersHelpers HelpersHelpers;

        public Helpers(BoardGameRepository boardGameRepository, GameRepository gameRepository, PlayerRepository playerRepository, HelpersHelpers helpersHelpers)
        {
            _boardGameRepository = boardGameRepository;
            _gameRepository = gameRepository;
            _playerRepository = playerRepository;
            HelpersHelpers = helpersHelpers;
        }

        public class HighScore
        {
            public decimal Score { get; set; }
            public string? GameName { get; set; }
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
            public int HighestScore { get; set; }
            //Lowest score
            public int LowestScore { get; set; }
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
            foreach(Game game in playerGames)
            {
                if (game.Won)
                {
                    playerDetails.TotalWins++;
                }
            }

            //***************Get WinPercentage*******************************
            playerDetails.WinPercentage = Decimal.Round((playerDetails.TotalWins / playerDetails.ThisPlayer.NumPlays) * 100, 2);


            //*****************Get TopThreeGames******************************

            //Separate all games played by board game
            List<List<Game>> gamesByBoardGame = HelpersHelpers.GetGamesByBoardGame(playerGames);

            //Calculate win percentages for each board game group
            decimal[] winPercentages = new decimal[gamesByBoardGame.Count()];
            for(int i = 0; i < gamesByBoardGame.Count(); i++)
            {
                int wins = 0;
                int totalPlays = 0;
                //Count wins and total plays
                foreach(Game g in gamesByBoardGame[i])
                {
                    if (g.Won)
                    {
                        wins++;
                    }
                    totalPlays++;
                }
                
                //Add calculated percentage to winPercentages array
                winPercentages[i] = Decimal.Round(((decimal)wins / (decimal)totalPlays) * 100, 2);
            }

            IEnumerable<BoardGame> allBoardGames = _boardGameRepository.GetAllBoardGames();

            //Get the greatest 3 win percentages
            for (int i = 0; i < 3; i++)
            {
                decimal winPercentage = 0;
                int bestIndex = 0;
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
                winPercentages[bestIndex] = 0;
            }

            //************************Get HighScores***************************
            //In what games does this player have the high score?
            //Sort ALL game sessions into a 2D list
            gamesByBoardGame = HelpersHelpers.GetGamesByBoardGame(allGames);
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
    }
}
