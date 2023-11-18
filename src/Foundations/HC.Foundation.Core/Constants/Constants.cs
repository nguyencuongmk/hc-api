﻿using System.ComponentModel;

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