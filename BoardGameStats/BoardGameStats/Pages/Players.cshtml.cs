using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using BoardGameStats.Data;
using BoardGameStats.Models;

namespace BoardGameStats.Pages
{
    public class PlayersModel : PageModel
    {
        private readonly PlayerRepository pr;
        [BindProperty]
        public Player newPlayer { get; set; }

        public List<Player> allPlayers { get; set; }

        public PlayersModel(PlayerRepository pr)
        {
            this.pr = pr;
        }
        
        public IActionResult OnGet()
        {
            allPlayers = pr.GetAllPlayers().ToList();
            newPlayer = new Player();

            return Page();
        }

        public IActionResult OnPost()
        {
            pr.Add(newPlayer);
            newPlayer = new Player();
            allPlayers = pr.GetAllPlayers().ToList();
            return Page();
        }
    }
}
