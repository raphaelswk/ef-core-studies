using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Infrastructure;

namespace MasteringEFCore.Domain
{
    public class Department
    {
        public Department()
        {            
        }

        // #2 - WORKAROUND TO MAKE LAZY LOAD WORKS FOR SPECIFC CASES
        private Action<object, string> _lazyLoader { get; set; }
        public Department(Action<object, string> lazyLoader)
        {
            _lazyLoader = lazyLoader;
        }

        // #1 - WORKAROUND TO MAKE LAZY LOAD WORKS FOR SPECIFC CASES
        // private ILazyLoader _lazyLoader { get; set; }
        // public Department(ILazyLoader lazyLoader)
        // {
        //     _lazyLoader = lazyLoader;
        // }
        //

        public int Id { get; set; }
        public string Description { get; set; }
        public bool Active { get; set; }

        // public virtual List<Employee> Employees { get; set; }

        // #2 - WORKAROUND TO MAKE LAZY LOAD WORKS FOR SPECIFC CASES
        private List<Employee> _employees;
        public List<Employee> Employees 
        {
            get
            {
                _lazyLoader?.Invoke(this, nameof(Employees));

                return _employees;
            }
            set => _employees = value;
        }
        //

        // #1 - WORKAROUND TO MAKE LAZY LOAD WORKS FOR SPECIFC CASES
        // private List<Employee> _employees;
        // public List<Employee> Employees 
        // {
        //     get => _lazyLoader.Load(this, ref _employees);
        //     set => _employees = value;
        // }
        //
    }
}
