namespace Covid19Analysis.View
{
    /// <summary>
    ///     Represents the dialog for keeping or replacing data
    /// </summary>
    /// <seealso cref="Windows.UI.Xaml.Controls.ContentDialog" />
    /// <seealso cref="Windows.UI.Xaml.Markup.IComponentConnector" />
    /// <seealso cref="Windows.UI.Xaml.Markup.IComponentConnector2" />
    public sealed partial class KeepOrReplaceContentDialog
    {
        #region Properties

        /// <summary>
        ///     Gets a value indicating whether checkbox is checked or not.
        /// </summary>
        /// <value>
        ///     <c>true</c> if checked; otherwise, <c>false</c>.
        /// </value>
        public bool CheckBoxStatus { get; private set; }

        #endregion

        #region Constructors

        /// <summary>
        ///     Initializes a new instance of the <see cref="KeepOrReplaceContentDialog" /> class.
        /// </summary>
        public KeepOrReplaceContentDialog()
        {
            this.InitializeComponent();
        }

        #endregion

    }
}