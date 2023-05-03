using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace SAOT
{
    public static class WinFormsExtensions
    {
        public static void Clear(this Control.ControlCollection controls, bool dispose)
        {
            for (int ix = controls.Count - 1; ix >= 0; --ix)
            {
                var tmpObj = controls[ix];
                controls.RemoveAt(ix);
                if (dispose) tmpObj.Dispose();
            }
        }
    }

    
}
