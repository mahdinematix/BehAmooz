﻿namespace StudyManagement.Application.Contracts.Order
{
    public interface ICartService
    {
        Cart Get();
        void Set(Cart cart);
    }
}
