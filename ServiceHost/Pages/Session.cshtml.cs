using _01_Framework.Application;
using _01_Framework.Infrastructure;
using _02_Query.Contracts.Class;
using _02_Query.Contracts.Course;
using _02_Query.Contracts.Order;
using _02_Query.Contracts.Session;
using Microsoft.AspNetCore.Mvc;
using StudyManagement.Application.Contracts.SessionVideoView;

namespace ServiceHost.Pages
{
    public class SessionModel : UserContextPageModel
    {
        private readonly ISessionQuery _sessionQuery;
        ISessionVideoViewApplication _videoViewApplication;
        private readonly IClassQuery _classQuery;
        private readonly IOrderQuery _orderQuery;
        public SessionQueryModel Session;
        public ClassQueryModel Class;
        public CourseQueryModel Course;
        public bool ShowSessionDetailsIfPaid { get; set; }
        public bool IsPaid;
        public bool ShowVideoPlayer { get; set; }
        public bool CanWatchVideo { get; set; }
        public string VideoMessage { get; set; }
        public int RemainingVideoCount { get; set; }


        public SessionModel(IAuthHelper authHelper, ISessionQuery sessionQuery, IClassQuery classQuery, IOrderQuery orderQuery, ISessionVideoViewApplication videoViewApplication):base(authHelper)
        {
            _sessionQuery = sessionQuery;
            _classQuery = classQuery;
            _orderQuery = orderQuery;
            _videoViewApplication = videoViewApplication;
        }

        public IActionResult OnGet(long sessionId)
        {
            if (!IsAuthenticated)
            {
                return RedirectToPage("/Login");
            }

            if (CurrentAccountRole == Roles.Professor)
            {
                return RedirectToPage("/Index", new { area = "Administration" });
            }

            if (CurrentAccountStatus == Statuses.Waiting)
            {
                return RedirectToPage("/NotConfirmed");
            }

            if (CurrentAccountStatus == Statuses.Rejected)
            {
                return RedirectToPage("/Reject");
            }
            
            IsPaid = _orderQuery.IsPaid(sessionId);
            Session = _sessionQuery.GetSessionById(sessionId);
            Class = _classQuery.GetClassById(Session.ClassId);
            Course = _classQuery.GetCourseNameAndPriceByClassId(Class.CourseId);
            ShowSessionDetailsIfPaid = IsPaid && CurrentAccountRole == Roles.Student;
            ShowVideoPlayer = false;
            CanWatchVideo = false;
            VideoMessage = "";
            RemainingVideoCount = _videoViewApplication.GetRemainingCount(sessionId,CurrentAccountId);

            return Page();
        }
        public IActionResult OnPostShowVideo(long sessionId)
        {
            if (!IsAuthenticated)
                return RedirectToPage("/Login");

            if (CurrentAccountRole == Roles.Professor)
                return RedirectToPage("/Index", new { area = "Administration" });

            if (CurrentAccountStatus == Statuses.Waiting)
                return RedirectToPage("/NotConfirmed");

            if (CurrentAccountStatus == Statuses.Rejected)
                return RedirectToPage("/Reject");
            if (CurrentAccountRole==Roles.Student)
            {
                IsPaid = _orderQuery.IsPaid(sessionId);
                Session = _sessionQuery.GetSessionById(sessionId);
                Class = _classQuery.GetClassById(Session.ClassId);
                Course = _classQuery.GetCourseNameAndPriceByClassId(Class.CourseId);
                ShowSessionDetailsIfPaid = IsPaid && CurrentAccountRole == Roles.Student;

                if (!ShowSessionDetailsIfPaid)
                    return Page();

                var result = _videoViewApplication.TryWatch(sessionId,CurrentAccountId);

                RemainingVideoCount = result.RemainingCount;

                if (!result.IsSucceeded)
                {
                    ShowVideoPlayer = false;
                    VideoMessage = result.Message;
                    return Page();
                }
                ShowVideoPlayer = true;
            }


            return Page();
        }

    }
}
