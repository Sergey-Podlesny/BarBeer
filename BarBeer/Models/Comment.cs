using System;
using System.Collections.Generic;

#nullable disable

namespace BarBeer.Models
{
    public partial class Comment
    {
        public int Id { get; set; }
        public int BarId { get; set; }
        public int UserId { get; set; }
        public string Text { get; set; }

        [System.Text.Json.Serialization.JsonIgnore]
        public virtual Bar Bar { get; set; }
        [System.Text.Json.Serialization.JsonIgnore]
        public virtual User User { get; set; }
    }
}
