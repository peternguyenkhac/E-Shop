using EShop.Models;
using EShop.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Net.WebSockets;
using System.Security.Claims;
using System.Threading.Tasks;

namespace EShop.Services
{

    public class FeedBackService 
    {
        private readonly OrderRepository _orderRepository;
        private readonly CartRepository _cartRepository;
        private readonly ProductRepository _productRepository;
        private readonly FeedBackRepository _feedbackRepository;
        private readonly UserService _userService;


        public FeedBackService(OrderRepository orderRepository, CartRepository cartRepository, ProductRepository productRepository, FeedBackRepository feedbackRepository, UserService userService)
        {
            _orderRepository = orderRepository;
            _cartRepository = cartRepository;
            _productRepository = productRepository;
            _feedbackRepository = feedbackRepository;
            _userService = userService;
        }

        public async Task<List<Feedback>> GetComments()
        {
            var comments = await _feedbackRepository.GetAllFeedBack();
            return comments;
        }

        public async Task<Feedback> GetFeedBackByIdService(int id)
        {
            var comment = await _feedbackRepository.GetFeedBackByIdRepo(id);
            return comment;
        }

        public async Task<bool> IsDeleteFeedBackByUserSerVice(int id, ClaimsPrincipal principal)
        {


            /*var comment = await _feedbackRepository.DeleteFeedBackByUserRepo(id);

            if (comment == null)
            {
                return false;
            }

            var UserId = await _userService.GetUserId(principal); // service trong service no co the bi loi

            return comment.UserId == UserId;*/

            // logic:
            // lay ra comment
            // kiem tra user Id cua cmt
            // trung voi user hien tai thi cho phep xoa

            var comment = await _feedbackRepository.GetSingleById(id);

            if(comment == null)
            {
                return false;
            }

            //nhung ma cu thu
            var UserId = await _userService.GetUserId(principal);

            if(comment.UserId == UserId)
            {
                await _feedbackRepository.Delete(id); // coi nhu xoa auto thanh cong, ben tren da kiem tra null r
                return true;
            }

            // nhieu cho van chua toi uu, nhu the nay thi no tuong duong lay ra 2 lan 1 cai cmt
            // co the viet rieng 1 method nhan dau vao la cai comment va xoa no di

            return false;

        }

    }
}
