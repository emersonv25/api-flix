﻿using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Api.MyFlix.Models
{
    [Index(nameof(SeasonKey))]
    public class Season
    {
        [Key]
        public int SeasonId { get; set; }
        public string? SeasonKey { get; set; }
        public int Number { get; set; }
        public List<Episode>? Episodes { get; set; }
        public DateTime CreatedDate { get; set; } = DateTime.Now;
        [ForeignKey("Movie")]
        public int MovieId { get; set; }
        public Movie? Movie { get; set; }

    }
}
