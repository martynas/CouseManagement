using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;

namespace Atrendia.CourseManagement.Frontend.Calendar
{
    public partial class Default : Support.BasePage
    {
        enum CalendarViewMode
        {
            Past,
            Agenda,
            Month,
            Week,
            Day
        };

        private CalendarViewMode? _viewMode;
        /// <summary>
        /// Current view mode.
        /// </summary>
        CalendarViewMode ViewMode
        {
            get
            {
                if (!_viewMode.HasValue)
                {
                    switch (Request["mode"])
                    {
                        case "month":
                            _viewMode = CalendarViewMode.Month;
                            break;
                        case "week":
                            _viewMode = CalendarViewMode.Week;
                            break;
                        case "day":
                            _viewMode = CalendarViewMode.Day;
                            break;
                        case "past":
                            _viewMode = CalendarViewMode.Past;
                            break;
                        case "agenda":
                        default:
                            _viewMode = CalendarViewMode.Agenda;
                            break;
                    }
                }
                return _viewMode.Value;
            }
        }

        private DateTime? _startDate;
        /// <summary>
        /// Entry start date.
        /// </summary>
        private DateTime StartDate
        {
            get
            {
                if (!_startDate.HasValue)
                {
                    DateTime value;
                    try
                    {
                        value = DateTime.ParseExact(Request["start"], "yyyy-MM-dd", null);
                    }
                    catch
                    {
                        value = DateTime.Now.Date;
                    }
                    switch (ViewMode)
                    {
                        case CalendarViewMode.Past:
                            value = DateTime.Now.AddYears(-3);
                            break;
                        case CalendarViewMode.Agenda:
                            value = DateTime.Now.Date;
                            break;
                        case CalendarViewMode.Month:
                            // First day of month
                            value = value.AddDays(-value.Day + 1);
                            break;
                        case CalendarViewMode.Week:
                            // Monday
                            while (value.DayOfWeek != DayOfWeek.Monday)
                            {
                                value = value.AddDays(-1);
                            }
                            break;
                    }
                    _startDate = value;
                }
                return _startDate.Value;
            }
        }

        /// <summary>
        /// End date of interval.
        /// </summary>
        private DateTime? EndDate
        {
            get
            {
                switch (ViewMode)
                {
                    case CalendarViewMode.Past:
                        return DateTime.Now.Date;
                    case CalendarViewMode.Day:
                        return StartDate;
                    case CalendarViewMode.Week:
                        return StartDate.AddDays(6);
                    case CalendarViewMode.Month:
                        return StartDate.AddMonths(1).AddDays(-1);
                    default:
                        return null;
                }
            }
        }

        /// <summary>
        /// Date for next view.
        /// </summary>
        private DateTime? NextDate
        {
            get
            {
                switch (ViewMode)
                {
                    case CalendarViewMode.Day:
                        return StartDate.AddDays(1);
                    case CalendarViewMode.Week:
                        return StartDate.AddDays(7);
                    case CalendarViewMode.Month:
                        return StartDate.AddMonths(1);
                    default:
                        return null;
                }
            }
        }

        /// <summary>
        /// Date for previous view.
        /// </summary>
        private DateTime? PrevDate
        {
            get
            {
                switch (ViewMode)
                {
                    case CalendarViewMode.Day:
                        return StartDate.AddDays(-1);
                    case CalendarViewMode.Week:
                        return StartDate.AddDays(-7);
                    case CalendarViewMode.Month:
                        return StartDate.AddMonths(-1);
                    default:
                        return null;
                }
            }
        }

        /// <summary>
        /// Highest number of records to show.
        /// </summary>
        private int? MaxRecords
        {
            get
            {
                if (ViewMode == CalendarViewMode.Agenda)
                {
                    return 50;
                }
                else
                {
                    return null;
                }
            }
        }

        private string FormatUrl(CalendarViewMode mode, DateTime date)
        {
            return ResolveUrl(String.Format(
                "~/Calendar/Default.aspx?mode={0}&start={1}",
                mode.ToString().ToLower(),
                date.ToString("yyyy-MM-dd")));
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            List<Logic.Entities.Activity> activities;
            activities = Endpoint.GetActivities(CurrentContact.Id,
                MaxRecords, StartDate, EndDate);
            if (activities.Count > 0)
            {
                rptrActivities.DataSource = activities;
                rptrActivities.DataBind();
            }
            else
            {
                pnlNoActivities.Visible = true;
            }
        }

