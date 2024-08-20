using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Text;

namespace ShopApp.Mvc.Controllers
{
    public class AccountController : Controller
    {
        public IActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> LogIn(string username, string password)
        {
            using HttpClient client = new();
            StringContent content = new StringContent(JsonConvert.SerializeObject(new { username, password }), Encoding.UTF8, "application/json");
            HttpResponseMessage response = await client.PostAsync("http://localhost:5104/api/Auth/LogIn", content);
            if (response.IsSuccessStatusCode)
            {
                var data = await response.Content.ReadAsStringAsync();
                var result = JsonConvert.DeserializeObject<UserToken>(data);
                Response.Cookies.Append("token", result.Token);

                return Content("success");

            }
            return BadRequest();
        }
        public class UserToken()
        {
            public string Token { get; set; }
        }
    }
}
