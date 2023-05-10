namespace AccountManager.Models
{
    public class AccountInfoModel
    {
        public int Id { get; set; }
        public string? Login { get; set; }
        public string? Password { get; set; }
        public string? Name { get; set; }
        public string? Surname { get; set; }
        public string? Phone { get; set; }
        public string? Email { get; set; }

        public AccountInfoModel(string? login, string? password, string? name, string? surname, string? phone, string? email)
        {
            Login = login;
            Password = password;
            Name = name;
            Surname = surname;
            Phone = phone;
            Email = email;
        }
    }
}