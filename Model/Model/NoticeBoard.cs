using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Navigation;

namespace Model.Model
{
    public class NoticeBoard
    {
        private List<ExtraTask> extraTasks;
        public void addExtraTask(ExtraTask extraTask) { extraTasks.Add(extraTask); }
        public bool isExtraTaskFinished() { /*code*/ return true; }
    }

    public class ExtraTask
    {
        private int points;
        private Field[] fields;
        public int Points { get { return points; } set { points = value; } }
        public Field[] Fields { get { return fields; } set { fields = value; } }
    }
}
