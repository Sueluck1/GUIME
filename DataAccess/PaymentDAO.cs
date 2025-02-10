using Microsoft.EntityFrameworkCore;
using Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DataAccess
{
    public class PaymentDAO : SingletonBase<PaymentDAO>
    {
        public async Task<IEnumerable<Payment>> GetAllPayments()
        {
            return await _context.Payments.ToListAsync();
        }

        public async Task<Payment> GetPaymentById(int id)
        {
            return await _context.Payments.FirstOrDefaultAsync(p => p.PaymentId == id);
        }

        public async Task Add(Payment payment)
        {
            _context.Payments.Add(payment);
            await _context.SaveChangesAsync();
        }

        public async Task Update(Payment payment)
        {
            var existingPayment = await GetPaymentById(payment.PaymentId);
            if (existingPayment != null)
            {
                _context.Entry(existingPayment).CurrentValues.SetValues(payment);
            }
            else
            {
                _context.Payments.Add(payment);
            }
            await _context.SaveChangesAsync();
        }

        public async Task Delete(int id)
        {
            var payment = await GetPaymentById(id);
            if (payment != null)
            {
                _context.Payments.Remove(payment);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<int> GetPaymentCount()
        {
            return await _context.Payments.CountAsync();
        }
    }
}
