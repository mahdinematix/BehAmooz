﻿using _01_Framework.Application;
using _01_Framework.Application.Sms;
using StudyManagement.Application.Contracts.Order;
using StudyManagement.Domain.OrderAgg;

namespace StudyManagement.Application
{
    public class OrderApplication:IOrderApplication
    {
        private readonly IAuthHelper _authHelper;
        private readonly IOrderRepository _orderRepository;
        private readonly ISmsService _smsService;

        public OrderApplication(IAuthHelper authHelper, IOrderRepository orderRepository,  ISmsService smsService)
        {
            _authHelper = authHelper;
            _orderRepository = orderRepository;
            _smsService = smsService;
        }

        public long PlaceOrder(Cart cart)
        {
            var currentAccountId = _authHelper.CurrentAccountId();
            var order = new Order(currentAccountId, cart.TotalAmount, cart.PaymentMethod);

            foreach (var cartItem in cart.Items)
            {
                var orderItem = new OrderItem(cartItem.Id,cartItem.SessionPrice,cartItem.SessionNumber,cartItem.ProfessorFullName,cartItem.ClassStartTime,cartItem.ClassEndTime,cartItem.ClassDay,cartItem.CourseName);
                order.AddItem(orderItem);
            }

            _orderRepository.Create(order);
            _orderRepository.Save();
            return order.Id;
        }

        public string PaymentSucceeded(long orderId, long refId)
        {
            var order = _orderRepository.GetBy(orderId);
            order.PaymentSucceeded(refId);
            var issueTrackingNo = CodeGenerator.Generate("S");
            order.SetIssueTrackingNo(issueTrackingNo);
            _orderRepository.Save();
            //var account = _shopAccountAcl.GetAccountNameAndMobileBy(order.AccountId);
            //_smsService.Send(account.mobile,$"{account.name} گرامی سفارش شما با کد پیگیری {issueTrackingNo} ثبت شد و در سریعترین زمان ممکن ارسال خواهد شد.");
            return issueTrackingNo;
        }

        public double GetAmountBy(long id)
        {
            return _orderRepository.GetAmountBy(id);
        }

        public List<OrderViewModel> Search(OrderSearchModel searchModel)
        {
            return _orderRepository.Search(searchModel);
        }

        public void Cancel(long id)
        {
            var order = _orderRepository.GetBy(id);
            order.Cancel();
            _orderRepository.Save();
        }

        public List<OrderItemViewModel> GetItems(long orderId)
        {
            return _orderRepository.GetItems(orderId);
        }
    }
}
