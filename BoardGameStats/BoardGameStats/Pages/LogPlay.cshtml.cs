using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using BoardGameStats.Data;
using BoardGameStats.Models;

namespace BoardGameStats.Pages
{
    public class LogPlayModel : PageModel
    {
        private PlayerRepository pr { get; set; }
        private BoardGameRepository bgr { get; set; }
        private GameRepository gr { get; set; }

        public LogPlayModel(PlayerRepository pr, BoardGameRepository bgr, GameRepository gr)
        {
            this.pr = pr;
            this.bgr = bgr;
            this.gr = gr;
        }

        public List<Player> allPlayers { get; set; }

        public BoardGame boardGame { get; set; }

        [BindProperty]
        public List<Game> newGames { get; set; }

        [BindProperty]
        public Game newGame { get; set; }

        public void OnGet(int boardGameId)
        {
            boardGame = bgr.Get(boardGameId);
            allPlayers = pr.GetAllPlayers().ToList();
            newGames = new List<Game>();
            newGame = new Game();
            newGames.Add(newGame);
        }

        public IActionResult OnPost(int boardGameId)
        {
            foreach(var g in newGames)
            {
                g.BoardGameId = boardGameId;
            }
            gr.AddList(newGames);
            return Redirect("BoardGameDetails/" + boardGameId.ToString());
        }
    }
}
