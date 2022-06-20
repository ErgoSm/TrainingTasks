
namespace TrainingTasks.Task2
{
    internal sealed class User
    {
        internal string FirstName { get; set; }
        internal string LastName { get; set; }
        internal DateTime DateOfBirth { get; set; }
        internal long INN { get; set; }

        public override bool Equals(object? obj)
        {
            var other = obj as User;
            return FirstName == other?.FirstName && LastName == other.LastName && DateOfBirth == other.DateOfBirth && INN == other.INN;
        }

        public static bool operator ==(User a, User b)
        {
            return a.Equals(b);
        }

        public static bool operator !=(User a, User b)
        {
            return !a.Equals(b);
        }

        public override int GetHashCode()
        {
            return FirstName.GetHashCode() ^ LastName.GetHashCode() ^ DateOfBirth.GetHashCode() ^ INN.GetHashCode();
        }
    }
}
