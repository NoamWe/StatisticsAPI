using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace StatisticsAPI.Model
{
    public class ClosestAddressInput
    {
        [JsonPropertyName("target-location")]
        [Display(Name = "target-location")]
        [BindRequired]
        public string TargetLocation { get; set; }
    }
}