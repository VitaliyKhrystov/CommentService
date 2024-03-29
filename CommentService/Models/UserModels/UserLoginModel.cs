﻿using System.ComponentModel.DataAnnotations;

namespace CommentService.Models.UserModels
{
    public class UserLoginModel
    {
        [Required]
        public string NickName { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }
    }
}
