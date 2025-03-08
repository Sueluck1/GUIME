using DataAccess;
using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories
{
    public class CertificateRepository : ICertificateRepository
    {
        // Thêm chứng chỉ
        public async Task Add(Certificate certificate)
        {
            await CertificateDAO.Instance.Add(certificate);
        }

        // Xóa chứng chỉ
        public async Task Delete(int id)
        {
            await CertificateDAO.Instance.Delete(id);
        }

        // Lấy tất cả chứng chỉ
        public async Task<IEnumerable<Certificate>> GetAllCertificates()
        {
            return await CertificateDAO.Instance.GetAllCertificates();
        }

        // Lấy chứng chỉ theo ID
        public async Task<Certificate> GetCertificateById(int id)
        {
            return await CertificateDAO.Instance.GetCertificateById(id);
        }

        // Cập nhật chứng chỉ
        public async Task Update(Certificate certificate)
        {
            await CertificateDAO.Instance.Update(certificate);
        }

        // Lấy chứng chỉ của một người dùng
        public async Task<IEnumerable<Certificate>> GetCertificatesByUserId(int userId)
        {
            return await CertificateDAO.Instance.GetCertificatesByUserId(userId);
        }

        // Đếm số lượng chứng chỉ
        public async Task<int> GetCertificateCount()
        {
            return await CertificateDAO.Instance.GetCertificateCount();
        }
    }
}
