using System.Collections.Generic;
using Elmagram.Services;

namespace Elmagram.Models.HomeViewModels
{
    public class ChatModel
    {
        public List<User> Users { get; set; }

        public string CurrentUserId { get; set; }

        public ChatModel()
        {
            Users = new List<User>();
        }
    }
}