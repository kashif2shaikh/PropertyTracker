using Cirrious.MvvmCross.Binding.BindingContext;
using Cirrious.MvvmCross.Touch.Views;
using PropertyTracker.Core.ViewModels;

namespace PropertyTracker.UI.iOS.ViewControllers
{
    public class StatePickerViewController : MvxTableViewController
    {
        // Controller does not have a XIB/Storyboard.
        public StatePickerViewController()
        {

        }

        public new StatePickerViewModel ViewModel
        {
            get { return (StatePickerViewModel)base.ViewModel; }
            set { base.ViewModel = value; }
        }

        public override void DidReceiveMemoryWarning()
        {
            // Releases the view if it doesn't have a superview.
            base.DidReceiveMemoryWarning();

            // Release any cached data, images, etc that aren't in use.
        }
       
        public override void ViewDidLoad()
        {          
            base.ViewDidLoad();
            
            // Use custom table source so I can put checkmarks on cells
            var source = new CheckmarkTableSource(TableView, "TitleText");         
            TableView.Source = source;
            TableView.AllowsSelection = true;
            
            // reference: to allow multiple selection
            //TableView.AllowsMultipleSelection = true;
            //set.Bind(source).For(s => s.SelectionChangedCommand).To(vm => vm.CitySelectedCommand);           

            var set = this.CreateBindingSet<StatePickerViewController, StatePickerViewModel>();
            set.Bind(source).To(vm => vm.States);            
            set.Bind(source).For(s => s.SelectedItem).To(vm => vm.SelectedState);
            set.Apply();
           
            TableView.ReloadData();
        }

        public override void ViewWillDisappear(bool animated)
        {
            base.ViewWillDisappear(animated);
            ViewModel.StatePickerDoneCommand.Execute(null);
        }
    }
}