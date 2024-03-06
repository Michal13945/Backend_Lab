namespace BlazorChat.Services
{
    public class UserService : IUserService
    {

        public void Add(string connectionId, string username)
        {
            Data.Add(connectionId, username);
        }


        public void RemoveByName(string username)
        {
            var pairsToRemove = Data.Where(pair => pair.Value == username).ToList();
            foreach (var pair in pairsToRemove)
            {
                Data.Remove(pair.Key);
            }
        }

        public string GetConnectionIdByName(string username)
        {
            var pair = Data.FirstOrDefault(x => x.Value == username);
            return pair.Key;
        }


        public Dictionary<string, string> Data = new Dictionary<string, string>();
        public IEnumerable<(string ConnectionId, string Username)> GetAll()
        {
            return Data.Select(x => (x.Key, x.Value));
        }




    }
}
