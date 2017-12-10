using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HiveAPI.Models
{
    public class MatchDto
    {
        public MatchDto(Match match)
        {
            Id = match.Id;
            Player1 = new UserDto(match.Player1);
            Player2 = new UserDto(match.Player2);
            Player1Points = match.Player1Points;
            Player2Points = match.Player2Points;
            Date = match.Date;
        }

        public Guid Id { get; set; }
        public UserDto Player1 { get; set; } 
        public UserDto Player2 { get; set; }
        public long Player1Points { get; set; }
        public long Player2Points { get; set; }

        public DateTime Date { get; set; }
    }
}
