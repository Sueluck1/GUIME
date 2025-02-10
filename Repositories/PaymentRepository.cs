using DataAccess;
using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories
{
    public class PaymentRepository : IPaymentRepository
    {
        public async Task Add(Payment payment)
        {
            await PaymentDAO.Instance.Add(payment);
        }

        public async Task Delete(int id)
        {
            await PaymentDAO.Instance.Delete(id);
        }

        public async Task<IEnumerable<Payment>> GetAllPayments()
        {
            return await PaymentDAO.Instance.GetAllPayments();
        }

        public async Task<Payment> GetPaymentById(int id)
        {
            return await PaymentDAO.Instance.GetPaymentById(id);
        }

        public async Task Update(Payment payment)
        {
            await PaymentDAO.Instance.Update(payment);
        }

        public async Task<int> GetPaymentCount()
        {
            return await PaymentDAO.Instance.GetPaymentCount();
        }
    }

}
