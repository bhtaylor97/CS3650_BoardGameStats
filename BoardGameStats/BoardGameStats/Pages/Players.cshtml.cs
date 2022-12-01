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

        [BindProperty(SupportsGet = true)]
        public string? SearchString { get; set; }

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

        public IActionResult OnPostSearch()
        {
            newPlayer = new Player();
            IEnumerable<Player> somePlayers = pr.GetAllPlayers();
            if (SearchString != "" && SearchString is not null)
            {
                somePlayers = somePlayers.Where(f => f.Name.Contains(SearchString) || f.Name.Contains(SearchString.ToLower()) || f.Name.Contains(SearchString.ToUpper()));
            }

            allPlayers = somePlayers.ToList();
            return Page();
        }
    }
}
