using BoardGameStats.Models;

namespace BoardGameStats.Data
{
    public class HelpersHelpers
    {
        //Takes a list of games and returns a 2D list of games sorted by board game
        public List<List<Game>> GetGamesByBoardGame (IEnumerable<Game> games)
        {
            List<List<Game>> gamesByBoardGame = new List<List<Game>>();
            bool gameHasBeenAdded;

            //Populate the list
            foreach (Game g in games)
            {
                gameHasBeenAdded = false;
                //Add initial list if needed
                if (gamesByBoardGame.Count() == 0)
                {
                    gamesByBoardGame.Add(new List<Game>());
                    gamesByBoardGame[0].Add(g);
                }
                else
                {
                    //Search lists in gamesByBoardGame to find matching boardGameIDs
                    foreach (List<Game> l in gamesByBoardGame)
                    {
                        if (g.BoardGameId == l[0].BoardGameId)
                        {
                            l.Add(g);
                            gameHasBeenAdded = true;
                            break;
                        }
                    }
                    //If the game was not added by this point, there is no list for any games with this boardGameId yet.
                    if (!gameHasBeenAdded)
                    {
                        gamesByBoardGame.Add(new List<Game>());
                        gamesByBoardGame[gamesByBoardGame.Count() - 1].Add(g);
                    }
                }
            }

            return gamesByBoardGame;
        }

        //Takes a list of games and returns a 2D list of games sorted by player
        public List<List<Game>> GetGamesByPlayer(IEnumerable<Game> games)
        {
            List<List<Game>> gamesByPlayer = new List<List<Game>>();
            bool gameHasBeenAdded;

            //Populate the list
            foreach (Game g in games)
            {
                gameHasBeenAdded = false;
                //Add initial list if needed
                if (gamesByPlayer.Count() == 0)
                {
                    gamesByPlayer.Add(new List<Game>());
                    gamesByPlayer[0].Add(g);
                }
                else
                {
                    //Search lists in gamesByBoardGame to find matching playerIDs
                    foreach (List<Game> l in gamesByPlayer)
                    {
                        if (g.PlayerId == l[0].PlayerId)
                        {
                            l.Add(g);
                            gameHasBeenAdded = true;
                            break;
                        }
                    }
                    //If the game was not added by this point, there is no list for any games with this boardGameId yet.
                    if (!gameHasBeenAdded)
                    {
                        gamesByPlayer.Add(new List<Game>());
                        gamesByPlayer[gamesByPlayer.Count() - 1].Add(g);
                    }
                }
            }

            return gamesByPlayer;
        }

        //Gets win percentages from a 2D list of games
        public decimal[] GetWinPercentages(List<List<Game>> sortedGames)
        {
            decimal[] winPercentages = new decimal[sortedGames.Count()];
            for (int i = 0; i < sortedGames.Count(); i++)
            {
                int wins = 0;
                int totalPlays = 0;
                //Count wins and total plays
                foreach (Game g in sortedGames[i])
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

            return winPercentages;
        }
    }
}
