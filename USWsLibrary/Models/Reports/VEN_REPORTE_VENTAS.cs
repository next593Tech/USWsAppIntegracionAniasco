using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text;

namespace NexoMobile
{
    public class VEN_REPORTE_VENTAS : INotifyPropertyChanged
    {


        private string pRODUCTONOMBRE;
        public string PRODUCTONOMBRE
        {
            get { return pRODUCTONOMBRE; }
            set { pRODUCTONOMBRE = value; OnPropertyChanged(); }
        }

        private decimal mES01;
        public decimal MES01
        {
            get { return mES01; }
            set { mES01 = value; OnPropertyChanged(); }
        }

        private decimal mES02;
        public decimal MES02
        {
            get { return mES02; }
            set { mES02 = value; OnPropertyChanged(); }
        }

        private decimal mES03;
        public decimal MES03
        {
            get { return mES03; }
            set { mES03 = value; OnPropertyChanged(); }
        }


        private decimal mES04;
        public decimal MES04
        {
            get { return mES04; }
            set { mES04 = value; OnPropertyChanged(); }
        }

        private decimal mES05;
        public decimal MES05
        {
            get { return mES05; }
            set { mES05 = value; OnPropertyChanged(); }
        }

        private decimal mES06;
        public decimal MES06
        {
            get { return mES06; }
            set { mES06 = value; OnPropertyChanged(); }
        }

        private decimal mES07;
        public decimal MES07
        {
            get { return mES07; }
            set { mES07 = value; OnPropertyChanged(); }
        }

        private decimal mES08;
        public decimal MES08
        {
            get { return mES08; }
            set { mES08 = value; OnPropertyChanged(); }
        }


        private decimal mES09;
        public decimal MES09
        {
            get { return mES09; }
            set { mES09 = value; OnPropertyChanged(); }
        }

        private decimal mES10;
        public decimal MES10
        {
            get { return mES10; }
            set { mES10 = value; OnPropertyChanged(); }
        }

        private decimal mES11;
        public decimal MES11
        {
            get { return mES11; }
            set { mES11 = value; OnPropertyChanged(); }
        }

        private decimal mES12;
        public decimal MES12
        {
            get { return mES12; }
            set { mES12 = value; OnPropertyChanged(); }
        }







        #region INotifyPropertyChanged
        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName]string propertyName = "")
        {
            var changed = PropertyChanged;
            if (changed == null)
                return;

            changed.Invoke(this, new PropertyChangedEventArgs(propertyName));

        }
        #endregion
    }
}