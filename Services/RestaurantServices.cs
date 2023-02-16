using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using RestaurantAPI.Authorization;
using RestaurantAPI.Entities;
using RestaurantAPI.Exceptions;
using RestaurantAPI.Models;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Security.Claims;

namespace RestaurantAPI.Services
{
    public class RestaurantServices : IRestaurantServices
    {
        private readonly RestaurantDbContext _db;
        private readonly IMapper _mapper;
        private readonly ILogger _logger;
        private readonly IAuthorizationService _authorizationService;

        public IUserContextService _UserContextService { get; }

        public RestaurantServices(RestaurantDbContext db, IMapper mapper, ILogger<RestaurantServices> logger, IAuthorizationService authorizationService,IUserContextService userContextService)
        {
        _db = db;
        _mapper = mapper;
        _logger = logger;
        _authorizationService = authorizationService;
        _UserContextService = userContextService;
        }

        public PageResoult<RestaurantDTO> GetAll(RestaurantQuery query)
        {
            var baseQuery = _db.Restaurants
            .Include("address")
            .Include("Dishes")
            .Where( r => query.SearchPhrase == null || r.name.ToLower().Contains(query.SearchPhrase.ToLower()) || r.description.ToLower().Contains(query.SearchPhrase.ToLower()));

            if(!string.IsNullOrEmpty(query.SortBy))
            {
                
                var columnsSelectors = new Dictionary<string, Expression<Func<Restaurant,object>>>
                {
                    {nameof(Restaurant.name), r => r.name},
                    {nameof(Restaurant.description), r => r.description},
                    {nameof(Restaurant.Category), r => r.Category}
                };

                var selectedColumn = columnsSelectors[query.SortBy];

               baseQuery = query.SortDirection == SortDirection.Asc ? baseQuery.OrderBy(selectedColumn) : baseQuery.OrderByDescending(selectedColumn);
            }

            var restaurants = baseQuery
            .Skip(query.PageSize * (query.PageNumber - 1))
            .Take(query.PageSize)
            .ToList();

            var totalItemsCount = baseQuery.Count();

            var restaurantsDTO = _mapper.Map<List<RestaurantDTO>>(restaurants);


            var resoult = new PageResoult<RestaurantDTO>(restaurantsDTO,totalItemsCount,query.PageSize,query.PageNumber);


            return resoult;
        }

        public RestaurantDTO GetById(int id)
        {
            var restaurant = _db.Restaurants.Include("address").Include("Dishes").FirstOrDefault(u => u.id == id);

            if (restaurant == null) return null;

            var restaurantDTO = _mapper.Map<RestaurantDTO>(restaurant);

            return restaurantDTO;

        }

        public int Create(CreateRestaurantDBO dto)
        {
            var restaurant = _mapper.Map<Restaurant>(dto);
            restaurant.CreatedById = _UserContextService.GetUserId;
            _db.Restaurants.Add(restaurant);
            _db.SaveChanges();
            return restaurant.id;

        }
        public void Delete(int id)
        {
            _logger.LogError($"Restaurant with id: {id} DELETE action invkoed");
            var restaurant = _db.Restaurants.FirstOrDefault(u=>u.id==id);
            if(restaurant == null)
            {
                throw new NotFoundException("Restaurant not found");
            }

            var authorizationResoult = _authorizationService.AuthorizeAsync(_UserContextService.User,restaurant, new ResourceOperationRequirement(ResourceOperation.Delete)).Result;

            if(!authorizationResoult.Succeeded)
            {
                throw new ForbidException("You haven't permissions to this restaurant");
            }

            _db.Restaurants.Remove(restaurant);
            _db.SaveChanges();

        }

        public void Update(int id,UpdateRestaurantDTO dto)
        {
            var restaurant = _db.Restaurants.FirstOrDefault(u=>u.id==id);
            
            if(restaurant == null)
            {
                throw new NotFoundException("Restaurant not found");
            }

            var authorizationResoult = _authorizationService.AuthorizeAsync(_UserContextService.User,restaurant, new ResourceOperationRequirement(ResourceOperation.Update)).Result;

            if(!authorizationResoult.Succeeded)
            {
                throw new ForbidException("You haven't permissions to this restaurant");
            }

            restaurant.name = dto.name;
            restaurant.description = dto.description;
            restaurant.hasDelivery = dto.hasDelivery;

            _db.SaveChanges();

        }


    }
}
