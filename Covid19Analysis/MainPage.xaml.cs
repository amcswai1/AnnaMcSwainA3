using System;
using System.Collections.Generic;
using Windows.Foundation;
using Windows.Storage;
using Windows.Storage.Pickers;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Covid19Analysis.Model;
using Covid19Analysis.View;
using System.Linq;
using Windows.UI.Xaml.Media;
using Covid19Analysis.IO;
using Covid19Analysis.Utils;

namespace Covid19Analysis
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage
    {
        #region Data members

        /// <summary>
        ///     The application height
        /// </summary>
        public const int ApplicationHeight = 425;


        /// <summary>
        ///     The application width
        /// </summary>
        public const int ApplicationWidth = 625;

        private int upperBoundInput;
        private int lowerBoundInput;
        private List<CovidStatisticsError> errors = new List<CovidStatisticsError>();
        private List<CovidStatistic> statisticsCurrentlyLoaded = new List<CovidStatistic>();
        private List<CovidStatistic> statisticsToAddOrReplace = new List<CovidStatistic>();
        private List<CovidStatistic> duplicatesFound = new List<CovidStatistic>();
        #endregion

        #region Constructors

        /// <summary>
        ///     Initializes a new instance of the <see cref="MainPage" /> class.
        /// </summary>
        public MainPage()
        {
            this.InitializeComponent();
            this.summaryTextBox.FontFamily = new FontFamily("Monospaced");
            ApplicationView.PreferredLaunchViewSize = new Size {Width = ApplicationWidth, Height = ApplicationHeight};
            ApplicationView.PreferredLaunchWindowingMode = ApplicationViewWindowingMode.PreferredLaunchViewSize;
            ApplicationView.GetForCurrentView().SetPreferredMinSize(new Size(ApplicationWidth, ApplicationHeight));
            this.upperBoundInput = int.Parse(this.upperBoundTextBox.Text);
            this.lowerBoundInput = int.Parse(this.lowerBoundTextBox.Text);

        }

        #endregion

        private async void loadFile_Click(object sender, RoutedEventArgs e)
        {
            FileOpenPicker openPicker = new FileOpenPicker {
                SuggestedStartLocation = PickerLocationId.DocumentsLibrary, ViewMode = PickerViewMode.Thumbnail
            };
            openPicker.FileTypeFilter.Add(".csv");
            openPicker.FileTypeFilter.Add(".txt");

            StorageFile file = await openPicker.PickSingleFileAsync();

            string text = await FileIO.ReadTextAsync(file);
            
            if (this.summaryTextBox.Text == "Summary")
            {
                CovidCsvParser parser = new CovidCsvParser();
                this.statisticsCurrentlyLoaded = parser.ParseText(text).ToList();
                this.errors = parser.Errors.ToList();
                this.setCovidDataToSummaryBox();
            }
            else
            {
                this.statisticsToAddOrReplace.Clear();
                CovidCsvParser parser = new CovidCsvParser();
                this.statisticsToAddOrReplace = parser.ParseText(text).ToList();
                this.errors = parser.Errors.ToList();
                this.displayMergeOption();


            }
        }

        private void setCovidDataToSummaryBox()
        {
            CovidStatisticsReportBuilder stringBuilder = new CovidStatisticsReportBuilder(this.statisticsCurrentlyLoaded);
            stringBuilder.SetUpperAndLowerBoundLimits(this.upperBoundInput, this.lowerBoundInput);
            this.summaryTextBox.Text = "Summary" + Environment.NewLine + stringBuilder.BuildStringForOutput();
        }
        private async void displayMergeOption()
        {
            MergeContentDialog mergeDialog = new MergeContentDialog();
            var result = await mergeDialog.ShowAsync();
            if (result == ContentDialogResult.Primary)
            {
                this.findAllDuplicates();
                this.displayApplyToAllDialog();                   
            }
            else
            {
                this.statisticsCurrentlyLoaded = this.statisticsToAddOrReplace;
                this.setCovidDataToSummaryBox();
            }
        }
        private void findAllDuplicates()
        {
            var duplicates = this.statisticsToAddOrReplace.Where(existingStatistic => this.statisticsToAddOrReplace.Exists(newStatistic => existingStatistic.Date == newStatistic.Date)).ToList();
            this.duplicatesFound = duplicates;
            
        }

        private async void displayApplyToAllDialog()
        {
            
            foreach (var duplicate in this.duplicatesFound)
            {
               CovidStatisticsMerger merger = new CovidStatisticsMerger(this.statisticsCurrentlyLoaded, this.statisticsToAddOrReplace, this.duplicatesFound);
                KeepOrReplaceContentDialog keepOrReplaceDialog = new KeepOrReplaceContentDialog();
                var result = await keepOrReplaceDialog.ShowAsync();
                if (result == ContentDialogResult.Primary)
                {
                    if (keepOrReplaceDialog.CheckBoxStatus)
                    {
                        this.statisticsCurrentlyLoaded = merger.KeepAllExistingData().ToList();
                        this.statisticsToAddOrReplace.Clear();
                        return;
                    }
                    else
                    {
                        merger.KeepExistingDataForCurrentDuplicate(duplicate);
                    }
                }
                else
                {
                    if (keepOrReplaceDialog.CheckBoxStatus)
                    {
                        this.statisticsCurrentlyLoaded = merger.ReplaceAllExistingDuplicateDataWithNewData().ToList();
                        this.statisticsToAddOrReplace.Clear();
                        return;
                    }
                    else
                    {
                        merger.ReplaceOneCurrentDataWithNewData(duplicate);
                    }
                }
            }

            this.statisticsCurrentlyLoaded =
                this.statisticsCurrentlyLoaded.Concat(this.statisticsToAddOrReplace).ToList();
            this.setCovidDataToSummaryBox();

        }
        private void upperBound_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (this.upperBoundTextBox.Text == string.Empty)
            {
                this.upperBoundInput = 0;
                this.setCovidDataToSummaryBox();
            }
            else
            { 
                this.upperBoundInput = int.Parse(this.upperBoundTextBox.Text);
                this.setCovidDataToSummaryBox();
            }
            
        }

        private void lowerBound_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (this.lowerBoundTextBox.Text == string.Empty)
            {
                this.lowerBoundInput = 0;
                this.setCovidDataToSummaryBox();
            }
            else
            {
                this.lowerBoundInput = int.Parse(this.lowerBoundTextBox.Text);
                this.setCovidDataToSummaryBox();
            }
        }

        private async void clear_Data_Click(object sender, RoutedEventArgs e)
        {
            this.duplicatesFound.Clear();
            this.statisticsToAddOrReplace.Clear();
            this.statisticsCurrentlyLoaded.Clear();
            this.summaryTextBox.Text = "Summary";
        } 

        private async void view_Errors_Click(object sender, RoutedEventArgs e)
        {
            string content = this.ErrorsToString();
            ContentDialog errorsFound = new ContentDialog() {
                Title = "Errors in file most recently added:",
                Content = content,
                CloseButtonText = "Close"
            };
            await errorsFound.ShowAsync();
        }

        /// <summary>
        /// Errors to string.
        /// </summary>
        /// <returns>he list of errors in string format</returns>
        public string ErrorsToString()
        {
            string output = string.Empty;
            if (this.errors.Count == 0)
            {
                output += "No errors found!";
            }
            else
            {
                foreach (CovidStatisticsError currentError in this.errors)
                {
                    output += "Line " + currentError.LineErrorWasFound + ": " + string.Join(",", currentError.ErrorLineContents) + Environment.NewLine;
                }
            }

            return output;
        }
    }
}
    
    


