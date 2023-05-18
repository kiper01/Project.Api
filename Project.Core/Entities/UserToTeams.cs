using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Project.Core.Entities
{
    public class UserToTeams
    {
        [Key]
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public User user { get; set; }
        public Guid TeamsId { get; set; }
        public Teams teams { get; set; }
        public Boolean status { get; set; }

    }
}
