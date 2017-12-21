using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace HiveAPI.Models
{
    public class Match
    {
        public Match()
        {

        }

        public Match(User player1, User player2, long player1Points, long player2Points)
        {
            Player1 = player1;
            Player2 = player2;
            Player1Points = player1Points;
            Player2Points = player2Points;
            Date = DateTime.Now;
        }

        public Match(Guid id, User player1, User player2, long player1Points, long player2Points)
            : this(player1, player2, player1Points, player2Points)
        {
            Id = id;          
        }

        public Match(Guid id, User player1, User player2, long player1Points, long player2Points, DateTime date) 
            : this(id, player1, player2, player1Points, player2Points)
        {
            Date = date;
        }
        
        public Guid Id { get; set; }
        [Required]
        public User Player1 { get; set; }
        [Required]
        public User Player2 { get; set; }
        [Required]
        public long Player1Points { get; set; }
        [Required]
        public long Player2Points { get; set; }

        public DateTime Date { get; set; }

    }
}
