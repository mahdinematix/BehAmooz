using _01_Framework.Application;
using _01_Framework.Infrastructure;
using AccountManagement.Infrastructure.EFCore;
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

        public int GetAmountBy(long id)
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
                CreationDate = x.CreationDate.ToFarsi(),
                IsPayed = x.IsPayed,
                IssueTrackingNo = x.IssueTrackingNo,
                RefId = x.RefId,
                TotalAmount = x.TotalAmount, 
            });


            if (searchModel.AccountId > 0)
                query = query.Where(x => x.AccountId == searchModel.AccountId);

            if (!string.IsNullOrWhiteSpace(searchModel.IssueTrackingNo))
            {
                query = query.Where(x => x.IssueTrackingNo.Contains(searchModel.IssueTrackingNo));
            }

            var orders = query.OrderByDescending(x => x.Id).ToList();
            foreach (var order in orders)
            {
                var fullName = accounts.FirstOrDefault(x => x.Id == order.AccountId)?.FirstName +" "+ accounts.FirstOrDefault(x => x.Id == order.AccountId)?.LastName;
                order.AccountFullname = fullName;
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
                SessionId = x.SessionId,
                ClassDay = x.ClassDay,
                ClassEndTime = x.ClassEndTime,
                ClassStartTime = x.ClassStartTime,
                CourseName = x.CourseName,
                ProfessorFullName = x.ProfessorFullName
            }).ToList();

            foreach (var item in items)
            {
                item.SessionNumber = sessions.FirstOrDefault(x => x.Id == item.SessionId)?.Number;
            }

            return items;
        }
    }
}
