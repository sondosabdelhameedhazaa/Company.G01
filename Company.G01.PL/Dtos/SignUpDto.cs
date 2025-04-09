using System.ComponentModel.DataAnnotations;

namespace Company.G01.PL.Dtos
{
    public class SignUpDto
    {
        [Required(ErrorMessage = "Username is Required")]
        public string UserName { get; set; }
        [Required(ErrorMessage = "FirstName is Required")]

        public string FirstName { get; set; }
        [Required(ErrorMessage = "lastName is Required")]

        public string LastName { get; set; }
        [Required(ErrorMessage = "Email is Required")]
        [EmailAddress]
        public string Email { get; set; }
        [Required(ErrorMessage = "Password is Required")]
        [DataType(DataType.Password)]
        public string Password { get; set; }
        [Required(ErrorMessage = "ConfirmPassword is Required")]
        [DataType(DataType.Password)]
        [Compare(nameof(Password), ErrorMessage = "Confirm Password dont match password")]
        public string ConfirmPassword { get; set; }
        public bool IsAgree { get; set; }

    }
}