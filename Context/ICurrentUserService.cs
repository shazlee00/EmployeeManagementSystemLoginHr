﻿namespace EmployeeManagementSystemLoginHr.Context
{
    public interface ICurrentUserService
    {
        string UserId { get; }
        string UserName { get; }
        string UserRole { get; }
    }



}