using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace QueueStepchenko.Models
{
    public class EmployeeViewModel
    {
        public int EmployeeId { get; set; }

        [Required]
        [Display(Name = "Логин")]
        [CustomValidation(typeof(ValidationMethods), "CheckLogin")]
        public string Login { get; set; }

        [Required]
        [Display(Name = "Имя")]
        public string Name { get; set; }

        [Required]
        [Display(Name = "Должность")]
        public string Position { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Старый пароль")]
        [CustomValidation(typeof(ValidationMethods), "CheckPassword")]
        public string OldPassword { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Пароль")]
        public string Password { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Подтвердите пароль")]
        [Compare("Password")]
        public string ConfirmPassword { get; set; }

        [Display(Name = "Изменить пароль")]
        public bool isChangePassword { get; set; }
        public string ErrorMessage { get; set; }
    }
}