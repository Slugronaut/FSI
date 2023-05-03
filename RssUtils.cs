using System;
using System.Drawing;
using System.Drawing.Text;

namespace SAOT
{
    

    public static class RssUtils
    {
        [System.Runtime.InteropServices.DllImport("gdi32.dll")]
        private static extern IntPtr AddFontMemResourceEx(IntPtr pbFont, uint cbFont,
               IntPtr pdv, [System.Runtime.InteropServices.In] ref uint pcFonts);

        private static PrivateFontCollection fonts = new PrivateFontCollection();

        public static Font _BarcodeFont;
        public static Font BarcodeFont
        { 
            get
            {
                if (_BarcodeFont == null)
                    _BarcodeFont = LoadBarcodeFont(24);
                return _BarcodeFont;
            }
        }

        public static Font LoadBarcodeFont(float size)
        {
            byte[] fontData = Properties.Resources.Barcode2;
            IntPtr fontPtr = System.Runtime.InteropServices.Marshal.AllocCoTaskMem(fontData.Length);
            System.Runtime.InteropServices.Marshal.Copy(fontData, 0, fontPtr, fontData.Length);
            uint dummy = 0;
            fonts.AddMemoryFont(fontPtr, Properties.Resources.Barcode2.Length);
            AddFontMemResourceEx(fontPtr, (uint)Properties.Resources.Barcode2.Length, IntPtr.Zero, ref dummy);
            System.Runtime.InteropServices.Marshal.FreeCoTaskMem(fontPtr);

            return new Font(fonts.Families[0], size);
        }
    }
}
