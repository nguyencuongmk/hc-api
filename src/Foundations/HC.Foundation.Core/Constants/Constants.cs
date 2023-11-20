using HC.Foundation.Cormmon.Attributes;
using System.ComponentModel;

namespace HC.Foundation.Core.Constants
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

        public enum Role
        {
            [RoleInfo(Name = "Admin", Code = "ADM")]
            Admin,

            [RoleInfo(Name = "Customer", Code = "CUS")]
            Customer
        }

        public static class ResponseResult
        {
            public static class Description
            {
                public const string TOKEN_EMPTY = "Token is empty";
                public const string TOKEN_EXPIRED = "Token is expired";
                public const string TOKEN_INVALID = "Token is invalid";
            }
        }
    }
}