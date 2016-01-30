using System;
using System.Diagnostics;
using System.Reactive.Linq;
using RxFramework;
using System.Windows;

namespace WpfForRx
{
    public class Vm : ReactiveObject
    {

        private string _value = "asa";
        private PropertyObserver<Vm,long> _interval;

        public string Value
        {
            get { return _value; }
            set
            {
                _value = value;
                OnPropertyChanged();
            }
        }

        public long Interval
        {
            get
            {
                return _interval.Value;
            }
        }

        public Vm()
        {
            var command = new ReactiveCommand<string>(s => MessageBox.Show(s), _ => true);
            ObservableFromProperty(this,t => t.Value)
                .DistinctUntilChanged()
                .Where(t => !string.IsNullOrWhiteSpace(t))
                .Select(t => t.ToUpper())
                .Throttle(TimeSpan.FromSeconds(1))
                .InvokeCommand(command);

            var obs = Observable.Merge(Observable.Interval(TimeSpan.FromSeconds(1)),
                Observable.Interval(TimeSpan.FromMilliseconds(300)).Select(t => t + 100));
            _interval = obs.ToProperty(this, t => t.Interval);
        }


    }


}