using System;
using System.Collections.Generic;

#nullable disable

namespace BarBeer.Models
{
    public partial class User
    {
        public User()
        {
            Comments = new HashSet<Comment>();
            PersonalBestBars = new HashSet<PersonalBestBar>();
        }

        public int Id { get; set; }
        public string UserLogin { get; set; }
        public string UserPassword { get; set; }
        public string UserRole { get; set; }

        public virtual ICollection<Comment> Comments { get; set; }
        public virtual ICollection<PersonalBestBar> PersonalBestBars { get; set; }
    }
}
