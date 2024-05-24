using MediatR;

namespace MyMessenger.Application.СommandsQueries.Users.Commands
{
    public class UpdateInfoCommand : IRequest
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Phone { get; set; }
        public UpdateInfoCommand(string id, string name, string phone)
        {
            Id = id;
            Name = name;
            Phone = phone;
        }
    }
}
