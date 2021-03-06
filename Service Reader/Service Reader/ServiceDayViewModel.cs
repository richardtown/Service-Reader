﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service_Reader
{
    public class ServiceDayViewModel : ObservableObject
    {
        //RT 28/11/16 - This class doesn't properly implement the MVVM pattern.  Fixing this now.
        private DateTime m_travelStartTime;
        private DateTime m_arrivalOnsiteTime;
        private DateTime m_departureSiteTime;
        private DateTime m_travelEndTime;
        private int m_mileage;
        private int m_dailyAllowance;
        private int m_overnightAllowance;
        private int m_barrierPayment;
        private double m_travelTimeToSite;
        private double m_travelTimeFromSite;
        private double m_totalTravelTime;
        private double m_totalOnsiteTime;
        private string m_dailyReport;
        private string m_partsSuppliedToday;
        private DateTime m_dtReport;

        //This is the reference to the service day model
        private ServiceDay m_serviceDayModel;
        //Adding a reference to the containing service sheet, so the total times can be updated
        private ServiceSheetViewModel m_parentServiceSheetVM;
        //private ServiceDay sd;

        //Class constructors
        public ServiceDayViewModel(DateTime dtTravelStart, DateTime dtArrivalOnsite, DateTime dtDepartSite, DateTime dtTravelEnd, int mileageEntered, bool dailyAllowanceEntered,
            bool overnightAllowanceEntered, bool barrierPaymentEntered, double travelTimeToSiteEntered, double travelTimeFromSiteEntered, double totalTravelTimeEntered, double timeOnsiteEntered, string dailyReportEntered,
            string partsSuppliedEntered, DateTime dtServiceDateEntered, ServiceSheetViewModel parentVM)
        {
            this.TravelStartTime = dtTravelStart;
            this.ArrivalOnsiteTime = dtArrivalOnsite;
            this.DepartureSiteTime = dtDepartSite;
            this.TravelEndTime = dtTravelEnd;
            this.Mileage = mileageEntered;
            this.DailyAllowance = dailyAllowanceEntered;
            this.OvernightAllowance = overnightAllowanceEntered;
            this.BarrierPayment = barrierPaymentEntered;
            this.TravelTimeToSite = travelTimeToSiteEntered;
            this.TravelTimeFromSite = travelTimeFromSiteEntered;
            this.TotalTravelTime = totalTravelTimeEntered;
            this.TotalOnsiteTime = timeOnsiteEntered;
            this.DailyReport = dailyReportEntered;
            this.PartsSuppliedToday = partsSuppliedEntered;
            this.DtReport = dtServiceDateEntered;
            this.ParentServiceSheetVM = parentVM;
        }

        private ServiceDayViewModel()
        {

        }

        public ServiceDayViewModel(ServiceDay sd)
        {
            this.TravelStartTime = sd.TravelStartTime;
            this.ArrivalOnsiteTime = sd.ArrivalOnsiteTime;
            this.DepartureSiteTime = sd.DepartureSiteTime;
            this.TravelEndTime = sd.TravelEndTime;
            this.Mileage = sd.Mileage;
            if (sd.DailyAllowance == 0)
            {
                this.DailyAllowance = false;
            }
            else if(sd.DailyAllowance == 1)
            {
                this.DailyAllowance = true;
            }
            else
            {
                throw new Exception("Unknown daily allowance value: " + sd.DailyAllowance);
            }
            if (sd.OvernightAllowance == 0)
            {
                this.OvernightAllowance = false;
            }
            else if (sd.OvernightAllowance == 1)
            {
                this.OvernightAllowance = true;
            }
            else
            {
                throw new Exception("Unknown Overnight Allowance value: " + sd.OvernightAllowance);
            }
            if (sd.BarrierPayment == 0)
            {
                this.BarrierPayment = false;
            }
            else if (sd.BarrierPayment == 1)
            {
                this.BarrierPayment = true;
            }
            else
            {
                throw new Exception("Unknown Barrier Payment value: " + sd.BarrierPayment);
            }
            this.TravelTimeToSite = sd.TravelToSiteTime;
            this.TravelTimeFromSite = sd.TravelFromSiteTime;
            this.TotalTravelTime = sd.TotalTravelTime;
            this.TotalOnsiteTime = sd.TotalOnsiteTime;
            this.DailyReport = sd.DailyReport;
            this.PartsSuppliedToday = sd.PartsSuppliedToday;
            this.DtReport = sd.DtReport;
            this.ServiceDayModel = sd;
        }

        public void Save()
        {
            if (ServiceDayModel == null)
            {
                ServiceDayModel = new ServiceDay();
            }
            ServiceDayModel.ArrivalOnsiteTime = m_arrivalOnsiteTime;
            ServiceDayModel.BarrierPayment = m_barrierPayment;
            ServiceDayModel.DailyAllowance = m_dailyAllowance;
            ServiceDayModel.DailyReport = m_dailyReport;
            ServiceDayModel.DepartureSiteTime = m_departureSiteTime;
            ServiceDayModel.DtReport = m_dtReport;
            ServiceDayModel.Mileage = m_mileage;
            ServiceDayModel.OvernightAllowance = m_overnightAllowance;
            ServiceDayModel.PartsSuppliedToday = m_partsSuppliedToday;
            ServiceDayModel.TotalOnsiteTime = m_totalOnsiteTime;
            ServiceDayModel.TotalTravelTime = m_totalTravelTime;
            ServiceDayModel.TravelEndTime = m_travelEndTime;
            ServiceDayModel.TravelFromSiteTime = m_travelTimeFromSite;
            ServiceDayModel.TravelStartTime = m_travelStartTime;
            ServiceDayModel.TravelToSiteTime = m_travelTimeToSite;
            ServiceDayModel.ServiceSheet = ParentServiceSheetVM.ServiceSubmission;
        }

        //RT 23/1/17 - This is used to display the date on the expander.  Shows date and day
        public string ReportDateAndDay
        {
            get
            {
                string day = DtReport.DayOfWeek.ToString();
                return DtReport.ToShortDateString() + " - " + day;
            }
        }

        public DateTime TravelStartTime
        {
            get
            {
                return m_travelStartTime;
            }

            set
            {
                m_travelStartTime = value;
                onPropertyChanged("TravelStartTime");
                //If the travel start is altered, then the travel to site needs recalculating
                recalculateTravelTimeToSite();
            }
        }

        public DateTime ArrivalOnsiteTime
        {
            get
            {
                return m_arrivalOnsiteTime;
            }

            set
            {
                m_arrivalOnsiteTime = value;
                onPropertyChanged("ArrivalOnsiteTime");
                recalculateTravelTimeToSite();
                recalculateTimeOnsite();
            }
        }

        public DateTime DepartureSiteTime
        {
            get
            {
                return m_departureSiteTime;
            }

            set
            {
                m_departureSiteTime = value;
                onPropertyChanged("DepartureSiteTime");
                recalculateTimeOnsite();
                recalculateTravelFromSite();
            }
        }

        public DateTime TravelEndTime
        {
            get
            {
                return m_travelEndTime;
            }

            set
            {
                m_travelEndTime = value;
                onPropertyChanged("TravelEndTime");
                recalculateTravelFromSite();
            }
        }

        public int Mileage
        {
            get
            {
                return m_mileage;
            }

            set
            {
                m_mileage = value;
                onPropertyChanged("Mileage");
            }
        }

        public bool DailyAllowance
        {
            get
            {
                if (m_dailyAllowance == 1)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }

            set
            {
                if (value)
                {
                    m_dailyAllowance = 1;
                }
                else
                {
                    m_dailyAllowance = 0;
                }
                onPropertyChanged("DailyAllowance");
                
                //RT 29/11/16 - If we haven't set the parentVM, then we are just setting up the days.
                if (ParentServiceSheetVM != null)
                {
                    ParentServiceSheetVM.recalculateDailyAllowances(); 
                }
            }
        }

        public bool OvernightAllowance
        {
            get
            {
                if (m_overnightAllowance == 1)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }

            set
            {
                if (value)
                {
                    m_overnightAllowance = 1;
                }
                else
                {
                    m_overnightAllowance = 0;
                }
                onPropertyChanged("OvernightAllowance");
                //RT 29/11/16 - If we haven't set the parentVM, then we are just setting up the days.
                if (ParentServiceSheetVM != null)
                {
                    ParentServiceSheetVM.recalculateOvernightAllowances();
                }
            }
        }

        public bool BarrierPayment
        {
            get
            {
                if (m_barrierPayment == 1)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }

            set
            {
                if (value)
                {
                    m_barrierPayment = 1;
                }
                else
                {
                    m_barrierPayment = 0;
                }
                onPropertyChanged("BarrierPayment");
                //RT 29/11/16 - If we haven't set the parentVM, then we are just setting up the days.
                if (ParentServiceSheetVM != null)
                {
                    ParentServiceSheetVM.recalculateBarrierPayments();
                }
            }
        }

        public double TravelTimeToSite
        {
            get
            {
                return m_travelTimeToSite;
            }

            set
            {
                m_travelTimeToSite = value;
                onPropertyChanged("TravelTimeToSite");
            }
        }

        public double TravelTimeFromSite
        {
            get
            {
                return m_travelTimeFromSite;
            }

            set
            {
                m_travelTimeFromSite = value;
                onPropertyChanged("TravelTimeFromSite");
            }
        }

        public double TotalTravelTime
        {
            get
            {
                return m_totalTravelTime;
            }

            set
            {
                m_totalTravelTime = value;
                onPropertyChanged("TotalTravelTime");
            }
        }

        public double TotalOnsiteTime
        {
            get
            {
                return m_totalOnsiteTime;
            }

            set
            {
                m_totalOnsiteTime = value;
                onPropertyChanged("TotalOnsiteTime");
            }
        }

        public string DailyReport
        {
            get
            {
                return m_dailyReport;
            }

            set
            {
                m_dailyReport = value;
                onPropertyChanged("DailyReport");
            }
        }

        public string PartsSuppliedToday
        {
            get
            {
                return m_partsSuppliedToday;
            }

            set
            {
                m_partsSuppliedToday = value;
                onPropertyChanged("PartsSuppliedToday");
            }
        }

        public DateTime DtReport
        {
            get
            {
                return m_dtReport;
            }

            set
            {
                m_dtReport = value;
                onPropertyChanged("DtReport");
            }
        }

        public void CancelEdit()
        {
            ArrivalOnsiteTime = ServiceDayModel.ArrivalOnsiteTime;
            if (ServiceDayModel.BarrierPayment == 1)
            {
                BarrierPayment = true;
            }
            else
            {
                BarrierPayment = false;
            }
            
            if (ServiceDayModel.DailyAllowance == 1)
            {
                DailyAllowance = true;
            }
            else
            {
                DailyAllowance = false;
            }
            DailyReport = ServiceDayModel.DailyReport;
            DepartureSiteTime = ServiceDayModel.DepartureSiteTime;
            DtReport = ServiceDayModel.DtReport;
            Mileage = ServiceDayModel.Mileage;

            if (ServiceDayModel.OvernightAllowance == 1)
            {
                OvernightAllowance = true;
            }
            else
            {
                OvernightAllowance = false;
            }
            
            PartsSuppliedToday = ServiceDayModel.PartsSuppliedToday;
            TotalOnsiteTime = ServiceDayModel.TotalOnsiteTime;
            TotalTravelTime = ServiceDayModel.TotalTravelTime;
            TravelEndTime = ServiceDayModel.TravelEndTime;
            TravelTimeFromSite = ServiceDayModel.TravelFromSiteTime;
            TravelStartTime = ServiceDayModel.TravelStartTime;
            TravelTimeToSite = ServiceDayModel.TravelToSiteTime;
        }
        public ServiceDay ServiceDayModel
        {
            get
            {
                return m_serviceDayModel;
            }

            set
            {
                m_serviceDayModel = value;
                onPropertyChanged("ServiceDayModel");
            }
        }

        public ServiceSheetViewModel ParentServiceSheetVM
        {
            get
            {
                return m_parentServiceSheetVM;
            }

            set
            {
                m_parentServiceSheetVM = value;
                onPropertyChanged("ParentServiceSheetVM");
            }
        }

        //public ServiceDayViewModel(ServiceDay currentDay, ServiceSheetViewModel parentVM)
        //{
        //    ServiceDay = currentDay;
        //    ParentServiceDayVM = parentVM;
        //}

        //public ServiceDayViewModel(ServiceSheetViewModel parentVM)
        //{
        //    ServiceDay = new ServiceDay();
        //    ParentServiceDayVM = parentVM;
        //}

        //public DateTime ServiceDate
        //{
        //    get
        //    {
        //        return ServiceDay.DtReport;
        //    }
        //    set
        //    {
        //        ServiceDay.DtReport = value;
        //        onPropertyChanged("ServiceDate");
        //    }
        //}

        //public DateTime TravelStartTime
        //{
        //    get
        //    {
        //        return ServiceDay.TravelStartTime;
        //    }
        //    set
        //    {
        //        ServiceDay.TravelStartTime = value;
        //        onPropertyChanged("TravelStartTime");
        //        //If the travel start is altered, then the travel to site needs recalculating
        //        recalculateTravelTimeToSite();
        //    }
        //}

        private void recalculateTravelTimeToSite()
        {
           //RT 29/11/16 - If we haven't set the parentVM, then we are just setting up the days.
           if (ParentServiceSheetVM == null)
            {
                return;
            }
           TimeSpan travelToSite = ArrivalOnsiteTime - TravelStartTime;
           TravelTimeToSite = travelToSite.TotalHours;
           recalculateTotalTravelTime();
        }

        private void recalculateTotalTravelTime()
        {
            //RT 29/11/16 - If we haven't set the parentVM, then we are just setting up the days.
            if (ParentServiceSheetVM == null)
            {
                return;
            }
            TotalTravelTime = TravelTimeToSite + TravelTimeFromSite;
            ParentServiceSheetVM.recalculateTravelTime();
        }

        private void recalculateTimeOnsite()
        {
            //RT 29/11/16 - If we haven't set the parentVM, then we are just setting up the days.
            if (ParentServiceSheetVM == null)
            {
                return;
            }
            TimeSpan timeOnsite = DepartureSiteTime - ArrivalOnsiteTime;
            TotalOnsiteTime = timeOnsite.TotalHours;
            //Need to recalculate the total time on the holding service sheet
            ParentServiceSheetVM.recalulateTimeOnsite();
        }

        private void recalculateTravelFromSite()
        {
            //RT 29/11/16 - If we haven't set the parentVM, then we are just setting up the days.
            if (ParentServiceSheetVM == null)
            {
                return;
            }
            TimeSpan travelFrom = TravelEndTime - DepartureSiteTime;
            TravelTimeFromSite = travelFrom.TotalHours;
            recalculateTotalTravelTime();
        }

        public static ServiceDayViewModel createServiceDayForHolidayAbsence(DateTime currentDate)
        {
            //Creates a day for a holiday / absence.  May not need to complete all fields
            ServiceDayViewModel retval = new ServiceDayViewModel();
            DayOfWeek currentDay = currentDate.DayOfWeek;

            if (currentDay.Equals(DayOfWeek.Saturday) || currentDay.Equals(DayOfWeek.Sunday))
            {
                //We don't set it for weekends
                return null;
            }

            //Set the items which don't change by the day
            retval.DailyReport = "";
            retval.DtReport = currentDate;
            retval.Mileage = 0;
            retval.OvernightAllowance = false;
            retval.PartsSuppliedToday = "";
            retval.TravelTimeFromSite = 0;
            retval.TravelTimeToSite = 0;

            //Day always starts at 8 am.
            TimeSpan dayStartTime = new TimeSpan(8, 0, 0);
            DateTime dayStartDateTime = currentDate.Date + dayStartTime;
            DateTime dayEndDateTime = new DateTime();
            int totalTimeOnsite = 0;

            switch (currentDay)
            {
                case DayOfWeek.Monday:
                case DayOfWeek.Tuesday:
                case DayOfWeek.Wednesday:
                case DayOfWeek.Thursday:
                    totalTimeOnsite = 8;
                    dayEndDateTime = dayStartDateTime.AddHours(totalTimeOnsite);
                    break;
                case DayOfWeek.Friday:
                    totalTimeOnsite = 6;
                    dayEndDateTime = dayStartDateTime.AddHours(totalTimeOnsite);
                    break;
                default:
                    throw new Exception("Unknown day: " + currentDay.ToString());
            }

            retval.TravelStartTime = dayStartDateTime;
            retval.ArrivalOnsiteTime = dayStartDateTime;
            retval.DepartureSiteTime = dayEndDateTime;
            retval.TravelEndTime = dayEndDateTime;
            retval.TotalOnsiteTime = totalTimeOnsite;
            retval.TotalTravelTime = 0;

            return retval;
        }

        //public DateTime ArrivalOnsiteTime
        //{
        //    get
        //    {
        //        return ServiceDay.ArrivalOnsiteTime;
        //    }
        //    set
        //    {
        //        ServiceDay.ArrivalOnsiteTime = value;
        //        onPropertyChanged("ArrivalOnsiteTime");
        //        recalculateTravelTimeToSite();
        //        recalculateTimeOnsite();
        //    }
        //}



        //public DateTime DepartSiteTime
        //{
        //    get
        //    {
        //        return ServiceDay.DepartureSiteTime;
        //    }
        //    set
        //    {
        //        ServiceDay.DepartureSiteTime = value;
        //        onPropertyChanged("DepartSiteTime");
        //        recalculateTimeOnsite();
        //        recalculateTravelFromSite();
        //    }
        //}



        //public DateTime TravelEndTime
        //{
        //    get
        //    {
        //        return ServiceDay.TravelEndTime;
        //    }
        //    set
        //    {
        //        ServiceDay.TravelEndTime = value;
        //        onPropertyChanged("TravelEndTime");
        //        recalculateTravelFromSite();
        //    }
        //}

        //public int Mileage
        //{
        //    get
        //    {
        //        return ServiceDay.Mileage;
        //    }
        //    set
        //    {
        //        ServiceDay.Mileage = value;
        //        onPropertyChanged("Mileage");
        //        ParentServiceDayVM.recalculateMileage();
        //    }
        //}

        //public bool DailyAllowance
        //{
        //    get
        //    {
        //        if (ServiceDay.DailyAllowance == 1)
        //        {
        //            return true;
        //        }
        //        else
        //        {
        //            return false;
        //        }
        //    }
        //    set
        //    {
        //        if (value)
        //        {
        //            ServiceDay.DailyAllowance = 1;
        //        }
        //        else
        //        {
        //            ServiceDay.DailyAllowance = 0;
        //        }
        //        onPropertyChanged("DailyAllowance");
        //        //Update the daily allowance total on the parent VM
        //        ParentServiceDayVM.recalculateDailyAllowances();
        //    }
        //}

        //public bool OvernightAllowance
        //{
        //    get
        //    {
        //        if (ServiceDay.OvernightAllowance == 1)
        //        {
        //            return true;
        //        }
        //        else
        //        {
        //            return false;
        //        }
        //    }
        //    set
        //    {
        //        if (value)
        //        {
        //            ServiceDay.OvernightAllowance = 1;
        //        }
        //        else
        //        {
        //            ServiceDay.OvernightAllowance = 0;
        //        }
        //        onPropertyChanged("OvernightAllowance");
        //        ParentServiceDayVM.recalculateOvernightAllowances();
        //    }
        //}

        //public bool BarrierPayment
        //{
        //    get
        //    {
        //        if (ServiceDay.BarrierPayment == 1)
        //        {
        //            return true;
        //        }
        //        else
        //        {
        //            return false;
        //        }
        //    }
        //    set
        //    {
        //        if (value)
        //        {
        //            ServiceDay.BarrierPayment = 1;
        //        }
        //        else
        //        {
        //            ServiceDay.BarrierPayment = 0;
        //        }
        //        onPropertyChanged("BarrierPayment");
        //        ParentServiceDayVM.recalculateBarrierPayments();
        //    }
        //}

        //public double TravelTimeToSite
        //{
        //    get
        //    {
        //        return ServiceDay.TravelToSiteTime;
        //    }
        //    set
        //    {
        //        ServiceDay.TravelToSiteTime = value;
        //        onPropertyChanged("TravelTimeToSite");
        //    }
        //}

        //public double TravelTimeFromSite
        //{
        //    get
        //    {
        //        return ServiceDay.TravelFromSiteTime;
        //    }
        //    set
        //    {
        //        ServiceDay.TravelFromSiteTime = value;
        //        onPropertyChanged("TravelTimeFromSite");
        //    }
        //}

        //public double TotalTravelTime
        //{
        //    get
        //    {
        //        return ServiceDay.TotalTravelTime;
        //    }
        //    set
        //    {
        //        ServiceDay.TotalTravelTime = value;
        //        onPropertyChanged("TotalTravelTime");
        //    }
        //}

        //public double TotalTimeOnsite
        //{
        //    get
        //    {
        //        return ServiceDay.TotalOnsiteTime;
        //    }
        //    set
        //    {
        //        ServiceDay.TotalOnsiteTime = value;
        //        onPropertyChanged("TotalTimeOnsite");
        //    }
        //}

        //public string DailyReport
        //{
        //    get
        //    {
        //        return ServiceDay.DailyReport;
        //    }
        //    set
        //    {
        //        ServiceDay.DailyReport = value;
        //        onPropertyChanged("DailyReport");
        //    }
        //}

        //public string PartsSupplied
        //{
        //    get
        //    {
        //        return ServiceDay.PartsSuppliedToday;
        //    }
        //    set
        //    {
        //        ServiceDay.PartsSuppliedToday = value;
        //        onPropertyChanged("PartsSupplied");
        //    }
        //}

        //public ServiceSheetViewModel ParentServiceDayVM
        //{
        //    get
        //    {
        //        return ParentServiceDayVM1;
        //    }

        //    set
        //    {
        //        ParentServiceDayVM1 = value;
        //    }
        //}


    }
}
