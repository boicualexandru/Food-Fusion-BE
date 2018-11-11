﻿using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using DataAccess.Models;
using Microsoft.EntityFrameworkCore;
using Services.Authentication.Exceptions;
using Services.Employees.Exceptions;
using Services.Employees.Models;
using Services.Restaurants.Exceptions;

namespace Services.Employees
{
    public class EmployeesService : IEmployeesService
    {
        private readonly FoodFusionContext _dbContext;
        private readonly IMapper _mapper;

        public EmployeesService(FoodFusionContext dbContext, IMapper mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }

        public IList<EmployeeModel> GetEmployees(int restaurantId)
        {
            var restaurant = _dbContext.Restaurants
                .Include(r => r.RestaurantEmployees)
                    .ThenInclude(re => re.User)
                .FirstOrDefault(r => r.Id == restaurantId);
            restaurant = restaurant ?? throw new RestaurantNotFoundException();

            var employees = restaurant.RestaurantEmployees
                .Select(re => re.User);

            return _mapper.Map<IList<EmployeeModel>>(employees);
        }

        public void AddEmployee(int restaurantId, int userId)
        {
            var restaurantExists = _dbContext.Restaurants
                .Any(r => r.Id == restaurantId);
            if (!restaurantExists) throw new RestaurantNotFoundException();

            var userExists = _dbContext.Users
                .Any(u => u.Id == userId);
            if (!userExists) throw new UserNotFoundException();

            var employeExists = _dbContext.RestaurantEmployees
                .Any(re => re.RestaurantId == restaurantId && re.UserId == userId);
            if (employeExists) throw new EmployeeAlreadyExistsException();

            var employee = new RestaurantEmployee
            {
                RestaurantId = restaurantId,
                UserId = userId
            };

            _dbContext.RestaurantEmployees.Add(employee);
            _dbContext.SaveChanges();
        }

        public void RemoveEmployee(int restaurantId, int userId)
        {
            var employe = _dbContext.RestaurantEmployees
                   .FirstOrDefault(re => re.RestaurantId == restaurantId && re.UserId == userId);
            employe = employe ?? throw new EmployeeNotFoundException();

            _dbContext.RestaurantEmployees.Remove(employe);
            _dbContext.SaveChanges();
        }

        public EmployeeModel GetManager(int restaurantId)
        {
            var restaurant = _dbContext.Restaurants
                .Include(r => r.Manager)
                .FirstOrDefault(r => r.Id == restaurantId);
            restaurant = restaurant ?? throw new RestaurantNotFoundException();
            restaurant.Manager = restaurant.Manager ?? throw new ManagerNotFoundException();

            return _mapper.Map<EmployeeModel>(restaurant.Manager);
        }

        public void AddOrReplaceManager(int restaurantId, int userId)
        {
            var userExists = _dbContext.Users
                .Any(u => u.Id == userId);
            if (!userExists) throw new UserNotFoundException();

            var restaurant = _dbContext.Restaurants
                .FirstOrDefault(r => r.Id == restaurantId);
            restaurant = restaurant ?? throw new RestaurantNotFoundException();

            restaurant.ManagerId = userId;
            _dbContext.SaveChanges();
        }
    }
}
