
using HiveAPI.Models;
using HiveAPI.Security;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HiveAPI.Data
{
    public static class DbInitializer
    {
        public static void Initialize(HiveApiContext hiveContext)
        {
            hiveContext.Database.EnsureCreated();

            if (hiveContext.Users.Any())
            {
                return;
            }

            var user1 = new User("example", "example@example.com", PasswordHasher.CalculateHash("1234"));
            var user2 = new User("Coney", "coney@o2.pl", PasswordHasher.CalculateHash("1234"));
            var user3 = new User("Eumajos", "eumajos@gmail.com", PasswordHasher.CalculateHash("1234"));
            var user4 = new User("Milson", "milson@gmail.com", PasswordHasher.CalculateHash("1234"));
            var user5 = new User("Lichoton", "lichoton@o2.pl", PasswordHasher.CalculateHash("1234"));
            var user6 = new User("Kuzniecow", "kuz@cnn.com", PasswordHasher.CalculateHash("1234"));
            var user7 = new User("Major", "major@op.com", PasswordHasher.CalculateHash("1234"));
            var user8 = new User("Matusio", "matus@onet.pl", PasswordHasher.CalculateHash("1234"));
            var user9 = new User("test", "test@o2.pl", PasswordHasher.CalculateHash("1234"));

            var users = new User[]
            {
                user1,
                user2,
                user3,
                user4,
                user5,
                user6,
                user7,
                user8,
                user9
            };

            foreach (var user in users)
            {
                hiveContext.Users.Add(user);
            }

            hiveContext.SaveChanges();

            if (hiveContext.Matches.Any())
            {
                return;
            }

            var match1 = new Match(user1, user2, 120, 113);
            var match2 = new Match(user1, user3, 212, 22);
            var match3 = new Match(user1, user4, 32, 333);
            var match4 = new Match(user2, user3, 442, 123);
            var match5 = new Match(user2, user4, 120, 113);
            var match6 = new Match(user2, user5, 212, 22);
            var match7 = new Match(user2, user6, 32, 333);
            var match8 = new Match(user2, user7, 442, 123);
            var match9 = new Match(user3, user1, 120, 113);
            var match10 = new Match(user3, user2, 212, 22);
            var match11 = new Match(user3, user4, 32, 333);
            var match12 = new Match(user3, user5, 442, 123);
            var match13 = new Match(user3, user6, 120, 113);
            var match14 = new Match(user3, user7, 212, 22);
            var match15 = new Match(user3, user8, 32, 333);

            var matches = new Match[]
            {
                match1,
                match2,
                match3,
                match4,
                match5,
                match6,
                match7,
                match8,
                match9,
                match10,
                match11,
                match12,
                match13,
                match14,
                match15
            };

            foreach (var match in matches)
            {
                hiveContext.Matches.Add(match);
            }

            hiveContext.SaveChanges();
        }
    }
}