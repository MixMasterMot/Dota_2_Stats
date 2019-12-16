using System;
using System.Collections.Generic;
using System.Text;

namespace Dota_2_Stats.Models
{
    public class GameMode
    {
        public int id { get; set; }
        public string name { get; set; } = "Error";
        public bool balanced { get; set; }
    }
}