        protected void Page_PreRender(object sender, EventArgs e)
        {
            if (NextDate.HasValue)
            {
                hlNext.NavigateUrl = FormatUrl(ViewMode, NextDate.Value);
            }
            else
            {
                hlNext.Visible = false;
            }
            if (PrevDate.HasValue)
            {
                hlPrev.NavigateUrl = FormatUrl(ViewMode, PrevDate.Value);
            }
            else
            {
                hlPrev.Visible = false;
            }
            switch (ViewMode)
            {
                case CalendarViewMode.Agenda:
                    pPastContainer.Visible = true;
                    lblRange.Text = "Agenda";
                    hlAgendaView.CssClass += " active";
                    break;
                case CalendarViewMode.Past:
                    lblRange.Text = "Agenda";
                    hlAgendaView.CssClass += " active";
                    break;
                case CalendarViewMode.Month:
                    lblRange.Text = StartDate.ToString("MMMM, yyyy");
                    hlMonthView.CssClass += " active";
                    break;
                case CalendarViewMode.Week:
                    lblRange.Text = String.Format("{0} - {1}",
                        StartDate.ToString("dd MMM"),
                        EndDate.Value.ToString("dd MMM"));
                    hlWeekView.CssClass += " active";
                    break;
                case CalendarViewMode.Day:
                    lblRange.Text = StartDate.ToString("dd MMM, yyyy");
                    hlDayView.CssClass += " active";
                    break;
            }
            hlPastView.NavigateUrl = FormatUrl(CalendarViewMode.Past, StartDate);
            hlAgendaView.NavigateUrl = FormatUrl(CalendarViewMode.Agenda, StartDate);
            hlMonthView.NavigateUrl = FormatUrl(CalendarViewMode.Month, StartDate);
            hlWeekView.NavigateUrl = FormatUrl(CalendarViewMode.Week, StartDate);
            hlDayView.NavigateUrl = FormatUrl(CalendarViewMode.Day, StartDate);
        }

        protected void rptrActivities_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.DataItem != null)
            {
                Logic.Entities.Activity activity = (Logic.Entities.Activity)e.Item.DataItem;
                Panel pnlActivity = (Panel)e.Item.FindControl("pnlActivity");
                PlaceHolder phParticipants = (PlaceHolder)e.Item.FindControl("phParticipants");
                Label lblTime = (Label)e.Item.FindControl("lblTime");
                Label lblDate = (Label)e.Item.FindControl("lblDate");
                Label lblTagCourse = (Label)e.Item.FindControl("lblTagCourse");
                Label lblTagPreparation = (Label)e.Item.FindControl("lblTagPreparation");
                Label lblDone = (Label)e.Item.FindControl("lblDone");
                Label lblNotes = (Label)e.Item.FindControl("lblNotes");
                Label lblParticipantsIn = (Label)e.Item.FindControl("lblParticipantsIn");
                Label lblParticipantsMax = (Label)e.Item.FindControl("lblParticipantsMax");
                HyperLink hlActivity = (HyperLink)e.Item.FindControl("hlActivity");

                hlActivity.Text = activity.Heading;
                hlActivity.NavigateUrl = ResolveUrl(string.Format(
                    "~/Calendar/View.aspx?activity={0}",
                    Helpers.General.Encode(activity.Id)));
                lblNotes.Text = new Helpers.RTF(activity.Notes).ToText();
                lblDate.Text = activity.Date.ToString("d MMM yyyy");
                if (activity.TypeFlag == Logic.Entities.ActivityTypeFlag.Course)
                {
                    lblTagCourse.Visible = true;
                    // Show time if we can
                    try
                    {
                        lblTime.Visible = true;
                        lblTime.Text = String.Format("{0:00}:{1:00}-{2:00}:{3:00}",
                            activity.StartTime.Hours, activity.StartTime.Minutes,
                            activity.EndTime.Hours, activity.EndTime.Minutes);
                    }
                    catch
                    {
                        lblTime.Visible = false;
                    }
                    // Participants
                    if (activity.DeliveryPackageId != null)
                    {
                        Logic.Entities.DeliveryPackage package;
                        List<Logic.Entities.Contact> participants;
                        
                        package = Endpoint.GetDeliveryPackageById(activity.DeliveryPackageId, null);
                        participants = Endpoint.GetParticipantsByActivityId(activity.Id);

                        phParticipants.Visible = true;
                        lblParticipantsMax.Text = package.GroupSize.ToString();
                        lblParticipantsIn.Text = participants.Count.ToString();
                    }
                }
                else if (activity.TypeFlag == Logic.Entities.ActivityTypeFlag.Preparation)
                {
                    lblTagPreparation.Visible = true;
                    lblDone.Visible = activity.IsDone;
                }
                else
                {
                    pnlActivity.Visible = false;
                }
            }
        }
    }
}
