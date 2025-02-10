using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories
{
    public interface IPaymentRepository
    {
        Task<IEnumerable<Payment>> GetAllPayments();
        Task<Payment> GetPaymentById(int id);
        Task Add(Payment payment);
        Task Update(Payment payment);
        Task Delete(int id);
        Task<int> GetPaymentCount();
    }
}
