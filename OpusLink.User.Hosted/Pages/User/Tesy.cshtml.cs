using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace OpusLink.User.Hosted.Pages.User
{
    public class TesyModel : PageModel
    {
        public async Task OnPostForTestAsync(IFormCollection collection)
        {
            List<string> keys = collection.Keys.ToList<string>();
            int x;
            // get values
            foreach (string key in keys)
            {
                if (key.Contains("Test1"))
                {
                    x = Int32.Parse(collection[key]);
                }
            }
        }
    }
}
