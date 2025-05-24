using System.ComponentModel.DataAnnotations;

namespace GameZone.ViewModel
{
    public class LoginUserViewModel
    {
        public string UserNameOrEmail { get; set; }

        [DataType(DataType.Password)]
        public string Password { get; set; }
        public bool RememberMe { get; set; }
    }
}
