using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Text;
using System.Windows.Forms;

namespace BrowserSelect.UI
{
    /// <summary>
    /// Test aid for the UI screenshot workflow: when the BROWSERSELECT_LAYOUT_LOG environment variable
    /// names a folder, every themed window writes a report of its controls there once it is shown
    /// (clipped text, overlapping controls, controls outside their parent or the screen).
    /// Does nothing for normal users (the variable is not set).
    /// </summary>
    internal static class LayoutCheck
    {
        private static readonly string Folder = Environment.GetEnvironmentVariable("BROWSERSELECT_LAYOUT_LOG");

        public static bool Enabled
        {
            get { return !string.IsNullOrEmpty(Folder); }
        }

        public static void Hook(Form form)
        {
            if (!Enabled)
                return;
            form.Shown -= Form_Shown;
            form.Shown += Form_Shown;
        }

        private static void Form_Shown(object sender, EventArgs e)
        {
            var form = (Form)sender;
            // after the first layout/paint pass
            var timer = new Timer { Interval = 700 };
            timer.Tick += (s, a) =>
            {
                timer.Stop();
                timer.Dispose();
                if (!form.IsDisposed)
                    Write(form);
            };
            timer.Start();
        }

        public static void Write(Form form)
        {
            try
            {
                var lines = new StringBuilder();
                var issues = new List<string>();
                lines.AppendFormat("form {0} \"{1}\" dpi={2} bounds={3} client={4} font={5} {6}pt",
                    form.Name, form.Text, Dpi(form), form.Bounds, form.ClientSize, form.Font.Name,
                    form.Font.SizeInPoints).AppendLine();
                var area = Screen.FromControl(form).WorkingArea;
                if (!area.Contains(form.Bounds))
                    issues.Add(string.Format("OFFSCREEN window {0} is not inside the screen working area {1}", form.Bounds, area));
                Walk(form, Dpi(form) / 96f, lines, issues, 1);
                // machine readable positions for the test script: name, rectangle relative to the window
                lines.AppendLine();
                lines.AppendFormat("#win {0} {1}", form.Width, form.Height).AppendLine();
                AppendPositions(form, form, lines);
                var text = new StringBuilder();
                text.AppendLine("issues: " + issues.Count);
                foreach (var issue in issues)
                    text.AppendLine("  " + issue);
                text.AppendLine();
                text.Append(lines);
                Directory.CreateDirectory(Folder);
                File.WriteAllText(Path.Combine(Folder, form.Name + ".txt"), text.ToString(), Encoding.UTF8);
            }
            catch (Exception ex)
            {
                try { File.WriteAllText(Path.Combine(Folder, form.Name + ".error.txt"), ex.ToString()); }
                catch (Exception) { }
            }
        }

        [System.Runtime.InteropServices.DllImport("user32.dll")]
        private static extern uint GetDpiForWindow(IntPtr hwnd);

        private static int Dpi(Control c)
        {
            try { return (int)GetDpiForWindow(c.Handle); }
            catch (Exception) { return 96; }
        }

        private static string Id(Control c)
        {
            return (string.IsNullOrEmpty(c.Name) ? c.GetType().Name : c.Name);
        }

        private static void Walk(Control parent, float scale, StringBuilder lines, List<string> issues, int depth)
        {
            var visible = new List<Control>();
            foreach (Control c in parent.Controls)
                if (c.Visible)
                    visible.Add(c);

            var inner = new Rectangle(Point.Empty, parent.ClientSize);
            inner.Inflate(1, 1);
            for (int i = 0; i < visible.Count; i++)
            {
                var c = visible[i];
                lines.Append(' ', depth * 2).AppendFormat("{0} ({1}) {2} font={3}pt \"{4}\"", Id(c), c.GetType().Name,
                    c.Bounds, c.Font.SizeInPoints, Short(c.Text)).AppendLine();
                if (!(parent is ScrollableControl && ((ScrollableControl)parent).AutoScroll) && !inner.Contains(c.Bounds))
                    issues.Add(string.Format("OUTSIDE {0}: {1} is not inside {2} (client {3})", Id(c), c.Bounds, Id(parent), parent.ClientSize));
                for (int j = i + 1; j < visible.Count; j++)
                {
                    var a = c.Bounds;
                    var b = visible[j].Bounds;
                    a.Inflate(-1, -1);
                    b.Inflate(-1, -1);
                    if (a.IntersectsWith(b) && !Designed(c, visible[j]))
                        issues.Add(string.Format("OVERLAP {0} {1} and {2} {3}", Id(c), c.Bounds, Id(visible[j]), visible[j].Bounds));
                }
                var clip = CheckText(c, scale);
                if (clip != null)
                    issues.Add(string.Format("CLIP {0} \"{1}\": {2}", Id(c), Short(c.Text), clip));
                if (c.HasChildren && !(c is DataGridView) && !(c is ComboBox) && !(c is NumericUpDown))
                    Walk(c, scale, lines, issues, depth + 1);
            }
        }

