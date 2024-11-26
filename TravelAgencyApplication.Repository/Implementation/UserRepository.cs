﻿using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TravelAgencyApplication.Domain.Identity;
using TravelAgencyApplication.Domain.Model;
using TravelAgencyApplication.Repository.Interface;
using TravelAgencyApplication.Web.Data;

namespace TravelAgencyApplication.Repository.Implementation
{
    public class UserRepository : IUserRepository
    {
        private readonly ApplicationDbContext context;
        private DbSet<TAUser> entities;
        string errorMessage = string.Empty;

        public UserRepository(ApplicationDbContext context)
        {
            this.context = context;
            entities = context.Set<TAUser>();
        }
        public IEnumerable<TAUser> GetAll()
        {
            return entities
                .Include(t => t.Reservations)
                .Include("Reservations.TravelPackage")
                .ToList();
        }

        public TAUser Get(string id)
        {
            return entities
                .Include(t => t.Reservations)
                .Include("Reservations.TravelPackage")
                .SingleOrDefault(s => s.Id == id);
        }
        public void Insert(TAUser entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException("entity");
            }
            entities.Add(entity);
            context.SaveChanges();
        }

        public void Update(TAUser entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException(nameof(entity));
            }

            var existingEntity = context.Users.Find(entity.Id);
            if (existingEntity == null)
            {
                throw new InvalidOperationException("Entity not found");
            }

            context.Entry(existingEntity).CurrentValues.SetValues(entity);

            context.SaveChanges();
        }

        public void Delete(TAUser entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException("entity");
            }
            entities.Remove(entity);
            context.SaveChanges();
        }
        public bool TAUserExists(string userId)
        {
            return entities.Any(e => e.Id == userId);
        }

        public IEnumerable<TAUser> GetAllGuides()
        {
            return entities.Include(t => t.Reservations).Where(t => t.UserRole == Domain.Enum.UserRole.TRAVEL_GUIDE).ToList();
        }
    }
}
