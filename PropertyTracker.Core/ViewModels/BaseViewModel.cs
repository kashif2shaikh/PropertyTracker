using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Cirrious.MvvmCross.ViewModels;

namespace PropertyTracker.Core.ViewModels
{

    public abstract class BaseViewModel : MvxViewModel
    {
        //
        // Tag each view model with unique instance ID
        // 
        public Guid ViewInstanceId {get; set;}

		public Guid RequestedByViewInstanceId { get; set; }


        protected BaseViewModel()
        {
           ViewInstanceId = Guid.NewGuid();
        }

		/*
		public void Init(Guid requestedViewId)
		{
			if (requestedViewId != null)
			{
				RequestedByViewInstanceId = requestedViewId;
			}
		}
		*/


        public bool ShowViewModel<TViewModel>() where TViewModel : IMvxViewModel
        {
            var requestedBy = new MvxRequestedBy();

            //
            // The Current (Parent) view model that is requesting to show a *NEW* view will have the Guid assigned into the 
            // requestedBy additional info column.
            //
            // The Presenter will pass this new view to the parent that has matching Guid, so Parent can show this view.
            //
            requestedBy.AdditionalInfo = ViewInstanceId.ToString();
            return ShowViewModel(
                typeof(TViewModel),
                null,
                null,
                requestedBy);
        }
    }
}
