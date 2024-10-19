using clout.Interface;
using clout.DbContexts;


namespace clout.Services
{
    public class UserIdGeneratorService : IUserIdGeneratorService
    {
        private readonly clout_DbContexts _context;

        public UserIdGeneratorService(clout_DbContexts context)
        {
            _context = context;
        }

        public string GenerateUserId()
        {
            var letters = GenerateRandomLetters(3);
            var nextNumber = GetNextAvailableNumber(letters);
            return $"{letters}_{nextNumber:D2}";
        }

        private string GenerateRandomLetters(int length)
        {
            var random = new Random();
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
            return new string(Enumerable.Repeat(chars, length)
              .Select(s => s[random.Next(s.Length)]).ToArray());
        }

        private int GetNextAvailableNumber(string letters)
        {
            var existingUsers = _context.Users
                .Where(u => u.Username.StartsWith(letters))
                .Select(u => u.Username)
                .ToList();

            var numbers = existingUsers
                .Select(Username => int.Parse(Username.Split('_')[1]))
                .OrderBy(num => num)
                .ToList();

            int nextNumber = 1;
            foreach (var number in numbers)
            {
                if (number != nextNumber)
                    break;
                nextNumber++;
            }

            return nextNumber;
        }
    }
}
