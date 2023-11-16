﻿using HC.Foundation.Data.Base;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using static HC.Foundation.Core.Constants.Constants;

namespace HC.Foundation.Data.Entities
{
    public class UserRole : IBaseInfo
    {
        [Key]
        public int UserId { get; set; }

        [Key]
        public int RoleId { get; set; }

        [ForeignKey("UserId")]
        public User User { get; set; }

        [ForeignKey("RoleId")]
        public Role Role { get; set; }

        public Status Status { get; set; }

        public DateTime CreatedOn { get; set; }

        public DateTime UpdatedOn { get; set; }

        public string CreatedBy { get; set; }

        public string UpdatedBy { get; set; }
    }
}