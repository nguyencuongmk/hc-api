using HC.Foundation.Data.Entities;
using HC.Service.Authentication.Helpers;
using Microsoft.EntityFrameworkCore;

namespace HC.Service.Authentication.Data
{
    public static class Initializer
    {
        public static void SeedData(ModelBuilder builder)
        {
            #region Role

            builder.Entity<Role>().HasData
            (
                new Role
                {
                    Id = 1,
                    Name = "Admin",
                    Code = "ADM",
                    CreatedOn = DateTime.Now,
                    UpdatedOn = DateTime.Now,
                    CreatedBy = "system",
                    Status = Foundation.Common.Constants.Constants.Status.Created
                },
                new Role
                {
                    Id = 2,
                    Name = "Customer",
                    Code = "CUS",
                    CreatedOn = DateTime.Now,
                    UpdatedOn = DateTime.Now,
                    CreatedBy = "system",
                    Status = Foundation.Common.Constants.Constants.Status.Created
                }
            );

            #endregion Role

            #region User

            builder.Entity<User>().HasData
            (
                new User
                {
                    Id = 1,
                    UserName = "Administrator",
                    Email = "administrator@localhost.com",
                    EmailConfirmed = true,
                    PasswordHash = PasswordHelper.EncodePasswordToBase64("CuongNM11!"),
                    IsLocked = false,
                    CreatedOn = DateTime.Now,
                    UpdatedOn = DateTime.Now,
                    CreatedBy = "system",
                    Status = Foundation.Common.Constants.Constants.Status.Created
                }
            );

            #endregion User

            #region UserRole

            builder.Entity("RoleUser").HasData( new Dictionary<string, object> { ["UsersId"] = 1, ["RolesId"] = 1 });
            //builder.Entity<UserRole>().HasData
            //(
            //    new UserRole
            //    {
            //        Id = 1,
            //        UserId = 1,
            //        RoleId = 1,
            //        CreatedOn = DateTime.Now,
            //        UpdatedOn = DateTime.Now,
            //        CreatedBy = "system",
            //        Status = Foundation.Common.Constants.Constants.Status.Created
            //    }
            //);

            #endregion UserRole
        }
    }
}