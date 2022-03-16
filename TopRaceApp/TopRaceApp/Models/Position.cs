using System;
using System.Collections.Generic;
using System.Text.Json;
using System.Text.Json.Serialization;


namespace TopRaceApp.Models
{
    public partial class Position
    {
        public Position()
        {
        }

        public int Id { get; set; }
        public int X { get; set; }
        public int Y { get; set; }

    }
}
