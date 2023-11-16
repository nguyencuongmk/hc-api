﻿using HC.Foundation.Data.Base;

namespace HC.Foundation.Data.Entities
{
    public class Role : BaseEntity
    {
        public string Name { get; set; }

        public string Code { get; set; }

        public virtual List<UserRole> UserRoles { get; set; }
    }
}