using _01_Framework.Application;
using _01_Framework.Infrastructure;
using AccountManagement.Infrastructure.EFCore;
using StudyManagement.Application.Contracts;
using StudyManagement.Application.Contracts.Order;
using StudyManagement.Domain.OrderAgg;

namespace StudyManagement.Infrastructure.EFCore.Repository
{
    public class OrderRepository : RepositoryBase<long,Order> , IOrderRepository
    {
        private readonly StudyContext _context;
        private readonly AccountContext _accountContext; 

        public OrderRepository(StudyContext context, AccountContext accountContext) : base(context)
        {
            _context = context;
            _accountContext = accountContext;
        }

        public double GetAmountBy(long id)
        {
            var result = _context.Orders.Select(x => new {x.TotalAmount, x.Id}).FirstOrDefault(x => x.Id == id);
            if (result != null)
            {
                return result.TotalAmount;
            }
            return 0;
        }

        public List<OrderViewModel> Search(OrderSearchModel searchModel)
        {
            var accounts = _accountContext.Accounts.Select(x=> new {x.Id,x.FirstName,x.LastName});
            var query = _context.Orders.Select(x => new OrderViewModel
            {
                Id = x.Id,
                AccountId = x.AccountId,
                IsCanceled = x.IsCanceled,
                CreationDate = x.CreationDate.ToFarsi(),
                IsPayed = x.IsPayed,
                IssueTrackingNo = x.IssueTrackingNo,
                PaymentMethodId = x.PaymentMethod,
                RefId = x.RefId,
                TotalAmount = x.TotalAmount, 
            });

            query = query.Where(x => x.IsCanceled == searchModel.IsCanceled);

            if (searchModel.AccountId > 0)
                query = query.Where(x => x.AccountId == searchModel.AccountId);

            if (!string.IsNullOrWhiteSpace(searchModel.IssueTrackingNo))
            {
                query = query.Where(x => x.IssueTrackingNo.Contains(searchModel.IssueTrackingNo));
            }

            var orders = query.OrderByDescending(x => x.Id).ToList();
            foreach (var order in orders)
            {
                order.AccountFullname = accounts.FirstOrDefault(x => x.Id == order.AccountId)?.LastName;
                order.PaymentMethod = PaymentMethod.GetBy(order.PaymentMethodId).Name;
            }

            return orders;
        }

        public List<OrderItemViewModel> GetItems(long orderId)
        {
            var sessions = _context.Sessions.Select(x => new { x.Id, x.Number }).ToList();
            var order = _context.Orders.FirstOrDefault(x => x.Id == orderId);
            if (order == null)
                return new List<OrderItemViewModel>();

            var items = order.Items.Select(x => new OrderItemViewModel
            {
                Id = x.Id,
                OrderId = x.OrderId,
                UnitPrice = x.SessionPrice,
                SessionId = x.SessionId
            }).ToList();

            foreach (var item in items)
            {
                item.Session = sessions.FirstOrDefault(x => x.Id == item.SessionId)?.Number;
            }

            return items;
        }
    }
}
