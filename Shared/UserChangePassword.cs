﻿using System.ComponentModel.DataAnnotations;

namespace BlazorEcommerce.Shared;

public class UserChangePassword
{
    [Required, StringLength(100, MinimumLength = 4)]
    public string Password { get; set; } = string.Empty;
    [Compare("Password", ErrorMessage = "The password do not match.")]
    public string ConfirmPassword { get; set; } = string.Empty;
}
