using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Interactivity;

namespace Aiva.Bot.Internal {
    // http://stackoverflow.com/questions/23613171/wpf-how-to-make-textbox-lose-focus-after-hitting-enter/23613757#23613757
    public class TextBoxEnterKeyUpdateBehavior : Behavior<TextBox> {
        protected override void OnAttached() {
            if (this.AssociatedObject != null) {
                base.OnAttached();
                this.AssociatedObject.KeyUp += AssociatedObject_KeyDown;
            }
        }

        protected override void OnDetaching() {
            if (this.AssociatedObject != null) {
                this.AssociatedObject.KeyUp -= AssociatedObject_KeyDown;
                base.OnDetaching();
            }
        }

        private void AssociatedObject_KeyDown(object sender, System.Windows.Input.KeyEventArgs e) {
            TextBox textBox = sender as TextBox;
            if (textBox != null) {
                if (e.Key == Key.Return) {
                    if (e.Key == Key.Enter) {
                        textBox.MoveFocus(new TraversalRequest(FocusNavigationDirection.Next));
                    }
                }
            }
        }
    }
}