﻿namespace NZWalksAPI.Models.Domain
{
    public class Role
    {
        public Guid Id { get; set; }
        public string Name { get; set; }

        //Navigation properties
        public List<User_Role> UserRoles { get; set; }
    }
}
