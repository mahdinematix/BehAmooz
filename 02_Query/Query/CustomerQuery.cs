﻿using _02_Query.Contracts.Customer;
using AccountManagement.Infrastructure.EFCore;
using Microsoft.EntityFrameworkCore;
using StudyManagement.Infrastructure.EFCore;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace _02_Query.Query
{
    public class CustomerQuery : ICustomerQuery
    {
        private readonly StudyContext _studyContext;
        private readonly AccountContext _accountContext;

        public CustomerQuery(StudyContext studyContext, AccountContext accountContext)
        {
            _studyContext = studyContext;
            _accountContext = accountContext;
        }

        public List<CustomerQueryModel> GetCustomersByProfessorId(CustomerSearchModel searchModel, long professorId)
        {
            // گرفتن کلاس‌های استاد
            var classes = _studyContext.Classes
                .Where(c => c.ProfessorId == professorId && c.IsActive)
                .Include(c => c.Course)
                .ToList();


            // گرفتن سفارشاتی که شامل کلاس‌های استاد هستند و پرداخت شده‌اند
            var ordersWithItems = _studyContext.Orders
                .Where(o => o.IsPayed)
                .Include(o => o.Items)
                .ToList();

            var orderItemsForClasses = ordersWithItems
                .SelectMany(o => o.Items)
                .Where(i => classes.Any(c => c.Id == i.ClassId))
                .ToList();

            // گرفتن شناسه دانشجویان
            var accountIds = orderItemsForClasses
                .Select(i => i.Order.AccountId)
                .ToList();

            var accounts = _accountContext.Accounts
                .Where(a => accountIds.Contains(a.Id))
                .ToList();

            // ساخت مدل ویو
            var customers = new List<CustomerQueryModel>();

            var groupedItems = orderItemsForClasses
                .GroupBy(i => new { i.Order.AccountId, i.ClassId })
                .ToList();

            foreach (var group in groupedItems)
            {
                var account = accounts.FirstOrDefault(a => a.Id == group.Key.AccountId);
                var classInfo = classes.FirstOrDefault(c => c.Id == group.Key.ClassId);

                if (account == null || classInfo == null)
                    continue;


                var sessionCounts = new Dictionary<int, int>();
                for (int i = 1; i <= 16; i++)
                {
                    // مجموع تعداد جلسات هر شماره جلسه برای آن دانشجو و کلاس
                    sessionCounts[i] = group.Count(it => it.SessionNumber == i);
                }

                int totalSessions = sessionCounts.Values.Sum();
                int sessionPrice = group.FirstOrDefault()?.SessionPrice ?? 0;
                int totalAmount = totalSessions * sessionPrice;

                customers.Add(new CustomerQueryModel
                {
                    AccountId = account.Id,
                    FullName = $"{account.FirstName} {account.LastName}",
                    Code = account.Code,
                    UniversityId = account.UniversityId,
                    CourseName = classInfo.Course.Name,
                    ClassCode = classInfo.Code,
                    ClassDay = classInfo.Day,
                    ClassStartTime = classInfo.StartTime,
                    ClassEndTime = classInfo.EndTime,
                    SessionCounts = sessionCounts,
                    TotalSessions = totalSessions,
                    SessionPrice = sessionPrice,
                    TotalAmount = totalAmount
                });
            }

            if (!string.IsNullOrWhiteSpace(searchModel.FullName))
            {
                customers = customers.Where(x => x.FullName.Contains(searchModel.FullName)).ToList();
            }

            if (!string.IsNullOrWhiteSpace(searchModel.ClassCode))
            {
                customers = customers.Where(x => x.ClassCode.Contains(searchModel.ClassCode)).ToList();
            }

            if (!string.IsNullOrWhiteSpace(searchModel.Code))
            {
                customers = customers.Where(x => x.Code.Contains(searchModel.Code)).ToList();
            }

            if (!string.IsNullOrWhiteSpace(searchModel.CourseName))
            {
                customers = customers.Where(x => x.CourseName.Contains(searchModel.CourseName)).ToList();
            }

            if (searchModel.ClassStartTime != "0" && searchModel.ClassStartTime != null)
            {
                customers = customers.Where(x => x.ClassStartTime == searchModel.ClassStartTime).ToList();
            }

            if (searchModel.ClassDay > 0)
            {
                customers = customers.Where(x => x.ClassDay == searchModel.ClassDay).ToList();
            }

            if (searchModel.UniversityId > 0)
            {
                customers = customers.Where(x => x.UniversityId == searchModel.UniversityId).ToList();
            }


            return customers;
        }
    }
}
