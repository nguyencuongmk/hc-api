using HC.Foundation.Common.Attributes;
using System.ComponentModel;

namespace HC.Foundation.Common.Constants
{
    public static class Constants
    {
        public enum Status
        {
            [Description("Deleted")]
            Deleted,

            [Description("Created")]
            Created,

            [Description("Modified")]
            Modified
        }

        public enum RoleType
        {
            [RoleInfo(Name = "Admin", Code = "ADM")]
            Admin,

            [RoleInfo(Name = "Customer", Code = "CUS")]
            Customer
        }

        public enum TokenType
        {
            AccessToken,
            RefreshToken
        }
    }
}