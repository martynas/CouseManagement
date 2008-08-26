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

namespace Atrendia.CourseManagement.Frontend.Controls
{
    public partial class MiniCalendar : System.Web.UI.UserControl
    {
        protected class Day
        {
            public string dayLink;
            public DayOfWeek dayOfWeek;
            public int dayOfMonth;
            public bool isToday;
            public bool isOtherMonth;
            public bool isWeekend
            {
                get 
                {
                    return (dayOfWeek == DayOfWeek.Saturday
                           || dayOfWeek == DayOfWeek.Sunday);
                }
            }
            public bool hasDayLink
            {
                get { return dayLink != null; }
            }
        }

        /// <summary>
        /// Links for days.
        /// </summary>
        public Dictionary<int, string> DayLinks
        {
            get
            {
                if (ViewState["DayLinks"] == null)
                {
                    ViewState["DayLinks"] = new Dictionary<int, string>();
                }
                return (Dictionary<int, string>)ViewState["DayLinks"];
            }
        }

        protected int GetDayOfWeekNumber(DayOfWeek day, DayOfWeek first)
        {
            int diff = (int)day - (int)first;
            return diff < 0 ? diff + 7 : diff;
        }

        /// <summary>
        /// Get index [0..6] for day of week.
        /// </summary>
        /// <param name="day"></param>
        /// <returns></returns>
        protected int GetDayOfWeekNumber(DayOfWeek day)
        {
            return GetDayOfWeekNumber(day, DayOfWeek.Monday);
        }

        /// <summary>
        /// Generate days and bind them to repeater.
        /// </summary>
        protected void GenerateDays()
        {
            List<Day[]> weeks = new List<Day[]>();

            DateTime today = DateTime.Now.Date;
            DateTime firstDay = today.AddDays(1 - today.Day);
            DateTime afterDay = firstDay.AddMonths(1);

            DateTime day = firstDay;
            while (day < afterDay)
            {
                Day[] week = new Day[7];
                int filled = 0;
                if (day == firstDay)
                {
                    int needToFill = GetDayOfWeekNumber(day.DayOfWeek);
                    DateTime past = firstDay.AddDays(-needToFill);
                    for (; needToFill > 0; needToFill--)
                    {
                        Day entry = new Day();
                        entry.dayOfMonth = past.Day;
                        entry.dayOfWeek = past.DayOfWeek;
                        entry.isOtherMonth = true;
                        entry.isToday = false;
                        week[filled++] = entry;
                        past = past.AddDays(1);
                    }
                }
                for (; filled < 7; filled++)
                {
                    Day entry = new Day();
                    entry.dayOfMonth = day.Day;
                    entry.dayOfWeek = day.DayOfWeek;
                    entry.isToday = day == today;
                    entry.isOtherMonth = day.Month != firstDay.Month;
                    if (DayLinks.ContainsKey(day.Day) && !entry.isOtherMonth)
                    {
                        entry.dayLink = DayLinks[day.Day];
                    }
                    week[filled] = entry;
                    day = day.AddDays(1); // May as well roll over to the next month
                }
                weeks.Add(week);
            }
            rptrWeeks.DataSource = weeks;
            rptrWeeks.DataBind();
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            GenerateDays();
        }

        protected void rptrWeeks_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.DataItem != null)
            {
                Day[] week = (Day[])e.Item.DataItem;
                Repeater rptrDays = (Repeater)e.Item.FindControl("rptrDays");
                rptrDays.DataSource = week;
                rptrDays.DataBind();
            }
        }

        protected void rptrDays_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.DataItem != null)
            {
                Day day = (Day)e.Item.DataItem;
                HtmlTableCell td = (HtmlTableCell)e.Item.FindControl("tdDay");
                if (day.hasDayLink)
                {
                    HyperLink link = new HyperLink();
                    link.Text = day.dayOfMonth.ToString();
                    link.NavigateUrl = day.dayLink;
                    td.Controls.Add(link);
                }
                else
                {
                    Literal literal = new Literal();
                    literal.Text = day.dayOfMonth.ToString();
                    td.Controls.Add(literal);
                }
                td.Attributes["class"] = string.Empty;
                if (day.isWeekend)
                    td.Attributes["class"] = "weekend";
                if (day.isOtherMonth)
                    td.Attributes["class"] = "other-month";
                if (day.isToday)
                    td.Attributes["class"] = "today";
            }
        }
    }
}
