﻿using ErrorOr;

namespace BuberDinner.Domain.Common.Errors;

public static class Errors
{
    public static class User
    {
        public static Error DuplicateEmail => Error.Conflict(
            code: "User.DuplicateEmail",
            description: "Email đã được sử dụng");
    }
}

