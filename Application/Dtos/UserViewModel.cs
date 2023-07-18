using PaelystSolution.Domain.Entities;
using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

namespace PaelystSolution.Application.Dtos
{
    public class UserViewModel
    {
        public Guid UserId { get; set; }

        [RegularExpression(@"^[a-zA-Z]+[ a-zA-Z-_]*$", ErrorMessage = "Please enter text")]
        [Required]
        [Display(Name = "Enter your name")]
        public string UserName { get; set; }
        [Required]
        [RegularExpression(@"^[A-Za-z0-9._%+-]+@[A-Za-z0-9.-]+\.[A-Za-z]{2,}$", ErrorMessage = "Invalid email address")]
        [Display(Name = "Enter a valid mail")]
        public Gender Gender { get; set; }
        public string UserEmail { get; set; }
        public DateTime DateOfBirth { get; set; }
        public IList<IFormFile> BrowseDocuments { get; set; }
    }

    public class UserDto
    {
        public Guid UserId { get; set; }


        [Required]
        [RegularExpression(@"^[A-Za-z0-9._%+-]+@[A-Za-z0-9.-]+\.[A-Za-z]{2,}$", ErrorMessage = "Invalid email address")]
        [Display(Name = "Enter a valid mail")]
        public string UserEmail { get; set; }
        public IList<Document> BrowseDocuments { get; set; }

    }
    public class LoginUserViewModel
    {
        public string Email { get; set; }
        public Guid UserId { get; set; }
    }

    

}
