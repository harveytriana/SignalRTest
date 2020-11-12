using System;
using System.Windows.Forms;

public static class ApplicationExtensions
{
    public static void Let<TControl>(this TControl control, Action<TControl> action) where TControl : Control
    {
        if (control.InvokeRequired) {
            control.Invoke(action, control);
        } else {
            action(control);
        }
    }

    public static void AddItemThread(this ListBox listBox, string text)
    {
        if (listBox.InvokeRequired) {
            listBox.Invoke(new Action(() => listBox.Items.Add(text)));
        } else {
            listBox.Items.Add(text);
        }
    }
}

