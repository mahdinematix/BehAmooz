using _01_Framework.Application;
using _01_Framework.Application.Sms;
using AccountManagement.Application.Contract.Wallet;
using StudyManagement.Application.Contracts.Order;
using StudyManagement.Domain.OrderAgg;

namespace StudyManagement.Application
{
    public class OrderApplication:IOrderApplication
    {
        private readonly IAuthHelper _authHelper;
        private readonly IOrderRepository _orderRepository;
        private readonly ISmsService _smsService;
        private readonly IWalletApplication _walletApplication;

        public OrderApplication(IAuthHelper authHelper, IOrderRepository orderRepository,  ISmsService smsService, IWalletApplication walletApplication)
        {
            _authHelper = authHelper;
            _orderRepository = orderRepository;
            _smsService = smsService;
            _walletApplication = walletApplication;
        }

        public long PlaceOrder(Cart cart)
        {
            var currentAccountId = _authHelper.CurrentAccountId();
            var order = new Order(currentAccountId, cart.TotalAmount);

            foreach (var cartItem in cart.Items)
            {
                var orderItem = new OrderItem(cartItem.Id,cartItem.SessionPrice,cartItem.SessionNumber,cartItem.ProfessorId,cartItem.ProfessorFullName,cartItem.ClassStartTime,cartItem.ClassEndTime,cartItem.ClassDay,cartItem.ClassId,cartItem.CourseName);
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
            foreach (var orderItem in order.Items)
            {
                _walletApplication.PayToProfessor(orderItem.SessionPrice,orderItem.ProfessorId);
            }
            _orderRepository.Save();
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

        public List<OrderItemViewModel> GetItems(long orderId)
        {
            return _orderRepository.GetItems(orderId);
        }

        
    }
}
