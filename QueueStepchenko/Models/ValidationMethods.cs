using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace QueueStepchenko.Models
{
    public class ValidationMethods
    {

        //static IRepositoryUser _userRepository;

        //public ValidationMethods(IRepositoryUser userRepo) 
        //{
        //    _userRepository = userRepo;
        //}

        public static ValidationResult CheckLogin(string value)
        {
            if (HttpContext.Current.User.Identity.IsAuthenticated)
            {
                return ValidationResult.Success;
            }
            else
            {
                IRepositoryUser _userRepository = DependencyResolver.Current.GetService<IRepositoryUser>();

                if (_userRepository.isFreeLogin(value))
                {
                    return ValidationResult.Success;
                }
                else
                {
                    return new ValidationResult("Логин занят");
                }
            }
        }

        public static ValidationResult CheckPassword(string value)
        {
            if (value == null)
            {
                return ValidationResult.Success;
            }
            else
            {
                IRepositoryUser _userRepository = DependencyResolver.Current.GetService<IRepositoryUser>();

                if (_userRepository.isVerifyPassword(HttpContext.Current.User.Identity.Name, value))
                {
                    return ValidationResult.Success;
                }
                else
                {
                    return new ValidationResult("Неверный пароль");
                }
            }
            
        }


    }
}