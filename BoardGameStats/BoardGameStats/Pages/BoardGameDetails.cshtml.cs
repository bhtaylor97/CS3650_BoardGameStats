using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using BoardGameStats.Data;

namespace BoardGameStats.Pages
{
    public class BoardGameDetailsModel : PageModel
    {
        public Helpers helpers { get; set; }
        public Helpers.BoardGameDetails details { get; set; }
        public BoardGameDetailsModel(Helpers helpers)
        {
            this.helpers = helpers;
        }

        public void OnGet(int boardGameId)
        {
            details = helpers.GetBoardGameDetails(boardGameId);
        }
    }
}
