using GalaSoft.MvvmLight.Command;
using Gemini.Framework;
using Gemini.Framework.Services;
using Gemini.Modules.Inspector;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.ComponentModel.Composition;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using System.Windows;
using System.Windows.Input;

namespace Wafle3DEditor.ViewModels
{
    [Export(typeof(HierarchyViewModel))]
    public class HierarchyViewModel : Tool, IInspectorTool
    {
        public ObservableCollection<HierarchyBtn> Hierarchy { get; set; }
        public ICommand TestCommand { get; private set; }

        public event EventHandler SelectedObjectChanged;

        private bool _isDrag;
        private bool _isMouseUp;
        private bool _isMouseOver;
        private HierarchyBtn _currentDragItem;
        private HierarchyBtn _currentOverItem;

        public HierarchyViewModel()
        {
            Hierarchy = new ObservableCollection<HierarchyBtn>();
            TestCommand = new RelayCommand<object>(AddObject);
        }

        private void AddObject(object parameter)
        {
            Random rndID = new Random();
            HierarchyBtn btnConfig = new HierarchyBtn(this, rndID.Next(1, 99999).ToString(), Hierarchy.Count);
            Hierarchy.Add(btnConfig);
        }

        public bool BtnOnClick(HierarchyBtn hBtn)
        {
            _currentDragItem = hBtn;
            _isDrag = true;
            //MessageBox.Show("CLICKED " + _currentDragItem.Id + "  " + Hierarchy.Count.ToString());

            return true;
        }
        public bool BtnOnMouseLeave(HierarchyBtn hBtn)
        {
            //MessageBox.Show(hBtn.test.ToString());

            /*if (_isDrag && _isMouseUp && _currentDragItem.Parent != null)
            {
                //if (_currentDragItem.Parent.Parent != null)
                    //_currentDragItem.Parent.BtnChildPosition = new Thickness(0, 0, 0, 0);

                _currentDragItem.Parent = null;
                _currentDragItem.BtnChildPosition = new Thickness(0, 0, 0, 0);

                Hierarchy.Remove(_currentDragItem);
                Hierarchy.Insert(Hierarchy.Count, _currentDragItem);

                _isDrag = false;
                _isMouseUp = false;
                _currentDragItem = null;
            }*/
            _currentOverItem = hBtn;
            _isMouseOver = false;

            return true;
        }
        public bool BtnOnMouseOver(HierarchyBtn hBtn)
        {
            //MessageBox.Show(hBtn.Id.ToString());
            //MessageBox.Show(_isMouseUp.ToString());

            /*if (_isDrag && !_isMouseUp && _currentDragItem.Id != hBtn.Id)
            {
                _currentDragItem.Parent = hBtn;
                _currentDragItem.BtnChildPosition = new Thickness(hBtn.BtnChildPosition.Left + 20, 0, 0, 0);

                Hierarchy.Remove(_currentDragItem);
                //MessageBox.Show("ASDAWD");
                Hierarchy.Insert(Hierarchy.IndexOf(hBtn) + 1, _currentDragItem);

                _isDrag = false;
                _currentDragItem = null;
                _isMouseUp = false;
            }*/

            _currentOverItem = hBtn;

            if (_isDrag && _isMouseUp)
            {
                if (_currentDragItem.Id == hBtn.Id)
                {
                    _isMouseUp = false;
                    _isDrag = false;
                    _currentDragItem = null;
                    return true;
                }

                _currentDragItem.Parent = hBtn;
                _currentDragItem.BtnChildPosition = new Thickness(hBtn.BtnChildPosition.Left + 20, 0, 0, 0);

                Hierarchy.Remove(_currentDragItem);
                //MessageBox.Show(hBtn.Id.ToString());
                Hierarchy.Insert(Hierarchy.IndexOf(hBtn) + 1, _currentDragItem);

                _isMouseUp = false;
                _isDrag = false;
                _currentDragItem = null;
            }

            //MessageBox.Show(_isMouseUp.ToString());

            _isMouseOver = true;
            return true;
        }
        public void BtnLoop(Object source, ElapsedEventArgs e)
        {
            
        }
        public bool BtnOnMouseUp(HierarchyBtn hBtn)
        {
            _isMouseUp = true;

            //MessageBox.Show(_currentOverItem.Id.ToString());



            //_isDrag = false;
            //_currentDragItem = null;
            //_isMouseUp = false;

            return true;
        }

        public override PaneLocation PreferredLocation
        {
            get { return PaneLocation.Left; }
        }

        private IInspectableObject _selectedObject;
        public IInspectableObject SelectedObject
        {
            get { return _selectedObject; }
            set
            {
                _selectedObject = value;
                NotifyOfPropertyChange(() => SelectedObject);
            }
        }
    }

    public class HierarchyBtn
    {
        public string BtnText { get; private set; }
        public Thickness BtnChildPosition { get; set; }
        public HierarchyBtn Parent { get; set; } = null;

        public ICommand BtnClick { get; private set; }
        public ICommand MouseEnterBtn { get; private set; }
        public ICommand MouseUpBtn { get; private set; }
        public ICommand MouseLeaveBtn { get; private set; }
        
        public int Id;

        public bool test;

        private HierarchyViewModel _hierarchy;
        private Func<bool> _hierarchyCall;

        public HierarchyBtn(HierarchyViewModel hierarchy, string text, int id)
        {
            this.BtnText = text;
            this.Id = id;
            this._hierarchy = hierarchy;
            //this.BtnChildPosition = childPos;

            BtnClick = new RelayCommand<object>(OnClick);
            MouseEnterBtn = new RelayCommand<object>(OnMouseEnter);
            MouseUpBtn = new RelayCommand<object>(OnMouseUp);
            MouseLeaveBtn = new RelayCommand<object>(OnMouseLeave);
        }

        private void OnClick(object parameter)
        {
            //MessageBox.Show("inClick " + Id);

            _hierarchyCall = () => _hierarchy.BtnOnClick(this);
            _hierarchyCall();
        }

        private void OnMouseEnter(object parameter)
        {
            test = true;

            //Console.WriteLine("DRAGGING");

            _hierarchyCall = () => _hierarchy.BtnOnMouseOver(this);
            _hierarchyCall();

            //MessageBox.Show("Mouse Enter " + Id);
        }
        private void OnMouseLeave(object parameter)
        {
            test = false;

            _hierarchyCall = () => _hierarchy.BtnOnMouseLeave(this);
            _hierarchyCall();

            //MessageBox.Show("Mouse up " + Id);
        }
        private void OnMouseUp(object parameter)
        {
            _hierarchyCall = () => _hierarchy.BtnOnMouseUp(this);
            _hierarchyCall();

            //MessageBox.Show("Mouse up " + Id);
        }

        public void RemoveParent()
        { 
            
        }
    }
}
