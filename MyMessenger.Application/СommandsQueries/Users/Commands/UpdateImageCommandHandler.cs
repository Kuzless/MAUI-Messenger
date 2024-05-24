using MediatR;
using MyMessenger.Domain.Interfaces;

namespace MyMessenger.Application.СommandsQueries.Users.Commands
{
    public class UpdateImageCommandHandler : IRequestHandler<UpdateImageCommand>
    {
        private readonly IUnitOfWork unitOfWork;
        public UpdateImageCommandHandler(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }
        public async Task Handle(UpdateImageCommand request, CancellationToken cancellationToken)
        {
            unitOfWork.User.UpdateImage(request.Id, request.ImageUrl);
            await unitOfWork.SaveAsync();
        }
    }
}
