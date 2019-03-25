using DevExpress.DashboardCommon;
using DevExpress.DashboardWin.Localization;
using DevExpress.DataAccess.Excel;
using DevExpress.XtraEditors;
using System;

namespace DateFilterDashboardItemSample
{
    public partial class Form1 : XtraForm
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            DashboardExcelDataSource excelDataSource = CreateExcelDataSource();
            Dashboard dBoard = new Dashboard();
            dBoard.DataSources.Add(excelDataSource);

            dBoard.BeginUpdate();
            // Create dashboard items.
            ChartDashboardItem chart = CreateChart(excelDataSource);
            dBoard.Items.Add(chart);
            DateFilterDashboardItem dateFilterItem = CreateDateFilterItem(excelDataSource);
            dBoard.Items.Add(dateFilterItem);
            DashboardItemGroup group = CreateGroup();
            dBoard.Groups.Add(group);
            group.AddRange(dateFilterItem, chart);
            // Create layout tree.
            DashboardLayoutItem dateFilterLayoutItem = new DashboardLayoutItem(dateFilterItem, 30);
            DashboardLayoutItem chartLayoutItem = new DashboardLayoutItem(chart, 70);
            DashboardLayoutGroup groupLayoutItem = new DashboardLayoutGroup(group, 100);
            groupLayoutItem.ChildNodes.AddRange(dateFilterLayoutItem, chartLayoutItem);
            DashboardLayoutGroup rootGroup = new DashboardLayoutGroup(null, 100);
            rootGroup.ChildNodes.Add(groupLayoutItem);
            rootGroup.Orientation = DashboardLayoutGroupOrientation.Vertical;
            dBoard.LayoutRoot = rootGroup;
            dBoard.EndUpdate();

            dashboardViewer1.Dashboard = dBoard;
        }

        private DashboardItemGroup CreateGroup()
        {
            DashboardItemGroup group = new DashboardItemGroup();
            group.Name = "Sales by Date";
            return group;
        }

        private DateFilterDashboardItem CreateDateFilterItem(DashboardExcelDataSource excelDataSource)
        {
            DateFilterDashboardItem dateFilter = new DateFilterDashboardItem();
            dateFilter.Name = string.Empty;
            dateFilter.ShowCaption = false;
            dateFilter.DataSource = excelDataSource;
            dateFilter.Dimension = new Dimension("orderDateId", "OrderDate", DateTimeGroupInterval.DayMonthYear);
            dateFilter.Dimension.DateTimeFormat.DateTimeFormat = DateTimeFormat.Short;
            dateFilter.ArrangementMode = DateFilterArrangementMode.Vertical;
            dateFilter.FilterType = DateFilterType.Between;
            dateFilter.VisibleComponents = DateFilterComponent.QuickFilters;
            dateFilter.DateTimePeriods.AddRange(
                   new DateTimePeriod
                   {
                       Name = DashboardWinLocalizer.GetString(DashboardWinStringId.PeriodLastYear),
                       Start = new FlowDateTimePeriodLimit
                       {
                           Interval = DateTimeInterval.Year,
                           Offset = -1
                       },
                       End = new FlowDateTimePeriodLimit
                       {
                           Interval = DateTimeInterval.Year,
                           Offset = 0
                       }
                   },
                   new DateTimePeriod
                   {
                       Name = DashboardWinLocalizer.GetString(DashboardWinStringId.PeriodNextSevenDays),
                       Start = new FlowDateTimePeriodLimit
                       {
                           Interval = DateTimeInterval.Day,
                           Offset = 1
                       },
                       End = new FlowDateTimePeriodLimit
                       {
                           Interval = DateTimeInterval.Day,
                           Offset = 8
                       }
                   },
                   new DateTimePeriod
                   {
                       Name = DashboardWinLocalizer.GetString(DashboardWinStringId.PeriodMonthToDate),
                       Start = new FlowDateTimePeriodLimit
                       {
                           Interval = DateTimeInterval.Month,
                           Offset = 0
                       },
                       End = new FlowDateTimePeriodLimit
                       {
                           Interval = DateTimeInterval.Day,
                           Offset = 1
                       }
                   },
                   new DateTimePeriod
                   {
                       Name = "Jul-18-2018 - Jan-18-2019",
                       Start = new FixedDateTimePeriodLimit
                       {
                           Date = new DateTime(2018, 7, 18)
                       },
                       End = new FixedDateTimePeriodLimit
                       {
                           Date = new DateTime(2019, 1, 18)
                       }
                   }
            );
            return dateFilter;
        }

        private ChartDashboardItem CreateChart(DashboardExcelDataSource excelDataSource)
        {
            ChartDashboardItem chart = new ChartDashboardItem();
            chart.Name = string.Empty;
            chart.ShowCaption = false;
            chart.DataSource = excelDataSource;
            chart.Arguments.Add(new Dimension("OrderDate", DateTimeGroupInterval.DayMonthYear));
            chart.Panes.Add(new ChartPane());
            SimpleSeries salesAmountSeries = new SimpleSeries(SimpleSeriesType.Line);
            salesAmountSeries.Value = new Measure("Extended Price");
            chart.Panes[0].Series.Add(salesAmountSeries);
            return chart;
        }

        private DashboardExcelDataSource CreateExcelDataSource()
        {
            DashboardExcelDataSource excelDataSource = new DashboardExcelDataSource();
            excelDataSource.FileName = "SalesPerson.xlsx";
            ExcelWorksheetSettings worksheetSettings = new ExcelWorksheetSettings("Data");
            excelDataSource.SourceOptions = new ExcelSourceOptions(worksheetSettings);
            excelDataSource.Fill();
            return excelDataSource;
        }
    }
}
