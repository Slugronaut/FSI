using Ookii.Dialogs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SAOT
{
    static class Dialog
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="msg"></param>
        /// <param name="owner"></param>
        public static void Message(string msg, IWin32Window owner = null)
        {
            MessageBox.Show(owner, msg);
        }

        public static DialogResult yesButton = DialogResult.Yes;
        public static DialogResult cancelButton = DialogResult.No;
        /// <summary>
        /// Helper for displaying yes/no confirmation dialogs.
        /// </summary>
        /// <param name="action"></param>
        public static DialogResult ConfirmAction(string msg, IWin32Window owner = null, Action<DialogResult> action = null)
        {
            var result = MessageBox.Show(owner, msg, "Confirm Action", MessageBoxButtons.YesNo);
            action?.Invoke(result);
            return result;
        }



        public static TaskDialogButton yesTaskButton = new TaskDialogButton(ButtonType.Yes);
        public static TaskDialogButton cancelTaskButton = new TaskDialogButton(ButtonType.No);
        public static TaskDialogButton ConfirmTask(string msg, IWin32Window owner = null, Action<TaskDialogButton> action = null)
        {
            TaskDialogButton result = null;

            using (var dialog = new TaskDialog())
            {
                dialog.FooterIcon = TaskDialogIcon.Warning;
                dialog.MainInstruction = msg;
                dialog.Buttons.Add(yesTaskButton);
                dialog.Buttons.Add(cancelTaskButton);
                result = dialog.ShowDialog(owner);
                action?.Invoke(result);
            }

            return result;
        }
    }
}
