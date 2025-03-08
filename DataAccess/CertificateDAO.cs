using Microsoft.EntityFrameworkCore;
using Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DataAccess
{
    public class CertificateDAO : SingletonBase<CertificateDAO>
    {
        // Lấy tất cả chứng chỉ
        public async Task<IEnumerable<Certificate>> GetAllCertificates()
        {
            return await _context.Certificates.ToListAsync();
        }

        // Lấy chứng chỉ theo ID
        public async Task<Certificate> GetCertificateById(int id)
        {
            return await _context.Certificates.FirstOrDefaultAsync(c => c.CertificateId == id);
        }

        // Thêm chứng chỉ mới
        public async Task Add(Certificate certificate)
        {
            _context.Certificates.Add(certificate);
            await _context.SaveChangesAsync();
        }

        // Cập nhật chứng chỉ
        public async Task Update(Certificate certificate)
        {
            var existingCertificate = await GetCertificateById(certificate.CertificateId);
            if (existingCertificate != null)
            {
                _context.Entry(existingCertificate).CurrentValues.SetValues(certificate);
            }
            else
            {
                _context.Certificates.Add(certificate);
            }
            await _context.SaveChangesAsync();
        }

        // Xóa chứng chỉ
        public async Task Delete(int id)
        {
            var certificate = await GetCertificateById(id);
            if (certificate != null)
            {
                _context.Certificates.Remove(certificate);
                await _context.SaveChangesAsync();
            }
        }

        // Lấy tất cả chứng chỉ của một user
        public async Task<IEnumerable<Certificate>> GetCertificatesByUserId(int userId)
        {
            return await _context.Certificates
                .Where(c => c.UserId == userId)
                .ToListAsync();
        }

        // Đếm số lượng chứng chỉ
        public async Task<int> GetCertificateCount()
        {
            return await _context.Certificates.CountAsync();
        }
    }
}
