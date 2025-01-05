﻿using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace Clone_Backend_Twitter.Models.Entity;
public class UserModel
{
    public string Slug { get; set; }
    public string Email { get; set; }
    public string PasswordHash { get; set; }
    public string Name { get; set; }
    public string Avatar { get; set; } = "defaultAvatar.png";
    public string Cover { get; set; } = "defaultCover.png";
    public string Bio { get; set; }
    public string Link { get; set; }
    public Collection<TweetModel> Tweets { get; set; }
    public Collection<TweetLikeModel> Likes { get; set; }
}