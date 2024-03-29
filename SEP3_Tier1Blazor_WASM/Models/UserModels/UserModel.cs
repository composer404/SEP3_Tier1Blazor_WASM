﻿using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace SEP3_Tier1Blazor_WASM.Models.UserModels
{
    public class User
    {
        [JsonPropertyName("id")]
        public int Id { get; set; }
        
        [Required]   
        [JsonPropertyName("email")]
        public string Email { get; set; }
        
        [JsonPropertyName("password")]
        public string Password { get; set; }
        
        [JsonPropertyName("name")]
        public string Name { get; set; }
        
        [Required]  
        [JsonPropertyName("city")]
        public string City { get; set; }
        
        [JsonPropertyName("description")]
        public string Description { get; set; }

        [JsonPropertyName("avatar")]
        public byte[] Avatar { get; set; }
        
        [JsonPropertyName("profileBackground")]
        public byte[] ProfileBackground { get; set; }
        
        [JsonPropertyName("address")]
        public Address Address { get; set; }

        [JsonPropertyName("userStatus")] 
        public bool[] UserStatus { get; set; }
        
        [JsonPropertyName("relevantFriendsNumber")]
        public int FriendsNumber { get; set;}
        
        [JsonPropertyName("score")]
        public int Score { get; set; }
        
        [JsonPropertyName("rating")]
        public int Rating { get; set; }

        public override string ToString()
        {
            return Id + Name + Email + Password + Avatar + ProfileBackground +
                   Description;
        }
    }
}