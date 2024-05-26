using System.Drawing;
using System.Windows.Forms;
using System.Windows.Forms.Design;

namespace Koobar.Windows.Forms
{
    /// <summary>
    /// Windows 10ネイティブの見た目に近いデザインのToolStripSeparator
    /// </summary>
    [ToolStripItemDesignerAvailability(ToolStripItemDesignerAvailability.MenuStrip)]
    public class NativeSeparator : ToolStripSeparator
    {
        protected override void OnPaint(PaintEventArgs e)
        {
            var pen = new Pen(Color.FromArgb(215, 215, 215));

            e.Graphics.Clear(Color.FromArgb(240, 240, 240));
            e.Graphics.DrawLine(pen, 32, 3f, e.ClipRectangle.Width, 3f);

            pen.Dispose();
        }
    }
}
