using System;
using System.Collections.Generic;

#nullable disable

namespace BarBeer.Models
{
    public partial class Bar
    {
        public Bar()
        {
            Comments = new HashSet<Comment>();
            PersonalBestBars = new HashSet<PersonalBestBar>();
        }

        public int Id { get; set; }
        public string BarName { get; set; }
        public string BarImage { get; set; }
        public double? BarRating { get; set; }
        public string BarLocation { get; set; }

        public virtual ICollection<Comment> Comments { get; set; }
        public virtual ICollection<PersonalBestBar> PersonalBestBars { get; set; }
    }
}
