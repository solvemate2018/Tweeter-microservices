using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace TweetsService.Models.DTOs
{
    public class CreateTweetDTO
    {
        public string Content { get; set; }

        public List<string> Tags { get; set; }
    }
}
