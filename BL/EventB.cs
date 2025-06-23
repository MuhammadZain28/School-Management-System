using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using LMS.DL;
using MessageBox = System.Windows.MessageBox;

namespace LMS.BL
{
    class EventB
    {
        public int event_id { get; set; }
        public string name { get; set; }
        public string description { get; set; }
        public DateTime Date { get; set; }
        public EventB() { }
        public List<EventB> getData()
        {
            return EventD.GetEvent();
        }
        public bool DeleteData(int id)
        {
            return EventD.DeleteEvent(id);
        }
        public EventB GetEvent(DateTime date)
        {
            return EventD.GetEventByMonth(date);
        }
        public bool AddEvent(EventB eventB)
        {
            if (eventB.name != null && eventB.Date != null && eventB.description != null)
            {
                if (eventB.Date < DateTime.Now)
                {

                    
                    MessageBox.Show("Event Can't be Scheduled in past!");
                    return false;
                }
                else if (description.Length < 15)
                {
                    MessageBox.Show("Description atleast have 15 words");
                    return false;
                }
                else
                {
                    return EventD.InsertEvent(eventB);
                }
            }
            else
            {
                MessageBox.Show("All Fields are Required");
                return false;
            }
        }
    }
}
