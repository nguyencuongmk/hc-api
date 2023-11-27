namespace HC.Service.Authentication
{
    public static class Constants
    {
        public static class Message
        {
            public const string NOT_ENOUGH_INFO = "Not enough information";
            public const string INVALID_EMAIL = "Email is invalid";
            public const string CANNOT_CONNECT_DB = "Unable to connect to database";
            public const string ERROR_SAVE = "An error occurred while saving data to the database";
            public const string ERROR_CREATE_USER = "An error occurred while creating new user";
            public const string ERROR_ADD_USER_ROLE = "An error occurred while assigning role to user";
            public const string NOT_EXISTS_USER = "User does not exists";
            public const string NOT_EXISTS_ROLE = "Role does not exists";
            public const string GRANTED_ROLE = "Role has been granted to user";
            public const string LOGIN_FAILED = "Username or Password is invalid";
        }
    }
}
