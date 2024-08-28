using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using ShopApp.Mvc.ViewModels.ProductVM;
using System.Net.Http.Headers;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace ShopApp.Mvc.Controllers
{
    public class ProductController : Controller
    {
        [Route("product/{search}/{page}")]

        public async  Task<IActionResult> Index(string search, string Category, int page = 1)
        {
           using HttpClient client = new HttpClient();
            client.DefaultRequestHeaders.Authorization =
           new AuthenticationHeaderValue("Bearer", Request.Cookies["token"]);
            HttpResponseMessage response = await client.GetAsync($"http://localhost:5104/api/Product?page={page}&search={search}&Category={Category}");
            
            if (response.IsSuccessStatusCode)
            { 
              
                var data = await response.Content.ReadAsStringAsync();
                var FinalResult = JsonConvert.DeserializeObject<ProductListVM>(data);
                return View(FinalResult);

            }
            return BadRequest();

        }
    }
}