        private static void AppendPositions(Form form, Control parent, StringBuilder lines)
        {
            foreach (Control c in parent.Controls)
            {
                if (!c.Visible)
                    continue;
                var r = parent.RectangleToScreen(c.Bounds);
                lines.AppendFormat("#ctl {0} {1} {2} {3} {4} {5}", Id(c), r.X - form.Left, r.Y - form.Top, r.Width, r.Height,
                    c.Enabled ? 1 : 0).AppendLine();
                if (c.HasChildren && !(c is DataGridView) && !(c is ComboBox) && !(c is NumericUpDown))
                    AppendPositions(form, c, lines);
            }
        }

        /// <summary>overlaps that are part of the original design</summary>
        private static bool Designed(Control a, Control b)
        {
            var pair = Id(a) + "/" + Id(b);
            switch (pair)
            {
                case "btn_help/ButtonsUC": case "ButtonsUC/btn_help":     // ? at the bottom of the side buttons
                case "btn_refresh/groupBox1": case "groupBox1/btn_refresh": // Refresh on the caption line
                case "shortcuts/name": case "name/shortcuts":               // transparent labels touching
                    return true;
            }
            return false;
        }

        private static string Short(string s)
        {
            s = (s ?? "").Replace("\r", " ").Replace("\n", " ");
            return s.Length > 50 ? s.Substring(0, 47) + "..." : s;
        }

        private static Size Measure(Control c, string text, int width, TextFormatFlags flags)
        {
            using (var g = c.CreateGraphics())
                return TextRenderer.MeasureText(g, text, c.Font, new Size(Math.Max(1, width), int.MaxValue), flags);
        }

        /// <summary>null when the text fits, otherwise what is wrong</summary>
        private static string CheckText(Control c, float scale)
        {
            var text = c.Text ?? "";
            if (c is VButton)
                return null;
            if (c is DataGridView)
            {
                var gv = (DataGridView)c;
                if (!gv.ColumnHeadersVisible)
                    return null;
                foreach (DataGridViewColumn col in gv.Columns)
                {
                    if (!col.Visible || string.IsNullOrEmpty(col.HeaderText))
                        continue;
                    var font = gv.ColumnHeadersDefaultCellStyle.Font ?? gv.Font;
                    var w = TextRenderer.MeasureText(col.HeaderText, font).Width;
                    if (w + 2 * scale > col.Width)
                        return string.Format("column header \"{0}\" needs {1}px, column is {2}px", col.HeaderText, w, col.Width);
                    var h = TextRenderer.MeasureText("Ag", font).Height;
                    if (h > gv.ColumnHeadersHeight)
                        return string.Format("header row {0}px is lower than the text ({1}px)", gv.ColumnHeadersHeight, h);
                }
                return null;
            }
            if (text.Length == 0)
                return null;
            var flags = TextFormatFlags.WordBreak | TextFormatFlags.TextBoxControl;
            if (c is GroupBox)
            {
                var w = Measure(c, text, int.MaxValue / 2, TextFormatFlags.SingleLine).Width;
                return w + 16 * scale > c.Width ? string.Format("caption needs {0}px, box is {1}px", w, c.Width) : null;
            }
            if (c is ButtonBase && !(c is CheckBox) && !(c is RadioButton))
            {
                var size = Measure(c, text, c.Width - (int)(6 * scale), flags);
                var single = Measure(c, text, int.MaxValue / 2, TextFormatFlags.SingleLine);
                if (single.Width > c.Width - 4 * scale && size.Height > c.Height - 2 * scale)
                    return string.Format("needs {0} ({1} single line), button is {2}", size, single, c.Size);
                return null;
            }
            if (c is CheckBox || c is RadioButton)
            {
                var box = (int)Math.Ceiling(18 * scale);
                var size = Measure(c, text, int.MaxValue / 2, TextFormatFlags.SingleLine);
                if (size.Height > c.Height + 1 || size.Width > c.Width - box + 1)
                    return string.Format("needs {0} + {1}px box, control is {2}", size, box, c.Size);
                return null;
            }
            if (c is Label)
            {
                var label = (Label)c;
                if (label.AutoEllipsis)
                    return null;
                var width = label.ClientSize.Width - label.Padding.Horizontal;
                Size size;
                if (label.UseCompatibleTextRendering)
                {
                    using (var g = label.CreateGraphics())
                        size = Size.Ceiling(g.MeasureString(text, label.Font, width));
                }
                else
                {
                    size = Measure(c, text, width, flags);
                }
                var height = label.ClientSize.Height - label.Padding.Vertical;
                if (size.Height > height + 1 || size.Width > width + 1)
                    return string.Format("needs {0}, label is {1}", size, label.ClientSize);
                return null;
            }
            if (c is ComboBox)
            {
                var w = Measure(c, text, int.MaxValue / 2, TextFormatFlags.SingleLine).Width;
                var room = c.Width - SystemInformation.VerticalScrollBarWidth * scale - 6 * scale;
                return w > room ? string.Format("selected text needs {0}px, room {1}px", w, (int)room) : null;
            }
            if (c is TextBox && !((TextBox)c).Multiline)
            {
                var h = Measure(c, "Ag", int.MaxValue / 2, TextFormatFlags.SingleLine).Height;
                return h > c.ClientSize.Height + 1 ? string.Format("text needs {0}px height, box is {1}px", h, c.ClientSize.Height) : null;
            }
            return null;
        }
    }
}
