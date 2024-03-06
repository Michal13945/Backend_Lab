namespace BlazorChat.Services
{
    public interface IUserService
    {
        public void Add(string connectionId, string username);
        public void RemoveByName(string username);
        public string GetConnectionIdByName(string username);
        public IEnumerable<(string ConnectionId, string Username)> GetAll();
    }
}
