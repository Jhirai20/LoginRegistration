using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LoginRegistration
{
    public class User
    {
        [Key]
        public int id{get;set;}
        [Required(ErrorMessage="Please Enter First Name!")]
        [MinLength(2,ErrorMessage="First Name Needs At Least 2 Characters!")]
        [RegularExpression(@"^[a-zA-Z]+$", ErrorMessage = "first name must be letters only")]

        public string first_name{get;set;}
        [Required(ErrorMessage="Please Enter Last Name!")]
        [MinLength(2,ErrorMessage="Last Name Needs At Least 2 Characters!")]
        [RegularExpression(@"^[a-zA-Z]+$", ErrorMessage = "Last name must be letters only")]
        public string last_name{get;set;}
        [Required(ErrorMessage="Please Enter Email!")]
        [EmailAddress(ErrorMessage="Please Enter a Valid Email!")]
        public string email{get;set;}
        [Required(ErrorMessage="Please Enter Password!")]
        [MinLength(8,ErrorMessage="Password Must Be More Than 8 Characters!")]
        [DataType(DataType.Password)]
        public string pw_hash{get;set;}
        public DateTime created_at{get;set;}=DateTime.Now;
        public DateTime updated_at{get;set;}=DateTime.Now;
        [NotMapped]
        [Compare("pw_hash",ErrorMessage="Passwords Do Not Match!")]
        [DataType(DataType.Password)]
        public string confirm{get;set;}
    }
}