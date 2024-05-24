using MediatR;

namespace MyMessenger.Application.СommandsQueries.Users.Commands
{
    public class UpdateImageCommand : IRequest
    {
        public string Id { get; set; }
        public string ImageUrl { get; set; }
        public UpdateImageCommand(string id, string url)
        {
            Id = id;
            ImageUrl = url;
        }
    }
}
