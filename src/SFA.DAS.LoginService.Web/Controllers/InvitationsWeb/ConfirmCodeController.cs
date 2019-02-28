using System;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using SFA.DAS.LoginService.Application.ConfirmCode;
using SFA.DAS.LoginService.Application.GetInvitationById;
using SFA.DAS.LoginService.Web.Controllers.InvitationsWeb.ViewModels;

namespace SFA.DAS.LoginService.Web.Controllers.InvitationsWeb
{
    public class ConfirmCodeController : Controller
    {
        private readonly IMediator _mediator;

        public ConfirmCodeController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet("/Invitations/ConfirmCode/{invitationId}")]
        public async Task<ActionResult> Get(Guid invitationId)
        {
            var invitationResponse = await _mediator.Send(new GetInvitationByIdRequest(invitationId));
            
            if (invitationResponse != null && !invitationResponse.IsUserCreated)
            {
                var confirmCodeViewModel = new ConfirmCodeViewModel(invitationResponse.Id, "");
                return View("ConfirmCode", confirmCodeViewModel);
            }
            
            return View("InvitationExpired");
        }

        [HttpPost("/Invitations/ConfirmCode/{invitationId}")]
        public async Task<ActionResult> Post(ConfirmCodeViewModel confirmCodeViewModel)
        {
            if (string.IsNullOrWhiteSpace(confirmCodeViewModel.Code))
            {
                ModelState.AddModelError("Code","Please supply code");
            
                return View("ConfirmCode", confirmCodeViewModel);
            }
            
            var confirmCodeResponse = await _mediator.Send(new ConfirmCodeRequest(confirmCodeViewModel.InvitationId, confirmCodeViewModel.Code));
            if (confirmCodeResponse.IsValid)
            {
                return RedirectToAction("Get", "CreatePassword", new {id = confirmCodeViewModel.InvitationId});
            }

            ModelState.AddModelError("Code","Code not valid");
            
            return View("ConfirmCode", confirmCodeViewModel);
        }
    }
}