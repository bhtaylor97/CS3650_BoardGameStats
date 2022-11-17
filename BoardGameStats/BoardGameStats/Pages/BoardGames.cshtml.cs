using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using BoardGameStats.Data;
using BoardGameStats.Models;

namespace BoardGameStats.Pages
{
    public class BoardGamesModel : PageModel
    {
        private readonly BoardGameRepository bgr;

        [BindProperty]
        public BoardGame newBoardGame { get; set; }
        public List<BoardGame> allBoardGames { get; set; }

        public BoardGamesModel(BoardGameRepository bgr)
        {
            this.bgr = bgr;
        }
        public IActionResult OnGet()
        {
            allBoardGames = bgr.GetAllBoardGames().ToList();
            newBoardGame = new BoardGame();

            return Page();
        }

        public IActionResult OnPost()
        {
            newBoardGame.NumPlays = 0;
            bgr.Add(newBoardGame);
            newBoardGame = new BoardGame();
            allBoardGames = bgr.GetAllBoardGames().ToList();
            return Page();
        }
    }
}
