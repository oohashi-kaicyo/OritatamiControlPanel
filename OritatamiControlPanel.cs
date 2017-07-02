using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace WindowsFormsApp1
{
    public class OritatamiControlPanel
    {
        private readonly Panel panel;
        private readonly List<Control> controls;
        private readonly int detailSettingOpenSize;
        private readonly int detailSettingCloseSize;
        private readonly Form form;
        private readonly int diffHeight;
        private bool IsClosed { get; set; }

        private OritatamiControlPanel() {
            throw new NotImplementedException();
        }

        public OritatamiControlPanel(Panel panel) {
            this.panel = panel;
            this.controls = new List<Control>();
            this.detailSettingOpenSize = this.panel.Size.Height;
            this.detailSettingCloseSize = 0;
            this.form = (Form)panel.Parent;
            this.diffHeight = this.detailSettingOpenSize - this.detailSettingCloseSize;
            Action<Form> addControlsUnderPanel = (target) => {
                List<Control> controls = GetAllControls<Control>(target);
                foreach (Control control in controls) {
                    if (control.Location.Y >= panel.Location.Y + detailSettingOpenSize) {
                        Add(control);
                    }
                }
            };
            addControlsUnderPanel(this.form);
            IsClosed = false;
        }

        private void Add(Control control) {
            controls.Add(control);
        }

        private bool OpenDetailSettingPanel() {
            if (!IsClosed) {
                return false;
            }
            this.panel.Size = new Size(this.panel.Size.Width, this.detailSettingOpenSize);
            foreach (Control control in controls) {
                control.Location = new Point(control.Location.X, control.Location.Y + this.diffHeight);
            }
            this.form.Size = new Size(form.Size.Width, this.form.Height + diffHeight);
            IsClosed = false;
            return true;
        }

        private bool CloseDetailSettingPanel() {
            if (IsClosed) {
                return false;
            }
            this.panel.Size = new Size(this.panel.Size.Width, this.detailSettingCloseSize);
            foreach (Control control in controls) {
                control.Location = new Point(control.Location.X, control.Location.Y - this.diffHeight);
            }
            this.form.Size = new Size(this.form.Size.Width, this.form.Size.Height - this.diffHeight);
            IsClosed = true;
            return true;
        }

        public void ChangeOritatamiState() {
            if (OpenDetailSettingPanel()) {
                return;
            }
            CloseDetailSettingPanel();
        }

        private List<T> GetAllControls<T>(Control top) where T : Control {
            List<T> buf = new List<T>();
            foreach (Control ctrl in top.Controls) {
                if (ctrl is T)
                    buf.Add((T)ctrl);
                buf.AddRange(GetAllControls<T>(ctrl));
            }
            return buf;
        }
    }
}
