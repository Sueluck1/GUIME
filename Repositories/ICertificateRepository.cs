using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories
{
    public interface ICertificateRepository
    {
        Task Add(Certificate certificate);
        Task Delete(int id);
        Task<IEnumerable<Certificate>> GetAllCertificates();
        Task<Certificate> GetCertificateById(int id);
        Task Update(Certificate certificate);
        Task<IEnumerable<Certificate>> GetCertificatesByUserId(int userId);
        Task<int> GetCertificateCount();
    }
}
