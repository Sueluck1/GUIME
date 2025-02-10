using DataAccess;
using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories
{
    public class TourRepository : ITourRepository
    {
        public async Task Add(Tour tour)
        {
            await TourDAO.Instance.Add(tour);
        }

        public async Task Delete(int id)
        {
            await TourDAO.Instance.Delete(id);
        }

        public async Task<IEnumerable<Tour>> GetAllTours()
        {
            return await TourDAO.Instance.GetAllTours();
        }

        public async Task<Tour> GetTourById(int id)
        {
            return await TourDAO.Instance.GetTourById(id);
        }

        public async Task Update(Tour tour)
        {
            await TourDAO.Instance.Update(tour);
        }

        public async Task<int> GetTourCount()
        {
            return await TourDAO.Instance.GetTourCount();
        }

		///Fixed Tour
		public async Task<IEnumerable<Tour>> GetFixedTours()
		{
			var tours = await TourDAO.Instance.GetAllTours();
			return tours.Where(t => t.Type == Tour.TourType.Fixed).ToList(); 
		}

		public async Task AddFixedTour(Tour tour)
		{
			tour.Type = Tour.TourType.Fixed; // Đảm bảo đây là Fixed Tour
			await TourDAO.Instance.Add(tour);
		}

		public async Task UpdateFixedTour(Tour tour)
		{
			var existingTour = await TourDAO.Instance.GetTourById(tour.TourId);
			if (existingTour != null && existingTour.Type == Tour.TourType.Fixed)
			{
				existingTour.Name = tour.Name;
				existingTour.Description = tour.Description;
				existingTour.StartDate = tour.StartDate;
				existingTour.EndDate = tour.EndDate;
				existingTour.Price = tour.Price; // Giá chỉ áp dụng cho Fixed Tour
				await TourDAO.Instance.Update(existingTour);
			}
		}

		public async Task DeleteFixedTour(int id)
		{
			var tour = await TourDAO.Instance.GetTourById(id);
			if (tour != null && tour.Type == Tour.TourType.Fixed)
			{
				await TourDAO.Instance.Delete(id);
			}
		}

	}
}
