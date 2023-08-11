using Microsoft.AspNetCore.Mvc;
using EShop.Services;
using EShop.Models;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using EShop.Repositories;

namespace EShop.Controllers
{
    public class FeedBackController : Controller
    {
        private readonly FeedBackService _feedbackService;
        private readonly FeedBackRepository _feedbackRepository;
        //private readonly UserService _userService;
        private readonly UserService _userService;
        public FeedBackController(FeedBackService feedbackService, UserService userService, FeedBackRepository feedBackRepository)
        {
            _feedbackService = feedbackService;
            _userService = userService;
            _feedbackRepository = feedBackRepository;
            //_userService = userService;
        }
        public async Task<IActionResult> Index()
        {
            var cmt = await _feedbackService.GetComments();

            return Ok(cmt);
        }
        [HttpGet]
        [Route("{customerId}")]
        public async Task<IActionResult<Feedback>> GetCommentById(int id)
        { 
            var cmtbyid = await _feedbackService.GetFeedBackByIdService(id);
            if (cmtbyid == null)
            {
                return (IActionResult<Feedback>)NotFound();
            }
            return (IActionResult<Feedback>)Ok(cmtbyid);
        }

        [HttpDelete]
        [Route("{id}")] // ben tren 1 kieu ben duoi 1 kieu 
        [Authorize]
        public async Task<IActionResult> DeleteCommentByUser(int id)
        {
            // nay bao gom ca check va xoa
            // chuan hon thi trong controller chi dung service, ko dùng repo

            var isDelete = await _feedbackService.IsDeleteFeedBackByUserSerVice(id, User);

            if (!isDelete)
            {
                return Forbid();
            }
            // ah hieu y thang b roi
            // viet rieng de check quyen so huu trc
            // roi xoa sau :))
            // tuong thg b dat ten method vo van :))


            return Ok();
        }
        
        

         
    }
}
