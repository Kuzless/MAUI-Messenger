using Moq;
using MyMessenger.Application.DTO;
using MyMessenger.Application.DTO.AuthDTOs;
using MyMessenger.Application.Services.Interfaces;
using MyMessenger.Application.СommandsQueries.Users.Commands;


namespace MyMessenger.Application.Tests.Commands
{
    public class UserCommandsTests
    {
        [Fact]
        public void SignUpCommandHandler_InvokesSignUp()
        {
            var signUpService = new Mock<ISignUpService>();
            var user = new SignUpDTO();
            var command = new SignUpCommand(user);
            var sut = new SignUpCommandHandler(signUpService.Object);
            signUpService.Setup(s => s.SignUp(It.IsAny<SignUpDTO>())).Returns(Task.FromResult(It.IsAny<ResponseDTO>()));

            sut.Handle(command, default);

            signUpService.Verify(s => s.SignUp(It.IsAny<SignUpDTO>()), Times.Once);
        }
        public void SignUpCommandHandler_ReturnsCorrectType()
        {
            var signUpService = new Mock<ISignUpService>();
            var user = new SignUpDTO();
            var command = new SignUpCommand(user);
            var sut = new SignUpCommandHandler(signUpService.Object);
            signUpService.Setup(s => s.SignUp(It.IsAny<SignUpDTO>())).Returns(Task.FromResult(It.IsAny<ResponseDTO>()));

            var result = sut.Handle(command, default);

            Assert.IsType<ResponseDTO>(result);
        }
    }
}
