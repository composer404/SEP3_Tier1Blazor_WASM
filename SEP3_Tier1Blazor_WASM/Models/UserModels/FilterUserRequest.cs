﻿using System.Text.Json.Serialization;

namespace SEP3_Tier1Blazor_WASM.Models.UserModels
{
    public class FilterUserRequest
    {
        [JsonPropertyName("senderId")]
        public int SenderId { get; set; }
        [JsonPropertyName("queryString")]
        public string QueryString { get; set; }
    }
}