using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
namespace LLI.Models
{
    public class User
    {
        [DefaultValue(0)]
        public int Id { get; set; }

        [Required(ErrorMessage = "Please enter a valid username")]
        public string username { get; set; }

        [Required(ErrorMessage = "please enter a password")]
        [DataType(DataType.Password)] 
        public string password { get; set; }

    }
}
