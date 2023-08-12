using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace BugTrackingDAL
{
    public class Comments
    {
        public int Id { get; set; }

        public string Content { get; set; } = string.Empty;
        public string UserType { get; set; } = string.Empty;

        [ForeignKey("user")]
        public string UserId { get; set; } = string.Empty;
        public GeneralUser? user { get; set; }

        [ForeignKey("Ticket")]
        public int TickectId { get; set; }
        public Ticket? Ticket { get; set; }
    }
}
