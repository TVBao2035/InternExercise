﻿namespace UserService.Models.DTOs
{
    public class UserDetailModal
    {
        public Guid Id { get; set; }
        public string  Name { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
    }
}
