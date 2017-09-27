using System;
using System.Collections.Generic;
using Android.App;
using Android.OS;
using Android.Views;
using Android.Support.V4.View;
using Android.Support.V4.App;

namespace Weather
{
    public class GenericFragmentPagerAdaptor : FragmentStatePagerAdapter
    {
        private List<Android.Support.V4.App.Fragment> _fragmentList = new List<Android.Support.V4.App.Fragment>();
        public GenericFragmentPagerAdaptor(Android.Support.V4.App.FragmentManager fm) : base(fm)
        {

        }

        public void AddFragmentView(Func<LayoutInflater, ViewGroup, Bundle, View> view)
        {
            _fragmentList.Add(new GenericViewPagerFragment(view));
        }
        
        public void AddFragment(GenericViewPagerFragment fragment)
        {
            _fragmentList.Add(fragment);
        }

        public override int Count
        {
            get
            {
                return _fragmentList.Count;
            }
        }
        
        public override Android.Support.V4.App.Fragment GetItem(int position)
        {

            return _fragmentList[position];            
        }
    }

    public class ViewPageListenerForActionBar : ViewPager.SimpleOnPageChangeListener
    {
        private ActionBar _bar;
        public ViewPageListenerForActionBar(ActionBar bar)
        {
            _bar = bar;
        }
        public override void OnPageSelected(int position)
        {
            _bar.SetSelectedNavigationItem(position);
        }
    }

    public static class ViewPagerExtensions
    {
#pragma warning disable CS0618 // 'ActionBar.Tab' is obsolete: 'This class is obsoleted in this android platform'
        public static ActionBar.Tab GetViewPageTab(this ViewPager viewPager, ActionBar actionBar, string name)
#pragma warning restore CS0618 // 'ActionBar.Tab' is obsolete: 'This class is obsoleted in this android platform'
        {
            var tab = actionBar.NewTab();
            tab.SetText(name);
            tab.TabSelected += (o, e) => {
                viewPager.SetCurrentItem(actionBar.SelectedNavigationIndex, false);
            };
            return tab;
        }
    }
}