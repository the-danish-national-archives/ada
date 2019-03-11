namespace Ada.UI.Winforms.Reflection
{
    #region Namespace Using

    using System;
    using System.Collections.Generic;
    using System.Drawing;
    using System.Reflection;
    using System.Windows.Forms;
    using Common;
    using Ra.Common.Reflection;

    #endregion

    public static class ReflectedGUIBuilder
    {
        #region

        private static void Ctrl_EnabledChanged(object sender, EventArgs e)
        {
            foreach (Control c in (sender as Control).Parent.Parent.Controls)
                if (c.GetType() != typeof(Panel))
                    c.Enabled = (sender as CheckBox).Checked;
        }

        private static void HandleDriveStatusList(ref int verticalOffset, GroupBox tlp, ref int i, List<DriveStatus> propertyValue)
        {
            foreach (var drv in propertyValue)
            {
                var lbl = new Label();
                lbl.Text = drv.Drive;
                Control Ctrl = new CheckBox();

                Ctrl.Dock = DockStyle.Fill;
                Ctrl.DataBindings.Add("Checked", drv, "Status");
                Ctrl.Name = "checkBoxDrv" + drv.Drive.Replace(@":", string.Empty).Replace(@"\", string.Empty);

                lbl.Location = new Point(10, verticalOffset);
                lbl.Anchor = AnchorStyles.Left;
                lbl.Anchor = AnchorStyles.Top;

                Ctrl.Location = new Point(tlp.Width - 28, verticalOffset - 5);
                Ctrl.Anchor = AnchorStyles.Right;
                Ctrl.Anchor = AnchorStyles.Top;

                tlp.Controls.Add(lbl);
                tlp.Controls.Add(Ctrl);

                verticalOffset += 25;
                i++;
            }
        }

        private static void HandleMasterSwitch(object reflectedObject, ref int verticalOffset, GroupBox tlp, ref int i, PropertyInfo PI)
        {
            var lbl = new Label();
            lbl.Text = UILabelsAttribute.GetUIName(PI);
            lbl.Location = new Point(10, verticalOffset - 14);
            lbl.Anchor = AnchorStyles.Top;

            var Ctrl = new CheckBox();
            Ctrl.DataBindings.Add("Checked", reflectedObject, PI.Name);
            Ctrl.Anchor = AnchorStyles.Right;
            Ctrl.Location = new Point(tlp.Width - 23, verticalOffset - 19);
            Ctrl.CheckedChanged += Ctrl_EnabledChanged;

            var p = new Panel();

            p.BackColor = SystemColors.Control;
//            p.Width = Ctrl.Width;
            p.Height = 24;
            p.Width = tlp.Width;

            p.Location = new Point(0, verticalOffset);
            p.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;

            verticalOffset += 40;

            p.Controls.Add(lbl);
            p.Controls.Add(Ctrl);
            tlp.Controls.Add(p);

            i++;
        }

        private static void HandleVarious(object reflectedObject, bool filtered, ref int verticalOffset, GroupBox tlp, ref int i, PropertyInfo PI, object propertyValue, bool restControlsEnabled)
        {
            var lbl = new Label();
            var Ctrl = new Control();

            if (filtered && UILabelsAttribute.hasUILabels(PI))
                lbl.Text = UILabelsAttribute.GetUIName(PI);
            else
                lbl.Text = PI.Name;


            if (propertyValue is int)
            {
                Ctrl = new NumericUpDown();
                Ctrl.DataBindings.Add("Value", reflectedObject, PI.Name);
                Ctrl.Anchor = AnchorStyles.Right;
                Ctrl.Location = new Point(tlp.Width - 64, verticalOffset - 5);
                Ctrl.Width = 50;
            }

            if (propertyValue is string)
            {
                Ctrl = new TextBox();
                Ctrl.DataBindings.Add("Text", reflectedObject, PI.Name);
            }

            if (propertyValue is bool)
            {
                Ctrl = new CheckBox();
                Ctrl.DataBindings.Add("Checked", reflectedObject, PI.Name);
                Ctrl.Anchor = AnchorStyles.Right;
                Ctrl.Location = new Point(tlp.Width - 28, verticalOffset - 5);
                Ctrl.Enabled = restControlsEnabled;
            }

            if (filtered && UILabelsAttribute.hasUILabels(PI) | !filtered)
            {
                lbl.Location = new Point(10, verticalOffset);
                lbl.Anchor = AnchorStyles.Top;


                Ctrl.Anchor = AnchorStyles.Top;

                verticalOffset += 25;

                tlp.Controls.Add(lbl);
                tlp.Controls.Add(Ctrl);
                i++;
            }
        }

        public static bool PopulateSettingsControl(object reflectedObject, Control control, bool filtered, bool ignoreDrives = false)
        {
            var populated = false;

            var reflectedObjectType = reflectedObject.GetType();

            if (filtered && UILabelsAttribute.hasUILabels(reflectedObjectType) | !filtered)
            {
                if (filtered && UILabelsAttribute.hasUILabels(reflectedObjectType))
                    control.Text = UILabelsAttribute.GetUIName(reflectedObjectType);
                else
                    control.Text = reflectedObjectType.Name;

                var verticalOffset = 20;

                var tlp = new GroupBox();

                if (filtered && UILabelsAttribute.hasUILabels(reflectedObjectType))
                    tlp.Text = UILabelsAttribute.GetUIName(reflectedObjectType);
                else
                    tlp.Text = reflectedObjectType.Name;

                tlp.Anchor = AnchorStyles.Top;
                tlp.Location = new Point(5, verticalOffset);
                tlp.Height = 300;
                var restControlsEnabled = true;
                var numControlsAdded = 0;
                foreach (
                    var PI in reflectedObjectType.GetProperties(BindingFlags.Public | BindingFlags.Instance))
                {
                    var propertyValue = PI.GetValue(reflectedObject, null);
                    if (propertyValue is List<DriveStatus>)
                    {
                        if (ignoreDrives)
                            continue;
                        HandleDriveStatusList(ref verticalOffset, tlp, ref numControlsAdded, propertyValue as List<DriveStatus>);
                        continue;
                    }

                    if (UILabelsAttribute.GetUIRole(PI) == "Master Switch")
                    {
                        HandleMasterSwitch(reflectedObject, ref verticalOffset, tlp, ref numControlsAdded, PI);
                        restControlsEnabled = PI.GetValue(reflectedObject) as bool? ?? true;
                        continue;
                    }

                    HandleVarious(reflectedObject, filtered, ref verticalOffset, tlp, ref numControlsAdded, PI, propertyValue, restControlsEnabled);
                }

                if (tlp.Controls.Count != 0)
                {
                    control.Controls.Add(tlp);

// tlp.Size = new System.Drawing.Size(control.Width - 10, 20 + tlp.Controls.Count * 13);
                    tlp.Anchor = AnchorStyles.Top | AnchorStyles.Bottom;
                    tlp.Size = new Size(control.Width - 10, control.Height - 20);

                    populated = true;
                }

                foreach (
                    var fi in
                    reflectedObjectType.GetFields(
                        BindingFlags.Public | BindingFlags.DeclaredOnly | BindingFlags.Instance))
                {
                    var subGroup = new Panel();
                    subGroup.Dock = DockStyle.Left;

                    if (PopulateSettingsControl(fi.GetValue(reflectedObject), subGroup, filtered, ignoreDrives))
                    {
                        populated = true;
                        control.Controls.Add(subGroup);
                    }
                }
            }

            return populated;
        }

        #endregion
    }
}