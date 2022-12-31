﻿using AutoMapper;
using Commons;
using Mango.OrderApi.Dtos;
using Mango.OrderApi.Models;
using Mango.OrderApi.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Mango.OrderApi.Services;

public class OrderService : IOrderService
{
    private readonly ApplicationDbContext _dbContext;
    private readonly IMapper _mapper;

    public OrderService(ApplicationDbContext dbContext, IMapper mapper)
    {
        _dbContext = dbContext;
        _mapper = mapper;
    }

    public async Task<Result<object?>> CreateOrder(CheckoutDto checkout)
    {
        var order = _mapper.Map<OrderHeader>(checkout);
        order.OrderTime = DateTime.UtcNow;
        order.PaymentStatus = false;

        _dbContext.OrderHeaders.Add(order);
        await _dbContext.SaveChangesAsync();

        return Result<object>.Success(null);
    }

    public async Task<Result<object?>> CreateOrder(Guid orderHeaderId, bool paid)
    {
        var order = await _dbContext.OrderHeaders.FirstOrDefaultAsync(
            o => o.Id == orderHeaderId);
        if (order == null)
            return Result<object>.Failure("Order not found.");

        order.PaymentStatus = paid;
        await _dbContext.SaveChangesAsync();

        return Result<object>.Success(null);
    }
}