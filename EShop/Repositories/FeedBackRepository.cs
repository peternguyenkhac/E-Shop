using EShop.Data;
using EShop.Models;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using System;

namespace EShop.Repositories
{
    public class FeedBackRepository : RepositoryBase<Feedback>
    {
        private new readonly EShopDBContext _context;
        public FeedBackRepository(EShopDBContext context) : base(context)
        {
            _context = context;
        }

        public async Task<List<Feedback>> GetAllFeedBack()
        {
            return await _context.Feedbacks.ToListAsync();
        }


        public async Task<Feedback> GetFeedBackByIdRepo(int id)
        {
            var feedback = await _context.Feedbacks.FindAsync(id);

            
            return feedback;
        }


        public async Task<Feedback> DeleteFeedBackByUserRepo(int id)
        {

            var deletecmt = await _context.Feedbacks.FindAsync(id);
            
            if(deletecmt != null)
            {
                _context.Feedbacks.Remove(deletecmt);
            }

            return deletecmt; // ke me no, khi nao bao do moi la loi :))

        }
    }


}
