using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using BoardGameStats.Data;
using BoardGameStats.Models;
using System.ComponentModel.DataAnnotations;

namespace BoardGameStats.Pages
{
    public class BoardGamesModel : PageModel
    {
        private readonly BoardGameRepository bgr;

        [BindProperty]
        public BoardGame newBoardGame { get; set; }

        [BindProperty]
        public BufferedImageUpload FileUpload { get; set; }

        [BindProperty(SupportsGet = true)]
        public string? SearchString { get; set; }
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
            if (FileUpload.FormFile != null)
            {
                //Get uploaded image
                using (MemoryStream ms = new MemoryStream())
                {
                    FileUpload.FormFile.CopyTo(ms);

                    //Upload the file if less than 2 MB
                    if (ms.Length < 2097152)
                    {
                        byte[] imageUpload = ms.ToArray();
                        newBoardGame.Image = imageUpload;
                    }
                    else
                    {
                        ModelState.AddModelError("File", "The file is too large!");
                    }
                }
            }

            newBoardGame.NumPlays = 0;
            bgr.Add(newBoardGame);
            newBoardGame = new BoardGame();
            allBoardGames = bgr.GetAllBoardGames().ToList();
            return Page();
        }


        public IActionResult OnPostSearch()
        {
            newBoardGame = new BoardGame();
            IEnumerable<BoardGame> someBoardGames = bgr.GetAllBoardGames();
            if (SearchString != "" && SearchString is not null)
            {
                someBoardGames = someBoardGames.Where(f => f.Name.ToUpper().Contains(SearchString.ToUpper()));
            }

            allBoardGames = someBoardGames.ToList();
            return Page();
        }
    }

    public class BufferedImageUpload
    {
        [Display(Name = "Game Image")]
        public IFormFile FormFile { get; set; }
    }
}
