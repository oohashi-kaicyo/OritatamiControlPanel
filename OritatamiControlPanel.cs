using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace WindowsFormsApp1
{
    public class OritatamiControlPanel
    {
        private readonly Panel panel;
        private readonly List<Control> controlsUnderPanel;
        private readonly int openHeight;
        private readonly int closeHeight;
        private readonly Form form;
        private bool IsClosed { get; set; }

        public OritatamiControlPanel(Panel panel) {

            this.panel              = panel;
            this.controlsUnderPanel = new List<Control>();
            this.openHeight         = this.panel.Size.Height;
            this.closeHeight        = 0;
            this.form               = (Form)panel.Parent;
            IsClosed                = false;
            Action<Form> addControlsUnderPanel = (target) => {
                List<Control> controls = GetAllControls<Control>(target);
                foreach (Control control in controls) {
                    bool isControlUnderPanel = control.Location.Y >= panel.Location.Y + openHeight;
                    if (isControlUnderPanel) {
                        controlsUnderPanel.Add(control);
                    }
                }
            };
            addControlsUnderPanel(this.form);
        }
        private OritatamiControlPanel() {
            throw new NotImplementedException();
        }

        private bool OpenPanel() {
            if (!IsClosed) {
                return false;
            }

            Action resizePanelForOpen = () => {
                this.panel.Size = new Size(this.panel.Size.Width, this.openHeight);
            };
            Action repositionControlsUnderPanelForOpen = () => {
                foreach (Control control in controlsUnderPanel) {
                    control.Location = new Point(control.Location.X, control.Location.Y + this.openHeight);
                }
            };
            Action resizeFormForOpen = () => {
                this.form.Size = new Size(form.Size.Width, this.form.Height + this.openHeight);
            };

            resizePanelForOpen();
            repositionControlsUnderPanelForOpen();
            resizeFormForOpen();
            IsClosed = false;
            return true;
        }

        private bool ClosePanel() {
            if (IsClosed) {
                return false;
            }

            Action resizePanleForClose = () => {
                this.panel.Size = new Size(this.panel.Size.Width, this.closeHeight);
            };
            Action repositionControlsForClose = () => {
                foreach (Control control in controlsUnderPanel) {
                    control.Location = new Point(control.Location.X, control.Location.Y - this.openHeight);
                }
            };
            Action resizeFormForClose = () => {
                this.form.Size = new Size(this.form.Size.Width, this.form.Size.Height - this.openHeight);
            };

            resizePanleForClose();
            repositionControlsForClose();
            resizeFormForClose();
            IsClosed = true;
            return true;
        }

        public void ChangeOritatamiState() {
            if (OpenPanel()) {
                return;
            }
            ClosePanel();
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
