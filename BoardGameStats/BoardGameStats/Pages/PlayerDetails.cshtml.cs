using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using BoardGameStats.Data;

namespace BoardGameStats.Pages
{
    public class PlayerDetailsModel : PageModel
    {
        public Helpers helpers { get; set; }
        public Helpers.PlayerDetails details { get; set; }
        public PlayerDetailsModel(Helpers helpers)
        {
            this.helpers = helpers;
        }

        public void OnGet(int playerId)
        {
            details = helpers.GetPlayerDetails(playerId);
        }
    }
}
