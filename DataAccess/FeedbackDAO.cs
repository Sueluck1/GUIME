using Microsoft.EntityFrameworkCore;
using Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DataAccess
{
    public class FeedbackDAO : SingletonBase<FeedbackDAO>
    {
        public async Task<IEnumerable<Feedback>> GetAllFeedbacks()
        {
            return await _context.Feedbacks.ToListAsync();
        }

        public async Task<Feedback> GetFeedbackById(int id)
        {
            return await _context.Feedbacks.FirstOrDefaultAsync(f => f.FeedbackId == id);
        }

        public async Task Add(Feedback feedback)
        {
            _context.Feedbacks.Add(feedback);
            await _context.SaveChangesAsync();
        }

        public async Task Update(Feedback feedback)
        {
            var existingFeedback = await GetFeedbackById(feedback.FeedbackId);
            if (existingFeedback != null)
            {
                _context.Entry(existingFeedback).CurrentValues.SetValues(feedback);
            }
            else
            {
                _context.Feedbacks.Add(feedback);
            }
            await _context.SaveChangesAsync();
        }

        public async Task Delete(int id)
        {
            var feedback = await GetFeedbackById(id);
            if (feedback != null)
            {
                _context.Feedbacks.Remove(feedback);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<int> GetFeedbackCount()
        {
            return await _context.Feedbacks.CountAsync();
        }
    }
}
