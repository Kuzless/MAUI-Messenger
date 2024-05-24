using MediatR;
using MyMessenger.Domain.Interfaces;

namespace MyMessenger.Application.СommandsQueries.Users.Commands
{
    public class UpdateInfoCommandHandler : IRequestHandler<UpdateInfoCommand>
    {
        private readonly IUnitOfWork unitOfWork;
        public UpdateInfoCommandHandler(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }
        public async Task Handle(UpdateInfoCommand request, CancellationToken cancellationToken)
        {
            var user = unitOfWork.User.GetById(request.Id);
            if (!string.IsNullOrEmpty(request.Name))
            {
                user.Name = request.Name;
            }
            if (!string.IsNullOrEmpty(request.Phone))
            {
                user.PhoneNumber = request.Phone;
            }
            unitOfWork.User.Update(user);
            await unitOfWork.SaveAsync();
        }
    }
}
