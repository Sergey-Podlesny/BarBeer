using BarBeer.Exceptions;
using BarBeer.Models;
using BarBeer.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Threading.Tasks;

namespace BarBeer.ViewModels
{
    public class UserViewModel : IValidation
    {
        public string UserLogin { get; set; }
        public string UserPassword { get; set; }
        public string UserRole { get; set; }
        public string UserEmail { get; set; }

        public void CheckValid()
        {
            if (UserRole != Roles.Admin.ToString() && UserRole != Roles.Visitor.ToString())
            {
                throw new InvalidModelException("Введена несуществующая роль.");
            }

            try
            {
                MailAddress mailAddress = new MailAddress(UserEmail);
            }
            catch
            {
                throw new InvalidModelException("Введена несуществующая почта.");
            }
        }

    }
}
