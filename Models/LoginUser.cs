using System;
using System.ComponentModel.DataAnnotations;

namespace LoginRegistration.Models
{
    public class LoginUser
    {
        [Required(ErrorMessage="Please enter a email!")]
        [EmailAddress(ErrorMessage="Please enter a valid email!")]
        public string Email{get;set;}
        [Required(ErrorMessage="Please enter a password!")]
        public string password{get;set;}
    }
}