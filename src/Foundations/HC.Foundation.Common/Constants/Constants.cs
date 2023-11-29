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

        public enum Role
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

        public static class ResponseResult
        {
            public static class Description
            {
                public const string NO_PERMISSION = "No permission";
                public const string TOKEN_INVALID = "Token is invalid";
            }
        }
    }
}