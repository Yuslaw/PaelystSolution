using Microsoft.AspNetCore.Mvc;
using PaelystSolution.Application.Dtos;
using PaelystSolution.Application.Interfaces;

namespace PaelystSolution.Controllers
{
    public class UserController : Controller
    {

        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        public IActionResult Index()
        {
            return View();
        }

       
        [HttpPost]
        public async Task<IActionResult> Create(UserViewModel model)
        {
            
           var response = await _userService.Create(model);


            return RedirectToAction("Get");
        }

        public IActionResult Get()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Get(LoginUserViewModel model)
        {
            var response = await _userService.CheckInfo(model);
            if (response != null)
            {
                return RedirectToAction("GetDetails", new { id = response.Data.UserId });
            }

            return RedirectToAction("Index");
        }


        [HttpGet]
        public async Task<IActionResult> GetDetails(Guid id)
        {
            var user = await _userService.Get(id);

            if(user != null)
            {
                return View(user.Data);
            }
            return View();
            
        }
    }
}
