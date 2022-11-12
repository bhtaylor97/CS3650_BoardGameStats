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
    }
}
