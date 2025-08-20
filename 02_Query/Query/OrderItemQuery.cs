using _01_Framework.Application;
using _02_Query.Contracts.Order;
using StudyManagement.Infrastructure.EFCore;

namespace _02_Query.Query
{
    public class OrderItemQuery : IOrderItemQuery
    {
        private readonly StudyContext _studyContext;
        private readonly IAuthHelper _authHelper;

        public OrderItemQuery(StudyContext studyContext, IAuthHelper authHelper)
        {
            _studyContext = studyContext;
            _authHelper = authHelper;
        }

        public List<OrderItemQueryModel> GetItemsThatPaid()
        {
            var orders = _studyContext.Orders.Where(x => x.IsPayed).Where(x => x.AccountId == _authHelper.CurrentAccountId()).ToList();
            var result = new List<OrderItemQueryModel>();
            foreach (var order in orders)
            {
                foreach (var orderItem in order.Items)
                {
                    result.Add(new OrderItemQueryModel
                    {
                        SessionId = orderItem.SessionId,
                        ClassDay = orderItem.ClassDay,
                        ClassEndTime = orderItem.ClassEndTime,
                        ClassStartTime = orderItem.ClassStartTime,
                        CourseName = orderItem.CourseName,
                        SessionNumber = orderItem.SessionNumber,
                        ProfessorFullName = orderItem.ProfessorFullName,
                        OrderId = orderItem.OrderId,
                        PayDate = orderItem.CreationDate.ToFarsi()
                    });
                }
            }
            return result;
        }

        public bool IsPaid(long sessionId)
        {
            var orders = _studyContext.Orders.Where(x => x.IsPayed).Where(x => x.AccountId == _authHelper.CurrentAccountId()).ToList();
            bool isPaid = false;
            foreach (var order in orders)
            {
                foreach (var orderItem in order.Items)
                {
                    if (orderItem.SessionId == sessionId)
                    {
                        if (orderItem.Order.IsPayed)
                        {
                            isPaid = true;
                        }
                    }
                }
            }
            return isPaid;
        }
    }
}
