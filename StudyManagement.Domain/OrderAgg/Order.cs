using _01_Framework.Domain;

namespace StudyManagement.Domain.OrderAgg
{
    public class Order : EntityBase
    {
        public long AccountId { get; private set; }
        public int TotalAmount { get; private set; }
        public bool IsPayed { get; private set; }
        public string IssueTrackingNo { get; private set; }
        public long RefId { get; private set; }
        public List<OrderItem> Items { get; private set; }

        public Order(long accountId, int totalAmount)
        {
            AccountId = accountId;
            TotalAmount = totalAmount;
            IsPayed = false;
            RefId = 0;
            IssueTrackingNo = "0";
            Items = new List<OrderItem>();
        }


        public void PaymentSucceeded(long refId)
        {
            IsPayed = true;
            if (refId != 0)
            {
                RefId = refId;
            }
        }

        public void SetIssueTrackingNo(string number)
        {
            IssueTrackingNo = number;
        }

        public void AddItem(OrderItem item)
        {
            Items.Add(item);
        }
    }
}
