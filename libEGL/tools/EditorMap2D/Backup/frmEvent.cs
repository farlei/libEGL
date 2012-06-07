using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace EditorMapa2D
{
    public delegate void newEventHandler();
    public delegate void deleteEventHandler(int event_code);
    public delegate void editEventHandler(int event_code, string event_name);

    public partial class frmEvent : Form
    {
        private int code_event;
        public bool ok;
        public event newEventHandler newEvent;
        public event editEventHandler editEvent;
        public event deleteEventHandler deleteEvent;

        public frmEvent()
        {
            InitializeComponent();
        }

        public int codeEvent
        {
            get { return code_event; }
        }

        public void clear()
        {
            ok = false;
            dsControle.events.Clear();
            dgEventos.Refresh();
        }

        public void AddEvent(int codeEvent, string name)
        {
            code_event = codeEvent;

            DataTable dt = dsControle.events;
            DataRow dr = dt.NewRow();
            dr["nm_event"] = name;
            dr["code"] = code_event;
            dt.Rows.InsertAt(dr, 0);
            
            if (dgEventos.Rows.Count > 0)
                dgEventos.Rows[0].Selected = true;

            dgEventos.Refresh();

            btnDeleteEvent.Enabled = (dgEventos.Rows.Count > 1);
        }

        private void btnNewEvent_Click(object sender, EventArgs e)
        {
            if (newEvent != null)
                newEvent();
        }

        private void btnDeleteEvent_Click(object sender, EventArgs e)
        {
            int code = Convert.ToInt32(dgEventos.SelectedRows[0].Cells["eventCode"].Value);
            DataRow[] rows = dsControle.events.Select("code=" + code);
            foreach (dsControle.eventsRow dr in rows)
            {
                dsControle.events.RemoveeventsRow(dr);

                if (deleteEvent != null)
                    deleteEvent(code);
            }
            dgEventos.Refresh();

            dgEventos.Rows[0].Selected = true;

            DataGridViewCell celula = dgEventos["eventCode", 0];

            code_event = Convert.ToInt32(celula.Value);

            btnDeleteEvent.Enabled = (dgEventos.Rows.Count > 1);
        }

        private void dgEventos_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            DataGridViewCell celula = dgEventos["eventName", e.RowIndex];
            if (celula.Value.ToString().Trim() != string.Empty)
            {
                if (editEvent != null)
                    editEvent(code_event, celula.Value.ToString());
            }
            else
            {
                celula.Value = "Evento";
            }
        }

        private void dgEventos_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            dgEventos.Rows[e.RowIndex].Selected = true;

            DataGridViewCell celula = dgEventos["eventCode", e.RowIndex];

            int code = Convert.ToInt32(celula.Value);
            if (code != code_event)
            {
                code_event = code;
            }

            dgEventos.Refresh();
        }

        private void btCancelar_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btOk_Click(object sender, EventArgs e)
        {
            ok = true;
            this.Close();
        }

        private void frmEvent_Shown(object sender, EventArgs e)
        {
            if (dgEventos.Rows.Count > 0)
                dgEventos.Rows[0].Selected = true;
            btnDeleteEvent.Enabled = (dgEventos.Rows.Count > 1);
        }
    }
}
