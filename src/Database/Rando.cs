using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace NinjaBotCore.Database
{
    public partial class Randos
    {
        
        public string ListName { get; set; }
        public string ListItem { get; set; }
        [Key]
        public Guid ListId { get; set; }
        public long ListWeight { get; set; }
        public long NumberOfPlayers { get; set; }
        public Nullable<long> ServerId { get; set; }
        public string AddedBy { get; set; }
        public Nullable<long> SetById { get; set; }
        public Nullable<System.DateTime> TimeSet { get; set; }

    }
}
